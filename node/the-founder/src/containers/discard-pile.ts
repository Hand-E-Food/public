import { Card } from '../cards/index.js';
import { Spacing } from './constants.js';
import { Container } from './container.js';
import { ZIndex } from './index.js';

export class DiscardPile extends Container {
  public static readonly right = Spacing;
  public static readonly bottom = Spacing;
  public static readonly width = Card.width + 5;
  public static readonly height = Card.height + 5;

  protected override async arrange(): Promise<void> {
    const promises: Promise<void>[] = [];
    let left = -DiscardPile.right - Card.width - 5;
    let top = -DiscardPile.bottom - Card.height;
    let zIndex = 0;
    for (const item of this.items) {
      promises.push(
        item.move({ left: `calc(100vw + ${left}px)`, top: `calc(100vh + ${top}px)` }, ZIndex.LowerStack + zIndex),
      );
      zIndex++;
      if (zIndex < 6) {
        left += 1;
        top -= 1;
      }
    }
    await Promise.all(promises);
  }
}
