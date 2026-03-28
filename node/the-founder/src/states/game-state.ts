import type { Item } from '../item.js';

export interface GameState {
  /** Called when this state is entered. */
  enter?(): void;

  /** Called when a sub-state is pushed. */
  pause?(): void;

  /** Called when a sub-state is popped. */
  resume?(): void;

  /** Called when this state is exited. */
  exit?(): void;

  /** Called when an item is clicked. */
  onItemClicked?(item: Item, modifier: number): void;
}
