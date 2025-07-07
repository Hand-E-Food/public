import { Chapter } from "./chapter";
import { ChapterDecks } from "./chapter-decks";
import { Goal } from "./goal";
import { GoalDeck } from "./goal-deck";
import { Player } from "./player";
import { PublicKnowledge } from "./public-knowledge";

export class Game {
    public currentPlayerIndex: number = 0;

    public readonly chapters: ChapterDecks = new ChapterDecks();
    public readonly discardedGoals: Goal[] = [];
    public readonly goals: GoalDeck = [];
    public readonly openHand: boolean;
    public readonly players: Player[];

    /** The last written chapter. */
    public lastChapter?: Chapter;

    public constructor(players: Player[], openHand: boolean = false) {
        if (players.length != 2 || players[0] === players[1]) throw Error("Two different players must be specified.");
        this.players = players;
        this.openHand = openHand;
    }

    public get author(): Player {
        return this.players[this.currentPlayerIndex];
    }

    public get character(): Player {
        return this.players[1 - this.currentPlayerIndex];
    }

    public nextPlayer(): void {
        this.currentPlayerIndex = 1 - this.currentPlayerIndex;
    }

    public getPublicKnowledge(expose: boolean = false): PublicKnowledge {
        return new PublicKnowledge(this, this.openHand || expose);
    }
}
