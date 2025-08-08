import { Book, BookChapter, Chapter, Completion } from "../model";

export interface Author {
    /** The book being written in by this author. */
    readonly book: Book;

    /**
     * Writes a chapter of the story into the book.
     * @param chapter The chapter to write.
     * @param completion How complete the story is on a scale of 0.0 to 1.0. See `Completion`.
     * @returns The written chapter.
     */
    writeChapter(chapter: Chapter, completion: Completion): Promise<BookChapter>;
}
