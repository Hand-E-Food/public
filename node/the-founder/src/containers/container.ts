import type { Item } from '../item.js';
import { game } from '../singleton/game.js';

/** Arranges cards on part of the game area. */
export abstract class Container {
  /** The cards in this container. */
  public readonly items: Item[] = [];

  /**
   * Move items to this container.
   * @param items The items to add to this container.
   */
  // Use the spread operation to ensure the array is not modified during the operation.
  public async addItems(...items: Item[]): Promise<void> {
    const removePromise = game.removeFromContainers(items);
    for (const card of items) {
      card.container = this;
      console.log(`Moving ${card.name} to ${this.constructor.name}`);
    }
    this.items.push(...items);
    await Promise.all([removePromise, this.arrange()]);
  }

  /**
   * Remove an item from this container.
   * @param item The item to remove.
   */
  public removeItems(...items: Item[]): Promise<void> {
    for (const item of items) item.container = undefined;
    for (let i = this.items.length - 1; i >= 0; i--) {
      if (items.includes(this.items[i]!)) this.items.splice(i, 1);
    }
    return this.arrange();
  }

  /** Arranges this container's items. */
  protected abstract arrange(): Promise<void>;
}
