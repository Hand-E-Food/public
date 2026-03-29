import { game } from '../../game.js';
import type { GameState } from '../game-state.js';

/** Waits for the specified promise to be resolved. */
export class AwaitPromise implements GameState {
  /**
   * Creates a state that waits for the specified promise to be resolved.
   * @param promise The promise to await.
   */
  public constructor(promise: Promise<void>) {
    promise.then(() => game.popState());
  }
}
