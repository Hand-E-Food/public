import { Chapter } from "./chapter";

export class Book {
    public readonly authorName: string;
    public readonly chapters: BookChapter[] = [];
    public title?: string;

    public constructor(authorName: string) {
        if (!authorName) throw new Error("A book must have an author name.");
        this.authorName = authorName;
    }

    public writeChapter(chapter: Chapter, text: string): BookChapter {
        const number = this.chapters.length + 1;
        const bookChapter = { chapter, number, text };
        this.chapters.push(bookChapter);
        return bookChapter;
    }
}

export interface BookChapter {
    readonly chapter: Chapter;
    readonly number: number;
    readonly text: string;
}
