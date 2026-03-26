import type { Card } from "./cards/index.js";
import { FedFamilyStack, NegativeStack, PositiveStack, type CardContainer } from "./containers/index.js";

/** A singleton game environment. */
export class Game {
    private readonly cards: Card[] = [];
    public readonly containers = {
        negativeStack: new NegativeStack(),
        positiveStack: new PositiveStack(),
        fedFamilyStack: new FedFamilyStack(),
    }

    /** This game's HTML element. */
    public readonly htmlElement: HTMLDivElement;

    public constructor() {
        this.htmlElement = document.createElement('div');
        this.htmlElement.classList.add('game');
    }

    /**
     * Adds cards to this game.
     * @param container The container to add these cards to.
     * @param cards The cards to add.
     */
    public addCards(container: CardContainer, ...cards: Card[]): void {
        for (const card of cards) {
            if (this.cards.includes(card)) throw new Error('Cannot add the same card twice.');
            this.htmlElement.appendChild(card.htmlElement);
        }
        this.cards.push(...cards);
        container.addCards(...cards);
    }

    /** Removes a card from this game. */
    public removeCard(card: Card): void {
        const i = this.cards.indexOf(card);
        if (i === -1) throw new Error('Cannot remove a card that wasn\'t added.');
        card.htmlElement.remove();
        this.cards.splice(i, 1);
    }
}

export const game = new Game();
