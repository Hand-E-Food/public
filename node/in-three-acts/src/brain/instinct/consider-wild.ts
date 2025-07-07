import { ChapterChoice, PublicKnowledge } from "../../model";
import { Instinct } from "./instinct";

/**
 * Filter in wild cards if I will draw more goals. Filter out wild cards if I have all of my goals.
 *
 * Using a wild card indicates what suit I need to complete my goals.
 */
export class ConsiderWild extends Instinct {
    protected name = "Consider deus ex machina";

    public chooseFrom(choices: ChapterChoice[], publicKnowledge: PublicKnowledge): ChapterChoice {
        const useWild: boolean = !this.player.hasAllGoals;
        const desiredChoices = choices.filter(choice => choice.chapter.wild === useWild);
        if (desiredChoices.length > 0) choices = desiredChoices;
        return this.askSubInstinct(choices, publicKnowledge);
    }
}
