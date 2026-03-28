import { Container } from './container.js';
import { Card } from '../cards/index.js';
import { Spacing } from './constants.js';
import type { Item } from '../item.js';

/** Contains cards actively providing negative morale. */
export class NegativeStack extends Container {
  override addItems(items: Item[]): void {
    for (const item of items) {
      if (!(item instanceof Card) || item.activeSide.morale >= 0) {
        throw new Error('Only cards with negative morale can be added to the negative stack.');
      }
    }
    super.addItems(items);
  }

  protected arrange(): void {
    const step = Card.titleHeight;
    const left = Spacing + Card.width / 2;
    let top = Spacing - step;
    let zIndex = 200;
    for (const item of this.items) {
      if (!(item instanceof Card)) throw new Error('Negative stack can only contain cards.');
      top -= step * item.activeSide.morale;
      item.reposition(left, top, zIndex);
      zIndex++;
    }
  }
}
