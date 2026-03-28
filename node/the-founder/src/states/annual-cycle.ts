import type { GameState } from './game-state.js';
import type { Item } from '../item.js';

/** Runs the annual cycle, performing automatic operations and verifications. */
export class AnnualCycle implements GameState {
  enter(): void {}
  pause(): void {}
  resume(): void {}
  exit(): void {}
  onItemClicked(_item: Item, _modifier: number): void {}
}
