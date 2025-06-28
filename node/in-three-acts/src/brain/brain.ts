import { Book, BookChapter, Chapter, ChapterChoice, Player, PublicKnowledge, StoryPhase, Suit } from "../model";

export abstract class Brain {
    protected player: Player = undefined!;

    public abstract book: Book;

    public initPlayer(player: Player): void {
        this.player = player;
    }

    /**
     * Asks the brain to choose the next chapter to write.
     * @param chapters The chapters to choose from.
     * @param wildSuits The suits that can be used as wild suits.
     * @param publicKnowledge The public knowledge of the game.
     * @returns A chosen chapter.
     */
    public abstract chooseChapter(chapters: Chapter[], wildSuits: Suit[], publicKnowledge: PublicKnowledge): Promise<ChapterChoice>;

    /**
     * Writes a chapter to the book.
     * @param chapter The chapter to write.
     * @param phase The phase of the story in which the chapter is being written.
     * @returns The book chapter that was written.
     */
    public abstract writeChapter(chapter: Chapter, phase: StoryPhase): Promise<BookChapter>;
}
