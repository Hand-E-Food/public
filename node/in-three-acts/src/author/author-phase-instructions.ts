import { Completion } from "../model";

interface Instruction {
    readonly completion: number;
    readonly instruction: string;
    readonly mandatory: boolean;
}

export class AuthorPhaseInstructions {
    private readonly defaultInstruction: string = 'continues the story';
    private readonly failureInstruction: string = 'rambles and does not fit the previous story';
    private readonly instructions: Instruction[];
    
    public constructor(characterName: string) {
        function instruction(mandatory: boolean, step: number, instruction: string): Instruction {
            return { completion: step / 15, mandatory, instruction };
        }

        this.instructions = [
            instruction( true,  0, `starts the story, introducing that main character ${characterName}`),
            instruction( true,  1, 'describes an inciting incident'),
            instruction(false,  2, 'causes second thoughts'),
            instruction( true,  3, 'is the climax of act one'),
            instruction(false,  4, 'suffers an obstacle'),
            instruction(false,  5, 'suffers another obstacle'),
            instruction( true,  6, 'is the midpoint of the story and ahs a big plot twist'),
            instruction(false,  7, 'suffers another obstacle'),
            instruction(false,  8, 'causes a disaster'),
            instruction(false,  9, 'causes a crisis'),
            instruction( true, 10, 'is the climax of act two'),
            instruction(false, 11, 'works towards solving the problem'),
            instruction(false, 12, 'begins the resolution to the problem'),
            instruction( true, 15, 'solves the problem and wraps up the story'),
        ];
    }

    /**
     * Gets an appropriate instruction for the current phase of the story that fits the prompt
     * "Write a paragraph that `instruction`".
     * @param phase The current story phase.
     * @returns The instruction to pass in the prompt.
     */
    public popInstruction(completion: Completion): string {
        if (completion === null) {
            return this.failureInstruction;
        }
        function score(instruction: Instruction): number {
            return (instruction.mandatory ? 0 : 2) + instruction.completion;
        }
        const availableInstructions = this.instructions
            .filter(i => i.completion <= completion)
            .sort((a, b) => score(a) - score(b));
        if (availableInstructions.length == 0)
            return this.defaultInstruction;

        const instruction = availableInstructions[0];
        this.instructions.splice(this.instructions.indexOf(instruction), 1);
        return instruction.instruction;
    }
}
