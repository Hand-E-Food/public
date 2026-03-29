import { game } from '../../game.js';
import type { GameState } from '../game-state.js';

/** Waits for a specific duration. */
export class WaitTime implements GameState {
  /**
   * Creates a state that waits for the specified duration.
   * @param milliseconds The duration to wait, in milliseconds.
   */
  public constructor(private readonly milliseconds: number) {}

  enter(): void {
    setTimeout(() => game.popState(), this.milliseconds);
  }
}
