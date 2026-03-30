import { game } from '../game.js';
import type { GameState } from './game-state.js';

export class PlayCards implements GameState {
  enter(): void {
    game.popState();
  }
}
