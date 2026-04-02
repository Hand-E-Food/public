import { stateMachine } from '../../singleton/index.js';
import type { GameState } from './game-state.js';

/** Waits for a specific duration. */
export class AwaitTime implements GameState {
  public readonly name: string;

  /**
   * Creates a state that waits for the specified duration.
   * @param milliseconds The duration to wait, in milliseconds.
   */
  public constructor(private readonly milliseconds: number) {
    this.name = `AwaitTime(${milliseconds}ms)`;
  }

  enter(): void {
    setTimeout(() => stateMachine.pop(), this.milliseconds);
  }
}
