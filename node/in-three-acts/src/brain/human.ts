import { Chapter, PublicKnowledge, ChapterChoice, Book, BookChapter } from "../model";
import { View } from "../view";
import { Brain } from "./brain";

export class Human extends Brain {
    public book: Book;
    private readonly view: View;

    public constructor(book: Book, view: View) {
        if (!book) throw new Error("A human brain must have a book.");
        if (!view) throw new Error("A human brain must have a way to view the game.");
        super();
        this.book = book;
        this.view = view;
    }

    public async chooseChapter(chapters: Chapter[], wildSuits: string[], publicKnowledge: PublicKnowledge): Promise<ChapterChoice> {
        this.view.showGame(publicKnowledge);
        const chapter = await this.view.chooseChapter(chapters);
        const asSuit = chapter.wild ? await this.view.chooseSuit(wildSuits) : undefined;
        return new ChapterChoice(chapter, asSuit);
    }

    public writeChapter(chapter: Chapter): Promise<BookChapter> {
        const text = '';
        const bookChapter = this.book.writeChapter(chapter, text);
        return Promise.resolve(bookChapter);
    }
}
