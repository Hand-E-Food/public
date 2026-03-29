import { game } from '../../game.js';
import type { Item } from '../../item.js';
import type { GameState } from '../game-state.js';

export class DestroyItems implements GameState {
  public constructor(private readonly items: Item[]) {}

  enter(): void {
    for (const item of this.items) item.htmlElement.style.opacity = '0%';
    setTimeout(() => game.popState(), 500);
  }

  exit(): void {
    game.removeItems(this.items);
  }
}
