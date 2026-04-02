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
import { stateMachine } from './state-machine.js';

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
      item.onClickedListener = (item, modifier) => this.onItemClicked(item, modifier);
      this.htmlElement.appendChild(item.htmlElement);
    }
    this.items.push(...items);
    container?.addItems(...items);
  }

  /** Removes an item from this game. */
  // Use the spread operation to ensure the array is not modified during the operation.
  public removeItems(...items: Item[]): void {
    for (const item of items) {
      item.container?.removeItem(item);
      const i = this.items.indexOf(item);
      if (i === -1) throw new Error('Cannot remove an item that was not added.');
      item.htmlElement.remove();
      this.items.splice(i, 1);
    }
  }

  /** Passes the click event to the current state. */
  public onItemClicked(item: Item, modifier: number): void {
    stateMachine.current?.onItemClicked?.(item, modifier);
  }
}

/** A singleton instance of the game. */
export const game = new Game();
