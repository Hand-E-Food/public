import type { Card } from "../cards/index.js";

/** Arranges cards on part of the game area. */
export abstract class CardContainer {
    /** The cards in this container. */
    public readonly cards: Card[] = [];

    /**
     * Move cards to this container.
     * @param cards The cards to add to this container.
     */
    public addCards(...cards: Card[]): void {
        for (const card of cards) {
            card.container?.removeCard(card);
            card.container = this;
        }
        this.cards.push(...cards);
        this.arrangeCards();
    }

    /**
     * Remove a card from this container.
     * @param card The card to remove.
     */
    public removeCard(card: Card): void {
        const i = this.cards.indexOf(card);
        if (i === -1) throw new Error('Cannot remove a card that is not in this container.');
        card.container = undefined;
        this.cards.splice(i, 1);
        this.arrangeCards();
    }

    /** Arranges this container's cards. */
    protected abstract arrangeCards(): void;
}