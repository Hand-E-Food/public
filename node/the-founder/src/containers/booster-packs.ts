import { BoosterPack } from '../boosters/booster-pack.js';
import { Container } from './container.js';
import { Card } from '../cards/index.js';
import { Spacing } from './constants.js';
import type { Item } from '../item.js';

export class BoosterPacks extends Container {
  override addItems(...items: Item[]): void {
    for (const item of items) {
      if (!(item instanceof BoosterPack)) throw new Error('Only boosters can be added to the booster pack container.');
    }
    super.addItems(...items);
  }

  protected arrange(): void {
    const step = BoosterPack.width + Spacing;
    let left = Spacing * 3 + Card.width * 2.5;
    const top = Spacing;
    let zIndex = 0;
    for (const item of this.items) {
      item.reposition(left, top, zIndex);
      left += step;
      zIndex++;
    }
  }
}
