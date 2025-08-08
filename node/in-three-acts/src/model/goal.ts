import { ChapterDecks } from './chapter-decks';
import { Suit } from "./suit";
import { SuitCount } from './suit-count';

export class Goal {
    public static of5(suit1: Suit, suit2: Suit, name: string) {
        if (suit1 == suit2) throw new Error("A goal must have different suits.")
        return new Goal({[suit1]: 5, [suit2]: 5}, name);
    }

    public static of7(suit: Suit, name: string) {
        return new Goal({[suit]: 7}, name);
    }

    public readonly name: string;
    public readonly suitCounts: SuitCount;

    private constructor(suitCounts: SuitCount, name: string) {
        this.name = name;
        this.suitCounts = suitCounts;
    }

    public chaptersCompleted(chapters: ChapterDecks): {count: number, total: number} {
        const suitCounts = this.suitCounts;
        let count = 0;
        let total = 0;
        for (const suit in suitCounts) {
            count += Math.min(suitCounts[suit], chapters[suit].length);
            total += suitCounts[suit];
        };
        return { count, total };
    }

    public getCompletedSuits(chapters: ChapterDecks): Suit[] {
        const suitCounts = this.suitCounts;
        return Object.keys(suitCounts).filter(suit => suitCounts[suit] <= chapters[suit].length);
    }
}
