import { Container } from './container.js';
import { Card } from '../cards/index.js';
import { Spacing } from './constants.js';
import { ZIndex } from './index.js';

export class DiscardPile extends Container {
  protected override arrange(): void {
    let left = Spacing + Card.width + 5;
    let top = Spacing + Card.height;
    let zIndex = 0;
    for (const item of this.items) {
      item.reposition(`calc(100vw - ${left}px)`, `calc(100vh - ${top}px)`, ZIndex.LowerStack + zIndex);
      zIndex++;
      if (zIndex < 5) {
        left += 1;
        top += 1;
      }
    }
  }
}
