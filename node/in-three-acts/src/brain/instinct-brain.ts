import { Chapter, ChapterChoice, Player, PublicKnowledge, Suit } from "../model";
import { Brain } from "./brain";
import { Instinct } from "./instinct/instinct";

export class InstinctBrain extends Brain {
    private readonly instinct: Instinct;

    public constructor(instinct: Instinct) {
        super();
        this.instinct = instinct;
    }

    public initPlayer(player: Player): void {
        super.initPlayer(player);
        this.instinct.initPlayer(player);
    }

    public chooseChapter(chapters: Chapter[], wildSuits: Suit[], publicKnowledge: PublicKnowledge): Promise<ChapterChoice> {
        const choices: ChapterChoice[] = [];
        chapters.forEach(chapter => {
            if (chapter.wild) {
                wildSuits.forEach(asSuit => {
                    choices.push(new ChapterChoice(chapter, asSuit));
                });
            } else {
                choices.push(new ChapterChoice(chapter));
            }
        });
        Instinct.log('Start', choices);
        const choice = this.instinct.chooseFrom(choices, publicKnowledge);
        return Promise.resolve(choice);
    }
}
