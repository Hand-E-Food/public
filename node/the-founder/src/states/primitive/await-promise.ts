import { stateMachine } from '../../singleton/index.js';
import type { GameState } from './game-state.js';

/** Waits for the specified promise to be resolved. */
export class AwaitPromise implements GameState {
  public readonly name = 'AwaitPromise';

  /**
   * Creates a state that waits for the specified promise to be resolved.
   * @param promise The promise to await.
   */
  public constructor(private readonly promise: Promise<void>) {}

  enter(): void {
    this.promise.then(() => stateMachine.pop());
  }
}
