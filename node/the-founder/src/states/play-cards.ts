import { stateMachine } from '../singleton/index.js';
import type { GameState } from './primitive/index.js';

export class PlayCards implements GameState {
  public readonly name: string = 'PlayCards';

  enter(): void {
    stateMachine.pop();
  }
}
