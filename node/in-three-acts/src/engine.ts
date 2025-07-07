import { Chapter, ChapterChoice, Game, Goal, Player, Suits, StoryPhase } from "./model";

export class Engine {
    private readonly game: Game;

    public constructor(game: Game) {
        this.game = game;
    }

    public async start(): Promise<Player> {
        for (const player of this.game.players)
            this.drawGoal(player);
        let winner: Player | undefined = undefined;
        while(true) {
            await this.nextTurn();
            winner = this.getWinner();
            if (winner) break;
            this.game.nextPlayer();
        }
        return winner;
    }

    private async nextTurn(): Promise<void> {
        const author = this.game.author;
        const choicePromise = this.game.lastChapter
            ? this.nextChapter()
            : this.firstChapter();
        const choice = await choicePromise;
        const chapter = choice.chapter;
        const chapterDeck = author.chapters[choice.asSuit];
        if (chapter.wild && chapterDeck.some(existing => existing.wild))
            throw new Error('Cannot play two wild cards to the same suit.');
        chapterDeck.push(chapter);
        this.game.lastChapter = chapter;

        let phase: StoryPhase;
        if (author.hasFailed) {
            author.book.ending = '~ Manuscript Rejected ~';
            phase = StoryPhase.Failure;
        } else if (!author.hasAllGoals) {
            if (!author.hasCompletedCurrentGoals) {
                phase = StoryPhase.Exposition;
            } else {
                this.drawGoal(author);
                phase = StoryPhase.PlotTwist;
            }
        } else {
            if (!author.hasCompletedCurrentGoals) {
                phase = StoryPhase.Resoultion;
            } else {
                author.book.ending = 'The End';
                phase = StoryPhase.Conclusion;
            }
        }
        await author.brain.writeChapter(choice.chapter, phase);
    }

    private async firstChapter(): Promise<ChapterChoice> {
        const chapters = Object.values(this.game.chapters).map(deck => deck[0]);
        const choice = await this.chooseChapter(chapters);
        this.game.chapters[choice.chapter.suit].shift();
        return choice;
    }

    private async nextChapter(): Promise<ChapterChoice> {
        const chapterDecks = this.game.chapters;
        const chapters = this.game.lastChapter!.inspires.map(suit => chapterDecks[suit].shift()!)
        const choice = await this.chooseChapter(chapters);
        const i = chapters.indexOf(choice.chapter);
        chapters.splice(i, 1);
        for (const chapter of chapters)
            chapterDecks[chapter.suit].push(chapter);
        return choice;
    }

    private async chooseChapter(chapters: Chapter[]): Promise<ChapterChoice> {
        const author = this.game.author;
        const writtenChapters = author.chapters;
        const wildSuits = Suits.filter(suit => writtenChapters[suit].every(chapter => !chapter.wild));
        return await author.brain.chooseChapter(chapters, wildSuits, this.game.getPublicKnowledge());
    }

    private drawGoal(author: Player) {
        const completedSuits = author.goals.flatMap(goal => Object.keys(goal.suitCounts));
        const goals = this.game.goals;
        let goal: Goal;
        while (true) {
            goal = goals.shift()!;
            if (Object.keys(goal.suitCounts).every(suit => !completedSuits.includes(suit))) break;
            this.game.discardedGoals.push(goal);
        }
        author.goals.push(goal);
    }

    private getWinner(): Player | undefined {
        const author = this.game.author;
        const character = this.game.character;
        if (author.hasFailed) return character;
        if (author.hasAllGoals && author.hasCompletedCurrentGoals) return author;
        return undefined;
    }
}
