import { Card } from '../cards/common/index.js';
import { Container } from '../container.js';
import { Spacing } from './constants.js';
import { NegativeStack } from './negative-stack.js';
import { ZIndex } from './z-index.js';

/** Contains families that have been fed. */
export class FedFamilyStack extends Container {
  public static readonly left = NegativeStack.left + NegativeStack.width + Spacing;
  public static readonly top = Spacing;
  public static readonly width = Card.width;

  protected async arrange(): Promise<void> {
    const promises: Promise<void>[] = [];
    const step = Card.titleHeight;
    let top = FedFamilyStack.top;
    let zIndex = ZIndex.UpperStack;
    for (const item of this.items) {
      promises.push(item.move({ left: `${FedFamilyStack.left}px`, top: `${top}px` }, zIndex));
      top += step;
      zIndex++;
    }
    await Promise.all(promises);
  }
}
