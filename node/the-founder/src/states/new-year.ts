import { eventHub, GameEvent, stateMachine } from '../singleton/index.js';
import type { GameState } from './primitive/game-state.js';

/** Runs the new year operations. */
export class NewYear implements GameState {
  public readonly name: string = 'NewYear';

  enter(): void {
    stateMachine.push(eventHub.invoke(GameEvent.NewYear));
  }

  resume(): void {
    stateMachine.pop();
  }
}
