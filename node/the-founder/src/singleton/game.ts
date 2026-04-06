import {
  BoosterPacks,
  BoosterTray,
  type Container,
  DiscardPile,
  DrawDeck,
  FedFamilyStack,
  Hand,
  NegativeStack,
  PositiveStack,
} from '../containers/index.js';
import type { Item } from '../item.js';
import { Primitive } from '../states/primitive.js';

/** A singleton game environment. */
export class Game {
  private readonly items: Item[] = [];

  /** This game's item containers. */
  public readonly containers = {
    boosterPacks: new BoosterPacks(),
    boosterTray: new BoosterTray(),
    discardPile: new DiscardPile(),
    drawDeck: new DrawDeck(),
    fedFamilyStack: new FedFamilyStack(),
    hand: new Hand(),
    negativeStack: new NegativeStack(),
    positiveStack: new PositiveStack(),
  };

  /** This game's HTML element. */
  public readonly htmlElement: HTMLDivElement;

  /** A callback function to be called when an item is clicked. */
  public onItemClicked: ((item: Item, modifier: number) => void) | null = null;

  /** The number of resources available per booster pack. */
  public readonly resourcesPerBooster = 4;

  /** The current game year. */
  public year: number = 1800;

  public constructor() {
    this.htmlElement = document.createElement('div');
    this.htmlElement.classList.add('game');
  }

  /**
   * Adds items to this game.
   * @param container The container to add these items to.
   * @param items The items to add.
   */
  // Use the spread operation to ensure the array is not modified during the operation.
  public addItems(container: Container | undefined, ...items: Item[]): void {
    for (const item of items) {
      if (this.items.includes(item)) throw new Error('Cannot add the same item twice.');
      item.onClickedListener = (item, modifier) => this.onItemClicked?.(item, modifier);
      this.htmlElement.appendChild(item.htmlElement);
    }
    this.items.push(...items);
    container?.addItems(...items);
  }

  /**
   * Destroys items.
   * @param items The items to destroy.
   */
  // Use the spread operation to ensure the array is not modified during the operation.
  public async destroyItems(...items: Item[]): Promise<void> {
    for (const item of items) item.htmlElement.onclick = null;
    await Promise.all(items.map((item) => Primitive.fadeOut(item.htmlElement, item.animationDuration)));
    for (let i = this.items.length - 1; i >= 0; i--) if (items.includes(this.items[i]!)) this.items.splice(i, 1);
    await this.removeFromContainers(items);
  }

  /**
   * Removes the specified items from their containers.
   * @param items The items to remove.
   */
  public async removeFromContainers(items: Item[]) {
    const promises: Promise<void>[] = [];
    for (const container of Object.values(this.containers)) {
      const filteredItems = items.filter((item) => item.container === container);
      promises.push(container.removeItems(...filteredItems));
    }
    await Promise.all(promises);
  }
}

/** A singleton instance of the game. */
export const game = new Game();
