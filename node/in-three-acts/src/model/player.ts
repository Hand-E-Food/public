import { Chapter } from "./chapter";
import { ChapterDecks } from "./chapter-decks";
import { ChapterChoice } from "./chapter-choice";
import { Goal } from "./goal";
import { Brain } from "../brain/brain";

export class Player {
    public readonly brain: Brain;
    public readonly chapters: ChapterDecks = new ChapterDecks();
    public readonly goals: Goal[] = [];
    public readonly id: Object = new Object();
    public readonly name: string;
    
    public lastChapter: Chapter = undefined!;

    public constructor(name: string, brain: Brain) {
        if (!name) throw new Error("A player must have a name.");
        if (!brain) throw new Error("A player must have a brain.");
        this.brain = brain;
        this.name = name;
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
