import { ChapterDeck } from "./chapter-deck";
import { Suits } from "./suits";

export type ChapterDecks = { [suit: string]: ChapterDeck };

export const ChapterDecks = function(this: ChapterDecks) {
    if (!new.target) throw new Error(`Constructor cannot be invoked without 'new'`);
    Suits.forEach(suit => this[suit] = [], this);
} as unknown as new() => ChapterDecks;
