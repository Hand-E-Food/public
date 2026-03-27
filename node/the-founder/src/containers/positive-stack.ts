import { Card } from '../cards/index.js';
import type { Item } from '../item.js';
import { Spacing } from './constants.js';
import { Container } from './container.js';

/** Contains cards actively providing positive morale. */
export class PositiveStack extends Container {
  override addItems(...items: Item[]): void {
    for (const item of items) {
      if (!(item instanceof Card) || item.activeSide.morale <= 0) {
        throw new Error('Only cards with positive morale can be added to the positive stack.');
      }
    }
    super.addItems(...items);
  }

  protected arrange(): void {
    const step = Card.titleHeight;
    const left = Spacing;
    let top = Spacing - step;
    let zIndex = 100;
    for (const item of this.items) {
      if (!(item instanceof Card)) throw new Error('Positive stack can only contain cards.');
      top += step * item.activeSide.morale;
      item.reposition(left, top, zIndex);
      zIndex++;
    }
  }
}
