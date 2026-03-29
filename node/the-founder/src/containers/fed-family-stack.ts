import { Card } from '../cards/card.js';
import { Spacing } from './constants.js';
import { Container } from './container.js';
import { ZIndex } from './index.js';

/** Contains families that have been fed. */
export class FedFamilyStack extends Container {
  protected arrange(): void {
    const step = Card.titleHeight;
    const left = Spacing * 2 + Card.width * 1.5;
    let top = Spacing - step;
    let zIndex = ZIndex.UpperStack;
    for (const item of this.items) {
      top += step;
      item.reposition(left, top, zIndex);
      zIndex++;
    }
  }
}
