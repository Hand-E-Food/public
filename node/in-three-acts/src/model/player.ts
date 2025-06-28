import { Brain } from "../brain";
import { Book } from "./book";
import { Chapter } from "./chapter";
import { ChapterChoice } from "./chapter-choice";
import { ChapterDecks } from "./chapter-decks";
import { Goal } from "./goal";

export class Player {
    public get book(): Book { return this.brain.book; }
    public readonly brain: Brain;
    public readonly chapters: ChapterDecks = new ChapterDecks();
    public readonly goals: Goal[] = [];
    public readonly id: Object = new Object();
    public get name(): string { return this.book.authorName; }

    public lastChapter: Chapter = undefined!;

    public constructor(brain: Brain) {
        if (!brain) throw new Error("A player must have a brain.");
        this.brain = brain;
        brain.initPlayer(this);
    }

    public hasCompletedAllGoals(character: Player): boolean {
        return this.goals.every(goal => goal.isCompleted(character.chapters));
    }

    public receiveChapter(chapterSelection: ChapterChoice): void {
        const chapter = chapterSelection.chapter;
        const chapterDeck = this.chapters[chapterSelection.asSuit];
        if (chapter.wild && chapterDeck.some(existing => existing.wild)) throw new Error('Cannot play two wild cards to the same suit.');
        this.lastChapter = chapter;
        chapterDeck.push(chapter);
    }
}
