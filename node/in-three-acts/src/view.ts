import { Chapter, Game, Player, PublicKnowledge, Suit } from "./model";

export interface View {
    setPlayer(player: Player): void;
    showGame(publicKnowledge: PublicKnowledge): void;
    chooseChapter(chapters: Chapter[]): Promise<Chapter>;
    chooseSuit(suits: Suit[]): Promise<Suit>;
    showWinner(player: Player): void;
    dispose(): void;
}
