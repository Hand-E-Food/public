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
            const my = game.players[i];
            const your = game.players[1 - i];
            return {
                chapters: my.chapters,
                goals: expose ? my.goals : my.goals.filter(goal => goal.isCompleted(your.chapters)),
                id: my.id,
                name: my.name,
            }
        });
        this.author = this.players[game.currentPlayerIndex];
        this.character = this.players[1 - game.currentPlayerIndex];
    }
}

export interface PublicKnowledgePlayer {
    chapters: ChapterDecks;
    goals: Goal[];
    id: Object;
    name: string;
}
