import { ChapterChoice, MaxChapters, PublicKnowledge, SuitCount, Suits } from "../../model";
import { Instinct } from "./instinct";

/** Filters in options that we know the opponent doesn't want. */
export class EncourageFailure extends Instinct {
    protected readonly name = "Focus on character's completed goals";
    
    public chooseFrom(choices: ChapterChoice[], publicKnowledge: PublicKnowledge): ChapterChoice {
        const desiredSuits = [
            ...publicKnowledge.character.goals.map(goal => goal.getCompletedSuits(publicKnowledge.author.chapters)).flat(),
            ...Suits.filter(suit => publicKnowledge.author.chapters[suit].length === MaxChapters),
        ];

        if (desiredSuits.length > 0) {
            const suitDanger: SuitCount = {};
            for (const suit of Suits)
                suitDanger[suit] = desiredSuits.includes(suit) ? publicKnowledge.author.chapters[suit].length : 0;
            function dangerousness(choice: ChapterChoice): number {
                return choice.chapter.inspires.reduce((total, suit) => total + suitDanger[suit], 0);
            }
            choices = choices.sort((a, b) => dangerousness(b) - dangerousness(a));
            const maxDangerousness = dangerousness(choices[0]);
            choices = choices.filter(choice => dangerousness(choice) == maxDangerousness);
        }

        return this.askSubInstinct(choices, publicKnowledge);
    }
}
