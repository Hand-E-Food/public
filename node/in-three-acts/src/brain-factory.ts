import { Author } from "./author";
import { Brain, Instinct, InstinctBrain } from "./brain";
import { AvoidFailure, ChooseRandomly, ConsiderWild, EncourageFailure, FocusOnGoal, Prefer } from "./brain/instinct";
import { MaxChapters } from "./model";
import { View } from "./view";

export class BrainFactory {
    private readonly view: View;

    public constructor(view: View) {
        this.view = view;
    }

    public createCpuBrain(author: Author, level: number): Brain {
        if (!author) throw new Error("A CPU brain must have an author.");
        // The instinct at the bottom of this list is questioned first.
        // The list is intentionally not in level order. Higher levels insert instincts where they are most appropriate.
        let instinct: Instinct = new ChooseRandomly();
        if (level >= 1) instinct = new ConsiderWild(instinct);
        if (level >= 2) instinct = new AvoidFailure(instinct, MaxChapters);
        if (level >= 4) instinct = new FocusOnGoal(instinct, Prefer.Furthest);
        if (level >= 5) instinct = new EncourageFailure(instinct);
        if (level >= 3) instinct = new FocusOnGoal(instinct);
        if (level >= 6) instinct = new AvoidFailure(instinct, 0);
        return new InstinctBrain(this.view, author, instinct);
    }
}
