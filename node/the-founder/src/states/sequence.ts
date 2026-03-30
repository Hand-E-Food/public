import { type GameState, stateMachine } from '../state-machine.js';

/** Iterates through a sequence of states. */
export class Sequence implements GameState {
  /**
   * Creates a new sequence.
   * @param states The states to iterate through.
   */
  public constructor(private readonly states: GameState[]) {}

  enter(): void {
    this.next();
  }

  resume(): void {
    this.next();
  }

  private next(): void {
    const nextState = this.states.shift();
    if (!nextState) stateMachine.pop();
    //else if (this.states.length === 0) stateMachine.nextState(nextState);
    else stateMachine.push(nextState);
  }
}
