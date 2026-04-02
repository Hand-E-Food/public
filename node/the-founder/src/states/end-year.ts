import { eventHub, game, GameEvent, stateMachine } from '../singleton/index.js';
import { ModalYear } from './modal-year.js';
import { NewYear } from './new-year.js';
import { PlayCards } from './play-cards.js';
import { type GameState, Sequence } from './primitive/index.js';

export interface EndYearProperties {
  nextState?: GameState;
}

/** Runs the end-of-year operations and verifications. */
export class EndYear implements GameState {
  private props!: EndYearProperties;

  public readonly name: string = 'EndYear';

  enter(): void {
    this.props = {};
    stateMachine.push(eventHub.invoke(GameEvent.EndYear, this.props));
  }

  resume(): void {
    if (this.props.nextState) {
      stateMachine.push(this.props.nextState);
    } else {
      game.year++;
      stateMachine.push(new AnnualSequence());
    }
  }
}

class AnnualSequence extends Sequence {
  public override readonly name: string = 'AnnualSequence';

  public constructor() {
    super([new ModalYear(), new NewYear(), new PlayCards()]);
  }
}
