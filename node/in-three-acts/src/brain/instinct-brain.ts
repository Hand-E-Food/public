import { Author } from "../author";
import { Book, BookChapter, Chapter, ChapterChoice, Player, PublicKnowledge, StoryPhase, Suit } from "../model";
import { View } from "../view";
import { Brain } from "./brain";
import { Instinct } from "./instinct";

export class InstinctBrain extends Brain {
    private readonly author: Author;
    private readonly instinct: Instinct;
    private readonly view: View;

    public constructor(view: View, author: Author, instinct: Instinct) {
        if (!author) throw new Error("An instinct brain must have an author.");
        if (!instinct) throw new Error("An instinct brain must have an instinct.");
        if (!view) throw new Error("An instinct brain must have a view to interact with.");
        super();
        this.author = author;
        this.instinct = instinct;
        this.view = view;
    }

    public get book(): Book {
        return this.author.book;
    }

    public initPlayer(player: Player): void {
        super.initPlayer(player);
        this.instinct.initPlayer(player);

    }

    public chooseChapter(chapters: Chapter[], wildSuits: Suit[], publicKnowledge: PublicKnowledge): Promise<ChapterChoice> {
        const choices: ChapterChoice[] = [];
        for (const chapter of chapters) {
            if (chapter.wild) {
                for (const asSuit of wildSuits)
                    choices.push(new ChapterChoice(chapter, asSuit));
            } else {
                choices.push(new ChapterChoice(chapter));
            }
        };
        Instinct.log('Start', choices);
        const choice = this.instinct.chooseFrom(choices, publicKnowledge);
        
        return Promise.resolve(choice);
    }

    public async writeChapter(chapter: Chapter, phase: StoryPhase): Promise<BookChapter> {
        const bookChapter = await this.author.writeChapter(chapter, phase);
        this.view.showBookChapter(bookChapter);
        return bookChapter;
    }
}
