import { Card } from '../cards/index.js';
import { Spacing } from './constants.js';
import { Container } from './container.js';
import { DiscardPile, DrawDeck, ZIndex } from './index.js';

export class Hand extends Container {
  public static readonly left = DrawDeck.left + DrawDeck.width + Spacing;
  public static readonly bottom = Spacing;
  public static readonly right = DiscardPile.right + DiscardPile.width + Spacing;
  public static readonly height = Card.height;

  protected override async arrange(): Promise<void> {
    const promises: Promise<void>[] = [];
    const step = Card.width + Spacing / 2;
    let left = Hand.left;
    const top = -Hand.bottom - Hand.height;
    let zIndex = ZIndex.LowerStack;
    for (const item of this.items) {
      promises.push(item.move({ left: `${left}px`, top: `calc(100vh + ${top}px)` }, zIndex));
      left += step;
      zIndex++;
    }
    await Promise.all(promises);
  }
}
