import type { CardSide } from './card-side.js';
import { Card } from './card.js';

/** A card that has a face and a back. */
export abstract class DeckCard extends Card {
  protected abstract readonly back: CardSide;
  protected abstract readonly face: CardSide;

  /** Flips this card face down. */
  public async flipDown(): Promise<void> {
    await this.flipTo(this.back);
  }

  /** Flips this card face up. */
  public async flipUp(): Promise<void> {
    await this.flipTo(this.face);
  }
}
