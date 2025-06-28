import { ChapterChoice, Player, PublicKnowledge } from "../../model";

export abstract class Instinct {
    protected abstract readonly name: string;
    protected player: Player = undefined!;
    private readonly subInstinct: Instinct;

    public constructor(subInstinct: Instinct) {
        this.subInstinct = subInstinct;
    }

    public initPlayer(player: Player): void {
        this.player = player;
        if (this.subInstinct) this.subInstinct.initPlayer(player);
    }

    public abstract chooseFrom(choices: ChapterChoice[], publicKnowledge: PublicKnowledge): ChapterChoice;

    protected askSubInstinct(choices: ChapterChoice[], publicKnowledge: PublicKnowledge): ChapterChoice {
        Instinct.log(this.name, choices);
        if (choices.length === 1) return choices[0];
        return this.subInstinct.chooseFrom(choices, publicKnowledge);
    }

    public static log(name: string, choices: ChapterChoice[]): void {
        const strings = choices.map(choice => `[${choice.chapter.wild ? `${choice.chapter.suit}@${choice.asSuit}` : ` ${choice.chapter.suit} `} => ${choice.chapter.inspires[0]} ${choice.chapter.inspires[1]}]`);
        const message = `${name.padEnd(20)}: ` + strings.join(', ') + '\n';
        //process.stdout.write(message);
    }
}