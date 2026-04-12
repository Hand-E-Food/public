import type { YearEndedProperties } from '../events/index.js';
import { game } from '../singleton/index.js';
import { GameOver } from './game-over.js';

/** Ensures morale is not a net negative. */
export class CheckMorale {
  public async execute(props: YearEndedProperties): Promise<void> {
    if (game.containers.negativeStack.morale > game.containers.positiveStack.morale) {
      props.gameOver = () => new GameOver().execute();
      props.stop = true;
    }
  }
}
