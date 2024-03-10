import { PublicKnowledge, ChapterChoice, Player } from "../../model";
import { Instinct } from "./instinct";

/** Choose one option at random. */
export class Random extends Instinct {
    protected readonly name = "Random";

    public constructor() {
        super(undefined!);
    }

    public initPlayer(player: Player): void {
        this.player = player;
    }

    public chooseFrom(choices: ChapterChoice[], publicKnowledge: PublicKnowledge): ChapterChoice {
        choices = [choices[Math.floor(Math.random() * choices.length)]];
        return this.askSubInstinct(choices, publicKnowledge);
    }
}
