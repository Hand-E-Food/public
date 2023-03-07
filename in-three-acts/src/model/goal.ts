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

    public isCompleted(chapters: ChapterDecks): boolean {
        return Object.keys(this.getOutstanding(chapters)).length === 0;
    }

    public getCompletedSuits(chapters: ChapterDecks): Suit[] {
        const suitCounts = this.suitCounts;
        return Object.keys(suitCounts).filter(suit => suitCounts[suit] <= chapters[suit].length);
    }

    public getOutstanding(chapters: ChapterDecks): SuitCount {
        const suitCounts = this.suitCounts;
        const result: SuitCount = {};
        Object.keys(suitCounts).forEach(suit => {
            const count = suitCounts[suit] - chapters[suit].length;
            if (count > 0) result[suit] = count;
        });
        return result;
    }
}
