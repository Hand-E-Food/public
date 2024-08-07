import { Chapter, ChapterChoice, Game, Goal, Player, Suits, MaxGoals, MaxChapters } from "./model";

export class Engine {
    private readonly game: Game;

    public constructor(game: Game) {
        this.game = game;
    }

    public async start(): Promise<Player> {
        this.game.players.forEach(player => this.drawGoal(player), this);
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
        const character = this.game.character;
        const chapterPromise = author.lastChapter
            ? this.nextChapter()
            : this.firstChapter();
        const chapter = await chapterPromise;
        character.receiveChapter(chapter);
        if (author.hasCompletedAllGoals(character) && author.goals.length < MaxGoals) this.drawGoal(author);
    }

    private async firstChapter(): Promise<ChapterChoice> {
        const chapters = Object.values(this.game.chapters).map(deck => deck[0]);
        const choice = await this.chooseChapter(chapters);
        this.game.chapters[choice.chapter.suit].shift();
        return choice;
    }

    private async nextChapter(): Promise<ChapterChoice> {
        const chapterDecks = this.game.chapters;
        const chapters = this.game.author.lastChapter.inspires.map(suit => chapterDecks[suit].shift()!)
        const choice = await this.chooseChapter(chapters);
        const i = chapters.indexOf(choice.chapter);
        chapters.splice(i, 1);
        chapters.forEach(chapter => chapterDecks[chapter.suit].push(chapter));
        return choice;
    }

    private async chooseChapter(chapters: Chapter[]): Promise<ChapterChoice> {
        const characterChapters = this.game.character.chapters;
        const wildSuits = Suits.filter(suit => characterChapters[suit].every(chapter => !chapter.wild));
        return await this.game.author.brain.chooseChapter(chapters, wildSuits, this.game.getPublicKnowledge());
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
        const character = this.game.character;
        if (character.chapters[character.lastChapter.suit].length > MaxChapters) return character;
        const author = this.game.author;
        if (author.hasCompletedAllGoals(character) && author.goals.length === MaxGoals) return author;
        return undefined;
    }
}
