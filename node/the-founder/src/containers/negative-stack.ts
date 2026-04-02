import { Card } from '../cards/index.js';
import type { Item } from '../item.js';
import { Spacing } from './constants.js';
import { Container } from './container.js';
import { ZIndex } from './z-index.js';

/** Contains cards actively providing negative morale. */
export class NegativeStack extends Container {
  /** The total negative morale in this stack. This is a positive number. */
  public get morale(): number {
    return this.items.reduce((total, item) => total - (item as Card).activeSide.morale, 0);
  }

  override addItems(...items: Item[]): void {
    for (const item of items) {
      if (!(item instanceof Card) || item.activeSide.morale >= 0) {
        throw new Error('Only cards with negative morale can be added to the negative stack.');
      }
    }
    super.addItems(...items);
  }

  protected arrange(): void {
    const step = Card.titleHeight;
    const left = Spacing + Card.width / 2;
    let top = Spacing - step;
    let zIndex = ZIndex.UpperStack;
    for (const item of this.items) {
      if (!(item instanceof Card)) throw new Error('Negative stack can only contain cards.');
      top -= step * item.activeSide.morale;
      item.reposition(left, top, zIndex);
      zIndex++;
    }
  }
}
