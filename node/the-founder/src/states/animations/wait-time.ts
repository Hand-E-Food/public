import { type GameState, stateMachine } from '../../state-machine.js';

/** Waits for a specific duration. */
export class WaitTime implements GameState {
  /**
   * Creates a state that waits for the specified duration.
   * @param milliseconds The duration to wait, in milliseconds.
   */
  public constructor(private readonly milliseconds: number) {}

  enter(): void {
    setTimeout(() => stateMachine.pop(), this.milliseconds);
  }
}
