import { Book, BookChapter, Chapter, StoryPhase } from "../model";
import { Author } from "./author";
import { AuthorPhaseInstructions } from "./author-phase-instructions";

export interface DummyAuthorProps {
    readonly book: Book;
    readonly characterName: string;
}

export class DummyAuthor implements Author {
    public readonly book: Book;
    private readonly instructions: AuthorPhaseInstructions;

    /**
     * Creates a new dummy author.
     * @param props The initialisation properties.
     */
    public constructor(props: DummyAuthorProps) {
        this.book = props.book;
        this.book.title = 'Story Title';
        this.instructions = new AuthorPhaseInstructions(props.characterName);
    }

    public writeChapter(chapter: Chapter, phase: StoryPhase): Promise<BookChapter> {
        const instruction = this.instructions.getInstruction(phase);
        const bookChapter = this.book.writeChapter(chapter, `This chapter ${instruction}`);
        return Promise.resolve(bookChapter);
    }
}
