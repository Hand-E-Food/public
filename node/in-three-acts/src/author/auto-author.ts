import { debugLog } from "../logger";
import { AuthorStyles, Book, BookChapter, Chapter, Genres, StoryPhase } from "../model";
import { Author } from "./author";
import { AuthorPhaseInstructions } from "./author-phase-instructions";
import { LlmClient, Message } from "../llm/llm-client";

const MaxWordCount = 50;

const Token = {
    ContentNotice: 'CONTENT NOTICE:',
    CreativityWarmup: 'CREATIVITY WARMUP:',
    NextChapter: 'NEXT CHAPTER:',
}

export interface AutoAuthorProps {
    /** An initialised book that this author will write in. */
    readonly book: Book;

    /** The name of the main character to write about. */
    readonly characterName: string;

    /**
     * The genre to write.
     * e.g. "You are writing a simple `genre` story"
     */
    readonly genre?: string;

    /** The LLM client to use for writing. */
    readonly llmClient: LlmClient;

    /**
     * The author's writing style.
     * e.g. "You are a `style` author"
     */
    readonly style?: string;
}

export class AutoAuthor implements Author {
    private readonly instructions: AuthorPhaseInstructions;
    private readonly llmClient: LlmClient;
    private readonly systemPrompt: Message;

    /** The book this author is writing in. */
    public readonly book: Book;

    private get allBookText(): string[] {
        return this.book.chapters.map(chapter => chapter.text);
    }

    /**
     * Creates a new author.
     * @param ollama The instance that communicates with Ollama.
     * @param props The initialisation properties.
     */
    public constructor(props: AutoAuthorProps) {
        this.book = props.book;
        this.llmClient = props.llmClient;
        const authorName = props.book.authorName;
        const characterName = props.characterName;
        const genre = props.genre || Genres.getRandom();
        const style = props.style || AuthorStyles.getRandom();
        this.systemPrompt = {
            role: 'system',
            content: [
                `You are a ${style} author named ${authorName} who uses the following approach to write simple ${genre} stories.`,
                `* You are writing your latest story about the main character "${characterName}".`,
                `* Begin each writing session by typing "${Token.ContentNotice}" followed by a heads-up about any potentially intense story elements, like "mild fantasy action" or "some emotional moments".`,
                `* After the content notice, write "${Token.CreativityWarmup}" and then a silly sentence with made-up words. This gets your imagination revved up to write.`,
                `* After the warmup, write "${Token.NextChapter}" and let the tale unfold.`,
                `* Stick to this format of content notice, creativity warmup, then diving into the story each time, even when revising earlier parts.`,
                `* As ${authorName}, you know characters are driven by a web of emotions, hopes, and dreams. Weaving these together creates relatable, compelling stories.`,
                `* The chapters must be very brief, just 2 sentences long, to keep the story moving quickly and maintain reader interest.`,
                `* Do not repeat the chapter title in the chapter text. Do not write chapter numbers. Do not quote the text. Do not write "End of chapter" or similar phrases.`,
                `* Use simple, clear language that is easy to read and understand. Avoid complex words and long sentences.`,
                `* Write in a way that is fun and engaging for young readers, especially tweens and teens.`,
                `* Keep each chapter focussed on a single idea or event. This helps maintain clarity and keeps the story moving.`,
                `* Stick to ANSII characters only. Do not use any special characters or smart quotes.`,
                `* Most importantly, have a blast bringing your stories to life! Your enthusiasm and joy will shine through and make your writing truly magical.`,
                ``,
                `For example, your response shuld be:`,
                `${Token.ContentNotice} This chapter contains some mild fantasy action.`,
                `${Token.CreativityWarmup} Flibberty flobberty floo!`,
                `${Token.NextChapter} ${characterName} stepped into the enchanted forest, ready for adventure.`,
            ].join('\n'),
        };
        this.instructions = new AuthorPhaseInstructions(props.characterName);
    }

    /**
     * Writes a chapter of the story into the book.
     * @param chapter The chapter to write.
     * @param phase The current phase of the story.
     * @returns The written chapter.
     */
    public async writeChapter(chapter: Chapter, phase: StoryPhase): Promise<BookChapter> {
        const userPrompt: string[] = [];
        const instruction = this.instructions.getInstruction(phase);
        if (this.book.chapters.length > 0) {
            userPrompt.push(
                `Here is the story so far:`,
                ``,
                ...this.allBookText,
                ``,
            );
        }
        userPrompt.push(
            `Write a single chapter titled "${chapter.name}" that ${instruction}`,
        );

        const messages: Message[] = [
            this.systemPrompt,
            { role: 'user', content: userPrompt.join('\n') },
        ];
        const response = await this.llmClient.chat(messages);
        messages.push(response);
        const text = this.parseChatResponse(response.content);

        const bookChapter = this.book.writeChapter(chapter, text);
        if (phase == StoryPhase.Conclusion) await this.writeTitle();
        
        return bookChapter;
    }

    private async writeTitle(): Promise<void> {
        const userPrompt: string[] = [
            `Here is the story so far:`,
            ``,
            ...this.allBookText,
            ``,
            `Choose a title for the above story. Your response must only include the title and no other text.`,
        ];
        const messages: Message[] = [
            this.systemPrompt,
            { role: 'user', content: userPrompt.join('\n') },
        ]
        let attempts = 3;
        let text: string | undefined = undefined;
        while (!text) {
            try {
                attempts--;
                const response = await this.llmClient.chat(messages);
                messages.push(response);
                let text = this.parseChatResponse(response.content);
                this.book.title = text;
            } catch (error) {
                if (attempts <= 0)
                    throw error;
                if (error instanceof PromptRetryError)
                    messages.push({ role: 'user', content: error.message });
                else
                    throw error;
            }
        }
    }

    private parseChatResponse(text: string): string {
        debugLog(text);
        // Find the start of the story.
        const index = text.indexOf(Token.NextChapter);
        if (index < 0) throw new PromptRetryError(`Your response did not include the start token "${Token.NextChapter}". I do not know where your chapter text starts.`);
        text = text.substring(index + Token.NextChapter.length);
        // Remove HTML tags.
        text = text.replace(/<[^>]*>/g, '');
        // Add space around em-dashes.
        text = text.replace(/—/g, ' — ');
        // Remove newlines and unnecessary whitespace.
        text = text.replace(/\s+/g, ' ')
        // Trim leading and trailing whitespace.
        text = text.trim();
        // Count words.
        if (text.split(' ').filter(word => word.length > 1).length > MaxWordCount)
            throw new PromptRetryError(`Your chapter text was too long. Edit your chapter text to be more concise. It must be less than 50 words.`);

        return text;
    }
}

class PromptRetryError extends Error {
    public constructor(message: string) {
        super(message);
        this.name = 'PromptRetryError';
    }
}