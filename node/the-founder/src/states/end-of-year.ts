import { eventHub, game, GameEvent, stateMachine } from '../singleton/index.js';
import { GameOver } from './game-over.js';
import { ModalYear } from './modal-year.js';
import { NewYear } from './new-year.js';
import { PlayCards } from './play-cards.js';
import { type GameState, Sequence } from './primitive/index.js';

/** Runs the end-of-year operations and verifications. */
export class EndOfYear implements GameState {
  public constructor() {
    eventHub.add(GameEvent.NewYear, 50, () => this.checkMorale());
  }

  enter(): void {
    stateMachine.push(new PlayCards());
  }

  resume(): void {
    const endState = eventHub.invoke(GameEvent.EndYear);
    if (endState) {
      stateMachine.push(endState);
    } else {
      game.year++;
      stateMachine.push(new AnnualSequence());
    }
  }

  private checkMorale(): GameState | undefined {
    return game.containers.negativeStack.morale > game.containers.positiveStack.morale ? new GameOver() : undefined;
  }
}

class AnnualSequence extends Sequence {
  public constructor() {
    super([new ModalYear(), new NewYear(), new PlayCards()]);
  }
}
