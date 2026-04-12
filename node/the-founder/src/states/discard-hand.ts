import type { YearEndedProperties } from '../events/index.js';
import { game } from '../singleton/index.js';

export class DiscardHand {
  public async execute(_props: YearEndedProperties): Promise<void> {
    await game.containers.discardPile.addItems(...game.containers.hand.items);
  }
}
