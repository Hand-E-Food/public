import { ChapterChoice, MaxChapters, PublicKnowledge, Suit, Suits } from "../../model";
import { Instinct } from "./instinct";

/** Filter out options that would cause me be closer to losing. */
export class AvoidFailure extends Instinct {
    protected readonly buffer: number;
    protected readonly name: string;

    public constructor(subInstinct: Instinct, buffer: number) {
        super(subInstinct);
        this.buffer = buffer;
        this.name = `Avoid suits within ${buffer} chapter(s) of failure`;
    }
    
    public chooseFrom(choices: ChapterChoice[], publicKnowledge: PublicKnowledge): ChapterChoice {
        const writtenChapters = publicKnowledge.author.chapters;
        const minCount = MaxChapters - this.buffer - 1;
        let bestSuits: Suit[] = [];
        let bestCount = MaxChapters;
        for (const suit of Suits) {
            const count = Math.max(minCount, writtenChapters[suit].length);
            if (count === bestCount) {
                bestSuits.push(suit);
            } else if (count < bestCount) {
                bestSuits = [suit];
                bestCount = count;
            }
        };

        const safeChoices = choices.filter(choice => bestSuits.includes(choice.asSuit));
        if (safeChoices.length > 0) choices = safeChoices;
        return this.askSubInstinct(choices, publicKnowledge);
    }
}
