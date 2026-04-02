import { stateMachine } from '../../singleton/index.js';
import type { GameState } from './game-state.js';

/** Iterates through a sequence of states. */
export class Sequence implements GameState {
  public readonly name: string;

  /**
   * Creates a new sequence.
   * @param states The states to iterate through.
   */
  public constructor(private readonly states: GameState[]) {
    this.name = `Sequence(${states.map((state) => state.name).join(', ')})`;
  }

  enter(): void {
    this.next();
  }

  resume(): void {
    this.next();
  }

  private next(): void {
    const nextState = this.states.shift();
    if (!nextState) stateMachine.pop();
    else stateMachine.push(nextState);
  }
}
