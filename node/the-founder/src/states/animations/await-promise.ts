import { type GameState, stateMachine } from '../../state-machine.js';

/** Waits for the specified promise to be resolved. */
export class AwaitPromise implements GameState {
  /**
   * Creates a state that waits for the specified promise to be resolved.
   * @param promise The promise to await.
   */
  public constructor(promise: Promise<void>) {
    promise.then(() => stateMachine.pop());
  }
}
