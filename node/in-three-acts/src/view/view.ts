import { BookChapter, Chapter, Player, PublicKnowledge, Suit } from "../model";

export interface View {
    startGame(): void;
    setPlayer(player: Player): void;
    showGame(publicKnowledge: PublicKnowledge): void;
    chooseChapter(chapters: Chapter[]): Promise<Chapter>;
    chooseSuit(suits: Suit[]): Promise<Suit>;
    showBookChapter(chapter: BookChapter): void;
    showWinner(player: Player): void;
    waitForClose(): Promise<void>;
    dispose(): void;
}
