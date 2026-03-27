import { Settlement } from '../boosters/index.js';
import type { GameState } from './game-state.js';
import type { Item } from '../item.js';
import { game } from '../game.js';

export class NewGame implements GameState {
  enter(): void {
    document.body.appendChild(game.htmlElement);
    const boosterPack = new Settlement();
    game.addItems(game.containers.boosterTray, [boosterPack]);
  }

  pause(): void {}

  resume(): void {}

  exit(): void {}

  onItemClicked(_item: Item, _modifier: number): void {}
}
