import { Chapter } from "./chapter";
import { Suit } from "./suit";

export class ChapterChoice {
    public readonly chapter: Chapter;
    public readonly asSuit: Suit;

    public constructor(chapter: Chapter, asSuit?: Suit) {
        if (chapter.wild) {
            if (!asSuit) throw new Error("When selecting a wild chapter, a suit must be specified.");
        } else {
            if (asSuit && asSuit !== chapter.suit) throw new Error("When selecting a non-wild chapter, a different suit cannot be specified.");
        }
        this.chapter = chapter;
        this.asSuit = asSuit ?? this.chapter.suit;
    }
}
