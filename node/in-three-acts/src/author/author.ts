import { Book, BookChapter, Chapter, StoryPhase } from "../model";

export interface Author {
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
