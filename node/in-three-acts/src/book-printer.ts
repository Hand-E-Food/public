import { promises as fs } from 'fs';
import { Book } from "./model";

export class BookPrinter {
    public async printBook(book: Book, filePath: string): Promise<void> {
        const data = [
            `# ${book.title}`,
            `by ${book.authorName}`,
            ...book.chapters.flatMap(chapter => [
                '',
                '',
                `## ${chapter.number}. ${chapter.chapter.name}`,
                '',
                chapter.text,
            ]),
            '',
            '',
            `# ${book.ending}`,
        ].join('\n');
        await fs.writeFile(filePath, data, 'utf8');
    }
}
