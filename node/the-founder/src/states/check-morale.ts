import { game } from '../singleton/index.js';
import type { EndYearProperties } from './end-year.js';
import { GameOver } from './game-over.js';
import { Instant } from './primitive/index.js';

/** Ensures morale is not a net negative. */
export class CheckMorale extends Instant {
  public readonly name: string = 'CheckMorale';

  public constructor(private readonly props: EndYearProperties) {
    super();
  }

  override execute(): void {
    if (!this.props.nextState && game.containers.negativeStack.morale > game.containers.positiveStack.morale) {
      this.props.nextState = new GameOver();
    }
  }
}
