import type { Item } from '../item.js';

/** Arranges cards on part of the game area. */
export abstract class Container {
  /** The cards in this container. */
  public readonly items: Item[] = [];

  /**
   * Move items to this container.
   * @param items The items to add to this container.
   */
  // Use the spread operation to ensure the array is not modified during the operation.
  public addItems(...items: Item[]): void {
    for (const card of items) {
      card.container?.removeItem(card);
      card.container = this;
      console.log(`Moving ${card.name} to ${this.constructor.name}`);
    }
    this.items.push(...items);
    this.arrange();
  }

  /**
   * Remove an item from this container.
   * @param item The item to remove.
   */
  public removeItem(item: Item): void {
    const i = this.items.indexOf(item);
    if (i === -1) throw new Error('Cannot remove a item that is not in this container.');
    item.container = undefined;
    this.items.splice(i, 1);
    this.arrange();
  }

  /** Arranges this container's items. */
  protected abstract arrange(): void;
}
