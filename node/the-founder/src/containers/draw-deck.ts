import { Card } from '../cards/index.js';
import { Spacing } from './constants.js';
import { Container } from './container.js';
import { ZIndex } from './index.js';

/** The deck of cards to draw from. */
export class DrawDeck extends Container {
  protected override async arrange(): Promise<void> {
    const promises: Promise<void>[] = [];
    let left = Spacing;
    let top = Spacing + Card.height;
    let zIndex = 0;
    for (let i = 0; i < this.items.length; i++) {
      const item = this.items[i];
      if (!item) continue;
      promises.push(item.move({ left: `${left}px`, top: `calc(100vh - ${top}px)` }, ZIndex.LowerStack + zIndex));
      zIndex++;
      if (i < 5) {
        left += 1;
        top += 1;
      }
    }
    await Promise.all(promises);
  }
}
