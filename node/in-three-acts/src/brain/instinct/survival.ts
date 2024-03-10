import { MaxChapters, PublicKnowledge, ChapterChoice, SuitCount, Suits, Suit } from "../../model";
import { Instinct } from "./instinct";

/** Filter out options that would cause me be closer to losing. */
export class Survival extends Instinct {
    protected readonly buffer: number;
    protected readonly name: string;

    public constructor(subInstinct: Instinct, buffer: number) {
        super(subInstinct);
        this.buffer = buffer;
        this.name = `Survival(${buffer})`;
    }
    
    public chooseFrom(choices: ChapterChoice[], publicKnowledge: PublicKnowledge): ChapterChoice {
        const characterChapters = publicKnowledge.character.chapters;
        const minCount = MaxChapters - this.buffer - 1;
        let bestSuits: Suit[] = [];
        let bestCount = MaxChapters;
        Suits.forEach(suit => {
            const count = Math.max(minCount, characterChapters[suit].length);
            if (count === bestCount) {
                bestSuits.push(suit);
            } else if (count < bestCount) {
                bestSuits = [suit];
                bestCount = count;
            }
        });

        const safeChoices = choices.filter(choice => bestSuits.includes(choice.asSuit));
        if (safeChoices.length > 0) choices = safeChoices;
        return this.askSubInstinct(choices, publicKnowledge);
    }
}
