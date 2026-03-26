import type { Card } from "./card.js";

/** A singleton game environment. */
export class Game {
    private static readonly cards: Card[] = [];

    /** The game's HTML element. */
    public static readonly htmlElement: HTMLDivElement = document.createElement('div');

    /**
     * Adds cards to the game.
     * @param cards The cards to add.
     */
    public static addCards(...cards: Card[]): void {
        for (const card of cards) {
            if (Game.cards.includes(card)) throw new Error('Cannot add the same card twice.');
            Game.htmlElement.appendChild(card.htmlElement);
        }
        Game.cards.push(...cards);
    }

    /** Removes a card from the game. */
    public static removeCard(card: Card): void {
        const i = Game.cards.indexOf(card);
        if (i === -1) throw new Error('Cannot remove a card that wasn\'t added.');
        card.htmlElement.remove();
        Game.cards.splice(i, 1);
    }
}

// static constructor() {
Game.htmlElement.classList.add('game');
