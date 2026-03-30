import { game } from '../game.js';
import type { GameState } from './game-state.js';

/** Prepares the game for a new year. */
export class NewYear implements GameState {
  enter(): void {
    game.popState();
  }
}
