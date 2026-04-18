import type { YearEndedProperties } from '../events/index.js';
import { game } from '../singleton/index.js';
import { GameOver } from './index.js';

export class YearEnded {
  public async checkMorale(props: YearEndedProperties): Promise<void> {
    if (game.containers.negativeStack.morale > game.containers.positiveStack.morale) {
      props.gameOver = () => new GameOver().execute();
      props.stop = true;
    }
  }

  public async discardHand(_props: YearEndedProperties): Promise<void> {
    await game.containers.discardPile.addItems(...game.containers.hand.items);
  }

  public async discardResources(_props: YearEndedProperties): Promise<void> {
    game.resources.clear();
  }
}
