import { StoryPhase } from "../model";

export class AuthorPhaseInstructions {
    private readonly phaseInstructions: Record<StoryPhase, string[]>;
    
    private phaseCount: number = 0;
    private previousPhase: StoryPhase = undefined!;

    public constructor(characterName: string) {
        this.phaseInstructions = {
            [StoryPhase.Exposition]: [
                `introduces the main character, ${characterName}, and the setting.`,
                'introduces the antagonist, who may be either an individual or an intangible concept.',
                'starts rising action.',
            ],
            [StoryPhase.PlotTwist]: [
                'introduces a dramatic plot twist.',
            ],
            [StoryPhase.Resoultion]: [
                'starts to build to a climax.',
            ],
            [StoryPhase.Conclusion]: [
                'is the conclusion to the story. Ensure that the main character defeats the antagonist.',
            ],
            [StoryPhase.Failure]: [
                'is confusing, rambling, and does not fit the story.',
            ],
        };
    }

    /**
     * Gets an appropriate instruction for the current phase of the story that fits the prompt
     * "Write a paragraph that `instruction`".
     * @param phase The current story phase.
     * @returns The instruction to pass in the prompt.
     */
    public getInstruction(phase: StoryPhase): string {
        if (phase != this.previousPhase) {
            this.previousPhase = phase;
            this.phaseCount = 0;
        } else {
            this.phaseCount++;
        }

        return this.phaseInstructions[phase][this.phaseCount] ?? 'continues the story.';
    }
}
