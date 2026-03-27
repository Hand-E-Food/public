import type { Item } from "./item.js";
import { DrawDeck } from "./containers/draw-deck.js";
import { BoosterPacks, BoosterTray, DiscardPile, FedFamilyStack, Hand, NegativeStack, PositiveStack, type Container } from "./containers/index.js";

/** A singleton game environment. */
export class Game {
    private readonly items: Item[] = [];
    public readonly containers = {
        boosterPacks: new BoosterPacks(),
        boosterTray: new BoosterTray(),
        discardPile: new DiscardPile(),
        drawDeck: new DrawDeck(),
        fedFamilyStack: new FedFamilyStack(),
        hand: new Hand(),
        negativeStack: new NegativeStack(),
        positiveStack: new PositiveStack(),
    }

    /** This game's HTML element. */
    public readonly htmlElement: HTMLDivElement;

    public constructor() {
        this.htmlElement = document.createElement('div');
        this.htmlElement.classList.add('game');
    }

    /**
     * Adds items to this game.
     * @param container The container to add these items to.
     * @param items The items to add.
     */
    public addItems(container: Container, ...items: Item[]): void {
        for (const item of items) {
            if (this.items.includes(item)) throw new Error('Cannot add the same item twice.');
            this.htmlElement.appendChild(item.htmlElement);
        }
        this.items.push(...items);
        container.addItems(...items);
    }

    /** Removes an item from this game. */
    public removeItem(item: Item): void {
        const i = this.items.indexOf(item);
        if (i === -1) throw new Error('Cannot remove a item that wasn\'t added.');
        item.htmlElement.remove();
        this.items.splice(i, 1);
    }
}

export const game = new Game();
