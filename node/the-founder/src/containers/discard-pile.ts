import { Card } from '../cards/index.js';
import { Spacing } from './constants.js';
import { Container } from './container.js';
import { ZIndex } from './index.js';

export class DiscardPile extends Container {
  protected override async arrange(): Promise<void> {
    const promises: Promise<void>[] = [];
    let left = Spacing + Card.width + 5;
    let top = Spacing + Card.height;
    let zIndex = 0;
    for (const item of this.items) {
      promises.push(
        item.move({ left: `calc(100vw - ${left}px)`, top: `calc(100vh - ${top}px)` }, ZIndex.LowerStack + zIndex),
      );
      zIndex++;
      if (zIndex < 5) {
        left += 1;
        top += 1;
      }
    }
    await Promise.all(promises);
  }
}
