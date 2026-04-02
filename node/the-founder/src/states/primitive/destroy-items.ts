import type { Item } from '../../item.js';
import { game, stateMachine } from '../../singleton/index.js';
import type { GameState } from './game-state.js';

export class DestroyItems implements GameState {
  private readonly items: Item[];
  public readonly name: string;

  // Use the spread operation to ensure the array is not modified during the operation.
  public constructor(...items: Item[]) {
    this.items = items;
    this.name = `DestroyItems(${items.map((item) => item.name).join(', ')})`;
  }

  enter(): void {
    for (const item of this.items) item.htmlElement.style.opacity = '0%';
    setTimeout(() => stateMachine.pop(), 500);
  }

  exit(): void {
    game.removeItems(...this.items);
  }
}
