import { type GameState, stateMachine } from '../state-machine.js';

/** Prepares the game for a new year. */
export class NewYear implements GameState {
  enter(): void {
    stateMachine.pop();
  }
}
