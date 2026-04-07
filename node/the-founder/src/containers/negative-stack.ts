import { Card } from '../cards/index.js';
import type { Item } from '../item.js';
import { Container } from './container.js';
import { PositiveStack } from './positive-stack.js';
import { ZIndex } from './z-index.js';

/** Contains cards actively providing negative morale. */
export class NegativeStack extends Container {
  public static readonly left = PositiveStack.left + Card.width / 3;
  public static readonly top = PositiveStack.top;
  public static readonly width = Card.width;

  /** The total negative morale in this stack. This is a positive number. */
  public get morale(): number {
    return this.items.reduce((total, item) => total - (item as Card).activeSide.morale, 0);
  }

  override addItems(...items: Item[]): Promise<void> {
    for (const item of items) {
      if (!(item instanceof Card) || item.activeSide.morale >= 0) {
        throw new Error('Only cards with negative morale can be added to the negative stack.');
      }
    }
    return super.addItems(...items);
  }

  protected async arrange(): Promise<void> {
    const promises: Promise<void>[] = [];
    const step = Card.titleHeight;
    let top = NegativeStack.top - step;
    let zIndex = ZIndex.UpperStack;
    for (const item of this.items) {
      if (!(item instanceof Card)) throw new Error('Negative stack can only contain cards.');
      top -= step * item.activeSide.morale;
      promises.push(item.move({ left: `${NegativeStack.left}px`, top: `${top}px` }, zIndex));
      zIndex++;
    }
    await Promise.all(promises);
  }
}
