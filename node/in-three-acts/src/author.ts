import { Message, Ollama } from "ollama";
import { AuthorStyles, Book, BookChapter, Chapter, Genres, StoryPhase } from "./model";

/**
 * The prompt to use for each phase of the story.
 * e.g. "Write a chapter on the topic "`chapter`" that `instruction`."
 */
const PhaseInstructions: Record<StoryPhase, string> = {
    [StoryPhase.Exposition]: 'contains exposition',
    [StoryPhase.RisingAction]: 'has rising action',
    [StoryPhase.PlotTwist]: 'introduces a dramatic plot twist',
    [StoryPhase.Resoultion]: 'builds to a climax',
    [StoryPhase.Conclusion]: 'is the conclusion to the story',
}

export interface AuthorProps {
    /** An initialised book that this author will write in. */
    readonly book: Book;

    /** The name of the main character to write about. */
    readonly characterName: string;

    /**
     * The genre to write.
     * e.g. "You are writing a simple `genre` story"
     */
    readonly genre?: string;

    /**
     * The author's writing style.
     * e.g. "You're a `style` author"
     */
    readonly style?: string;
}

export class Author {
    private readonly messages: Message[] = [];
    private readonly ollama: Ollama;

    /** The book being writte in by this author. */
    public readonly book: Book;

    /** The name of the LLM writing the story. */
    public readonly model: string = 'qwen2:7b';

    public constructor(ollama: Ollama, props: AuthorProps) {
        this.ollama = ollama;
        this.book = props.book;
        const genre = props.genre || Genres.random();
        const style = props.style || AuthorStyles.random();
        this.messages.push(
        {
            role: 'system',
            content: [
                `You're a ${style} author.`,
                `You are writing a simple ${genre} story about the main character '${props.characterName}'.`,
                `You may choose supporting characters of the story.`,
                `Each chapter should grow the story and develop the characters.`,
                `The chapter must fit in one single paragraph which is no longer than 40 words. Do not write multiple paragraphs.`,
                `Write a narative. Do not explain or sumarise the chapters.`,
                `Only include the content text in your response and do not include chapter name or any additional text.`,
            ].join('\n'),
        });
    }

    /**
     * Writes the next chapter of the story into the book.
     * @param chapter The chapter to write.
     * @param phase The current phase of the story.
     * @returns The written chapter.
     */
    public async writeChapter(chapter: Chapter, phase: StoryPhase): Promise<BookChapter> {
        const instruction = PhaseInstructions[phase];
        this.messages.push({
            role: 'user',
            content: `Write a chapter on the topic "${chapter.name}" that ${instruction}.`,
        });
        const response = await this.ollama.chat({
            messages: this.messages,
            model: this.model,
        });
        this.messages.push(response.message);
        let text = response.message.content;
        if (text.startsWith('> ')) text = text.substring(2);
        text = text.replace(/—/g, ' — ');
        return this.book.writeChapter(chapter, text);
    }
}
