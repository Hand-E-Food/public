import { ChapterChoice, PublicKnowledge, Suit } from "../../model";
import { Instinct } from "./instinct";
import { Prefer } from "./prefer";

interface SuitInfo {
    suit: Suit;
    target: number;
    have: number;
    need: number;
    score: number;
}

/** Filter in options that contribute to my goals. */
export class Offensive extends Instinct {
    protected readonly name: string;
    protected readonly prefer: Prefer;

    public constructor(subInstinct: Instinct, prefer: Prefer = Prefer.None) {
        super(subInstinct);
        this.prefer = prefer;
        this.name = `Offensive(${prefer})`;
    }

    public chooseFrom(choices: ChapterChoice[], publicKnowledge: PublicKnowledge): ChapterChoice {
        let infos: SuitInfo[] = [];
        const characterChapters = publicKnowledge.character.chapters;
        this.player.goals.forEach(goal => {
            Object.keys(goal.suitCounts).forEach(suit => {
                const target = goal.suitCounts[suit];
                const have = characterChapters[suit].length;
                const need = target - have;
                if (need > 0) {
                    const score = (() => {
                        switch (this.prefer) {
                            case Prefer.Closest : return need;
                            case Prefer.Furthest: return -need;
                            case Prefer.Highest : return -target;
                            case Prefer.Lowest  : return target;
                            default             : return 0;
                        }
                    })();
                    infos.push({ suit, target, have, need, score });
                }
            });
        });

        const availableSuits = choices.map(choice => choice.asSuit);
        infos = infos.filter(info => availableSuits.includes(info.suit));
        if (infos.length > 0) {
            infos.sort((a, b) => a.score - b.score);
            const bestScore = infos[0].score;
            infos = infos.filter(info => info.score === bestScore);
            const desiredSuits = infos.map(info => info.suit);
            const desiredChoices = choices.filter(choice => desiredSuits.includes(choice.asSuit));
            if (desiredChoices.length >= 1) choices = desiredChoices;
        }
        return this.askSubInstinct(choices, publicKnowledge);
    }
}
