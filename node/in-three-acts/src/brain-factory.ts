import { Brain, Instinct } from "./brain";
import { Controlling, Wild, Offensive, Prefer, Random, Survival } from "./brain/instinct";
import { InstinctBrain } from "./brain/instinct-brain";
import { MaxChapters } from "./model";

export class BrainFactory {
    public createCpuBrain(): Brain {
        // The brain at the bottom of this list is questioned first.
        let instinct: Instinct;
        instinct = new Random();
        instinct = new Wild(instinct);
        instinct = new Survival(instinct, MaxChapters);
        instinct = new Offensive(instinct, Prefer.Furthest);
        instinct = new Controlling(instinct);
        instinct = new Offensive(instinct);
        instinct = new Survival(instinct, 0);
        return new InstinctBrain(instinct);
    }
}
