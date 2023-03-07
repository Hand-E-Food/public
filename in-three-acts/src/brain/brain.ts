import { Chapter, ChapterChoice, Player, PublicKnowledge, Suit } from "../model";

export abstract class Brain {
    protected player: Player = undefined!;

    public initPlayer(player: Player): void {
        this.player = player;
    }

    public abstract chooseChapter(chapters: Chapter[], wildSuits: Suit[], publicKnowledge: PublicKnowledge): Promise<ChapterChoice>;
}
