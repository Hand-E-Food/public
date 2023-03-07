import { Chapter, ChapterDeck, Chapters, Game, GoalDeck, Goals, Player, Suits } from "./model";

export class GameFactory {
    public createGame(players: Player[], openHand: boolean = false): Game {
        const game: Game = new Game(players, openHand);
        this.createGoalDeck(game.goals);
        Suits.forEach(suit => this.createChapterDeck(game.chapters[suit], Chapters[suit]), this);
        return game;
    }

    private createGoalDeck(deck: GoalDeck): void {
        deck.push(...Goals);
        this.shuffle(deck);
    }

    private createChapterDeck(deck: ChapterDeck, chapters: Chapter[]): void {
        this.shuffle(chapters);
        const wilds = chapters.filter(chapter => chapter.wild);
        chapters = chapters.filter(chapter => !chapter.wild);
        const i = Math.floor(Math.random() * 6) + 10;
        for (let j = 0; j < 16; j++)
            deck.push((j === i ? wilds.pop() : chapters.pop())!);
    }

    private shuffle(array: any[]): void {
        for (let i = array.length - 1; i > 0; i--) {
            const j = Math.floor(Math.random() * (i + 1));
            [array[i], array[j]] = [array[j], array[i]];
        }
    }
}
