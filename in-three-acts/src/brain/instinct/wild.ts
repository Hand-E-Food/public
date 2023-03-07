import { ChapterChoice, MaxGoals, PublicKnowledge } from "../../model";
import { Instinct } from "./instinct";

/** Filter out wild cards if I will draw more goals. Filter in wild cards if I have all of my goals. */
export class Wild extends Instinct {
    protected name = "NotWild";

    public chooseFrom(choices: ChapterChoice[], publicKnowledge: PublicKnowledge): ChapterChoice {
        const useWild: boolean = this.player.goals.length < MaxGoals;
        const desiredChoices = choices.filter(choice => choice.chapter.wild === useWild);
        if (desiredChoices.length > 0) choices = desiredChoices;
        return this.askSubInstinct(choices, publicKnowledge);
    }
}
