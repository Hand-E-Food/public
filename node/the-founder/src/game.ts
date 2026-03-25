import type { Card } from "./card.js";

export class Game {
    private static readonly cards: Card[] = [];
    public static readonly htmlElement: HTMLDivElement = document.createElement('div');

    public static addCard(card: Card): void {
        if (Game.cards.includes(card)) throw new Error('Cannot add the same card twice.');
        Game.cards.push(card);
        Game.htmlElement.appendChild(card.htmlElement);
    }

    public static removeCard(card: Card): void {
        const i = Game.cards.indexOf(card);
        if (i === -1) throw new Error('Cannot remove a card that wasn\'t added.');
        card.htmlElement.remove();
        Game.cards.splice(i, 1);
    }
}

Game.htmlElement.classList.add('game');
