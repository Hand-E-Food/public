import type { Item } from '../../item.js';
import { game, stateMachine } from '../../singleton/index.js';
import type { GameState } from './game-state.js';

export class DestroyItems implements GameState {
  public constructor(private readonly items: Item[]) {}

  enter(): void {
    for (const item of this.items) item.htmlElement.style.opacity = '0%';
    setTimeout(() => stateMachine.pop(), 500);
  }

  exit(): void {
    game.removeItems(this.items);
  }
}
