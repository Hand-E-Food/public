import { BookChapter, Chapter, Player, PublicKnowledge, Suit } from "./model";

export interface View {
    setPlayer(player: Player): void;
    showGame(publicKnowledge: PublicKnowledge): void;
    chooseChapter(chapters: Chapter[]): Promise<Chapter>;
    chooseSuit(suits: Suit[]): Promise<Suit>;
    showBookChapter(chapter: BookChapter): void;
    showWinner(player: Player): void;
    dispose(): void;
}
