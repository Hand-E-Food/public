import { stateMachine } from '../singleton/index.js';
import type { GameState } from './primitive/index.js';

export class PlayCards implements GameState {
  enter(): void {
    stateMachine.pop();
  }
}
