import { ChapterDeck } from "./chapter-deck";
import { Suits } from "./suits";

export type ChapterDecks = { [suit: string]: ChapterDeck };

export const ChapterDecks = function(this: ChapterDecks) {
    if (!new.target) throw new Error(`Constructor cannot be invoked without 'new'`);
    for (const suit of Suits)
        this[suit] = [];
} as unknown as new() => ChapterDecks;
