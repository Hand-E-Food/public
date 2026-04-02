import type { Card } from '../../cards/index.js';
import type { Container } from '../../containers/index.js';
import { MoveItems } from './move-item.js';

/** Moves and flips cards. */
export class FlipCards extends MoveItems {
  public override readonly name: string;

  /**
   * Creates an animation of moving items.
   * @param container The container to move the items to.
   * @param items The items to move.
   */
  // Use the spread operation to ensure the array is not modified during the operation.
  public constructor(container: Container, ...cards: Card[]) {
    super(container, ...cards);
    this.name = `FlipCards(${cards.map((card) => card.name).join(', ')})`;
  }

  override enter(): void {
    for (const card of this.items as Card[]) card.flip();
    super.enter();
  }
}
