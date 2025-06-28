import { Author } from "./author";
import { Brain, Instinct, InstinctBrain } from "./brain";
import { Controlling, Offensive, Prefer, Random, Survival, Wild } from "./brain/instinct";
import { MaxChapters } from "./model";
import { View } from "./view";

export class BrainFactory {
    private readonly view: View;

    public constructor(view: View) {
        this.view = view;
    }

    public createCpuBrain(author: Author, level: number): Brain {
        if (!author) throw new Error("A CPU brain must have an author.");
        // The brain at the bottom of this list is questioned first.
        let instinct: Instinct;
        instinct = new Random();
        if (level >= 1) instinct = new Wild(instinct);
        if (level >= 2) instinct = new Survival(instinct, MaxChapters);
        if (level >= 3) instinct = new Offensive(instinct, Prefer.Furthest);
        if (level >= 4) instinct = new Controlling(instinct);
        if (level >= 5) instinct = new Offensive(instinct);
        if (level >= 6) instinct = new Survival(instinct, 0);
        return new InstinctBrain(this.view, author, instinct);
    }
}
