import { Book } from "./book";
import { Chapter } from "./chapter";
import { ChapterDecks } from "./chapter-decks";
import { Game } from "./game";
import { Goal } from "./goal";
import { Suit } from "./suit";

export class PublicKnowledge {
    public readonly discardedGoals: Goal[];
    public readonly nextChapters: { [suit in Suit]: Chapter };
    public readonly players: PublicKnowledgePlayer[];
    public readonly author: PublicKnowledgePlayer;
    public readonly character: PublicKnowledgePlayer;

    public constructor(game: Game, expose: boolean = false) {
        const chapterDecks = game.chapters;

        this.discardedGoals = game.discardedGoals;
        this.nextChapters = {
            a: chapterDecks.a[0],
            h: chapterDecks.h[0],
            j: chapterDecks.j[0],
            r: chapterDecks.r[0],
            s: chapterDecks.s[0],
        };
        this.players = [0, 1].map(i => {
            const player = game.players[i];
            return {
                book: player.book,
                chapters: player.chapters,
                goals: player.goals.filter(goal => expose || goal.isCompleted),
                id: player.id,
                name: player.name,
            }
        });
        this.author = this.players[game.currentPlayerIndex];
        this.character = this.players[1 - game.currentPlayerIndex];
    }
}

export interface PublicKnowledgePlayer {
    book: Book;
    chapters: ChapterDecks;
    goals: Goal[];
    id: Object;
    name: string;
}
