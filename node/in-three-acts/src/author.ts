import { Message, Ollama } from "ollama";
import { AuthorStyles, Book, BookChapter, Chapter, Genres, StoryPhase } from "./model";
import { AuthorPhaseInstructions } from "./author-phase-instructions";

export interface IAuthor {
    /** The book being written in by this author. */
    readonly book: Book;

    /**
     * Writes the next chapter of the story into the book.
     * @param chapter The chapter to write.
     * @param phase The current phase of the story.
     * @returns The written chapter.
     */
    writeChapter(chapter: Chapter, phase: StoryPhase): Promise<BookChapter>;
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

export class Author implements IAuthor {
    public readonly book: Book;
    private readonly instructions: AuthorPhaseInstructions;
    private maximumWordCount = 50;
    private readonly messages: Message[] = [];
    private readonly ollama: Ollama;

    /** The name of the LLM writing the story. */
    public readonly model: string = 'qwen2:7b';

    /**
     * Creates a new author.
     * @param ollama The instance that communicates with Ollama.
     * @param props The initialisation properties.
     */
    public constructor(ollama: Ollama, props: AuthorProps) {
        this.ollama = ollama;
        this.book = props.book;
        const genre = props.genre || Genres.getRandom();
        const style = props.style || AuthorStyles.getRandom();
        this.messages.push(
        {
            role: 'system',
            content: [
                `You're a ${style} author.`,
                `You are writing a simple ${genre} story about the main character '${props.characterName}'.`,
                'You may choose supporting characters of the story.',
                'Each chapter should grow the story and develop the characters.',
                `Each chapter must fit in one short paragraph which is no longer than ${this.maximumWordCount} words. Do not write multiple paragraphs.`,
                'Write a narative. Do not explain or sumarise the chapters.',
                'Only include the content text in your response and do not include chapter name or any additional text.',
            ].join('\n'),
        });
        this.instructions = new AuthorPhaseInstructions(props.characterName);
    }

    public async writeChapter(chapter: Chapter, phase: StoryPhase): Promise<BookChapter> {
        const instruction = this.instructions.getInstruction(phase);
        this.messages.push({
            role: 'user',
            content: `Write a paragraph on the topic "${chapter.name}" that ${instruction}.`,
        });
        let text: string;
        while(true) {
            const response = await this.ollama.chat({
                messages: this.messages,
                model: this.model,
            });
            this.messages.push(response.message);
            text = this.normaliseChatResponse(response.message.content);

            const wordCount = text.split(' ').filter(word => word.match(/[a-z]/)).length;
            if (wordCount <= this.maximumWordCount) break;

            this.messages.push({
                role: 'user',
                content: 'That paragraph is too long. Please rewrite it more concisely.',
            });
        }
        const bookChapter = this.book.writeChapter(chapter, text);
        this.maximumWordCount += 3;
        if (phase == StoryPhase.Conclusion) await this.writeTitle();
        
        return bookChapter;
    }

    private async writeTitle(): Promise<void> {
        this.messages.push({
            role: 'user',
            content: 'Choose a title for the story you have written. Your response must only include the title and no other text.',
        });
        const response = await this.ollama.chat({
            messages: this.messages,
            model: this.model,
        });
        this.messages.push(response.message);
        let text = this.normaliseChatResponse(response.message.content);
        this.book.title = text;
    }

    private normaliseChatResponse(text: string): string {
        // Remove markdown quote block.
        if (text.startsWith('> ')) text = text.substring(2);
        // Add space around em-dashes.
        text = text.replace(/—/g, ' — ');
        // Remove newlines.
        text = text.replace(/\s+/g, ' ')

        return text;
    }
}
