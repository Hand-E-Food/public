import { BoosterPack } from '../boosters/booster-pack.js';
import { Card } from '../cards/index.js';
import type { Item } from '../item.js';
import { Spacing } from './constants.js';
import { Container } from './container.js';
import { FedFamilyStack } from './fed-family-stack.js';
import { ZIndex } from './z-index.js';

export class BoosterPacks extends Container {
  override addItems(...items: Item[]): Promise<void> {
    for (const item of items) {
      if (!(item instanceof BoosterPack)) throw new Error('Only boosters can be added to the booster pack container.');
    }
    return super.addItems(...items);
  }

  protected async arrange(): Promise<void> {
    const promises: Promise<void>[] = [];
    const step = FedFamilyStack.left + FedFamilyStack.width + Spacing;
    let left = Spacing * 3 + Card.width * 2.5;
    const top = Spacing;
    let zIndex = ZIndex.LowerStack;
    for (const item of this.items) {
      promises.push(item.move({ left: `${left}px`, top: `${top}px` }, zIndex));
      left += step;
      zIndex++;
    }
    await Promise.all(promises);
  }
}
