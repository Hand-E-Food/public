import { Card } from '../cards/common/index.js';
import { Container } from '../container.js';
import type { Item } from '../item.js';
import { Spacing } from './constants.js';
import { ZIndex } from './z-index.js';

/** Contains cards actively providing positive morale. */
export class PositiveStack extends Container {
  public static readonly left = Spacing;
  public static readonly top = Spacing;
  public static readonly width = Card.width;

  /** The total positive morale in this stack. This is a positive number. */
  public get morale(): number {
    return this.items.reduce((total, item) => total + (item as Card).activeSide.morale, 0);
  }

  override addItems(...items: Item[]): Promise<void> {
    for (const item of items) {
      if (!(item instanceof Card) || item.activeSide.morale <= 0) {
        throw new Error('Only cards with positive morale can be added to the positive stack.');
      }
    }
    return super.addItems(...items);
  }

  protected async arrange(): Promise<void> {
    const promises: Promise<void>[] = [];
    const step = Card.titleHeight;
    let top = PositiveStack.top - step;
    let zIndex = ZIndex.LowerStack;
    for (const item of this.items) {
      if (!(item instanceof Card)) throw new Error('Positive stack can only contain cards.');
      top += step * item.activeSide.morale;
      promises.push(item.move({ left: `${PositiveStack.left}px`, top: `${top}px` }, zIndex));
      zIndex++;
    }
    await Promise.all(promises);
  }
}
