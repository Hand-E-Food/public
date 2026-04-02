import { stateMachine } from '../../singleton/index.js';
import type { GameState } from './game-state.js';

/** A state that executes instantly and then returns to the previous state. */
export abstract class Instant implements GameState {
  public abstract readonly name: string;

  enter(): void {
    try {
      stateMachine.lock('in an instant state');
      this.execute();
    } finally {
      stateMachine.unlock();
    }
    stateMachine.pop();
  }

  /** Executes this game state. Do not call the state machine. */
  protected abstract execute(): void;
}
