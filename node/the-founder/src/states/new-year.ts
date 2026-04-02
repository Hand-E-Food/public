import { stateMachine } from '../singleton/index.js';
import type { GameState } from './primitive/game-state.js';

/** Prepares the game for a new year. */
export class NewYear implements GameState {
  enter(): void {
    stateMachine.pop();
  }
}
