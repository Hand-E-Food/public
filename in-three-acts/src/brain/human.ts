import { Chapter, PublicKnowledge, ChapterChoice } from "../model";
import { View } from "../view";
import { Brain } from "./brain";

export class Human extends Brain {
    private readonly view: View;

    public constructor(view: View) {
        if (!view) throw new Error("A human brain must have a way to view the game.");
        super();
        this.view = view;
    }

    public async chooseChapter(chapters: Chapter[], wildSuits: string[], publicKnowledge: PublicKnowledge): Promise<ChapterChoice> {
        this.view.showGame(publicKnowledge);
        const chapter = await this.view.chooseChapter(chapters);
        const asSuit = chapter.wild ? await this.view.chooseSuit(wildSuits) : undefined;
        return new ChapterChoice(chapter, asSuit);
    }
}
