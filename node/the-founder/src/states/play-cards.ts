import { type GameState, stateMachine } from '../state-machine.js';

export class PlayCards implements GameState {
  enter(): void {
    stateMachine.pop();
  }
}
