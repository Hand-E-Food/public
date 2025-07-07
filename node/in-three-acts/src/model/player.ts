import { Brain } from "../brain";
import { Book } from "./book";
import { ChapterDecks } from "./chapter-decks";
import { MaxChapters, MaxGoals } from "./constants";
import { Goal } from "./goal";

export class Player {
    public get book(): Book { return this.brain.book; }
    public readonly brain: Brain;
    public readonly chapters: ChapterDecks = new ChapterDecks();
    public readonly goals: Goal[] = [];
    public readonly id: Object = new Object();
    public get name(): string { return this.book.authorName; }

    public constructor(brain: Brain) {
        if (!brain) throw new Error("A player must have a brain.");
        this.brain = brain;
        brain.initPlayer(this);
    }

    public get hasCompletedCurrentGoals(): boolean {
        return this.goals.every(goal => goal.isCompleted(this.chapters));
    }

    public get hasFailed(): boolean {
        return Object.values(this.chapters).some(suit => suit.length > MaxChapters);
    }

    public get hasAllGoals(): boolean {
        return this.goals.length >= MaxGoals;
    }
}
