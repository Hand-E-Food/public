import { ChapterChoice, Player, PublicKnowledge } from "../../model";
import { Instinct } from "./instinct";

/** Choose one option at random. */
export class ChooseRandomly extends Instinct {
    protected readonly name = "Choose randomly";

    public constructor() {
        super(undefined!);
    }

    public initPlayer(player: Player): void {
        this.player = player;
    }

    public chooseFrom(choices: ChapterChoice[], publicKnowledge: PublicKnowledge): ChapterChoice {
        const index = Math.floor(Math.random() * choices.length);
        choices = [choices[index]];
        return this.askSubInstinct(choices, publicKnowledge);
    }
}
