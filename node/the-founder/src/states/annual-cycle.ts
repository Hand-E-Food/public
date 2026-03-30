import { game } from '../game.js';
import { GameOver } from './game-over.js';
import type { GameState } from './game-state.js';
import { ModalYear } from './modal-year.js';
import { NewYear } from './new-year.js';
import { PlayCards } from './play-cards.js';
import { Sequence } from './sequence.js';

/** Runs the annual cycle, performing automatic operations and verifications. */
export class AnnualCycle implements GameState {
  enter(): void {
    game.pushState(new PlayCards());
  }

  resume(): void {
    const endState = this.validate();
    if (endState) {
      game.pushState(endState);
      return;
    }
    game.year++;
    game.pushState(new Sequence([new ModalYear(), new NewYear(), new PlayCards()]));
  }

  private validate(): GameState | undefined {
    if (game.containers.negativeStack.morale > game.containers.positiveStack.morale) return new GameOver();
    return undefined;
  }
}
