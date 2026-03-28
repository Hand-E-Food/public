import type { GameState } from './game-state.js';
import { game } from '../game.js';

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
    if (!nextState) game.popState();
    else game.pushState(nextState);
  }
}
