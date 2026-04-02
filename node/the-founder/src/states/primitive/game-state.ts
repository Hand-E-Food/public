import type { Item } from '../../item.js';

/** A state of the game. */

export interface GameState {
  /**
   * This state's name, used for logging.
   */
  readonly name: string;

  /**
   * Called when this state is entered.
   */
  enter?(): void;

  /**
   * Called when a sub-state is pushed. Do not perform state transition in this method.
   * @throws AbortStateTransitionError to abort the state transition.
   */
  pause?(): void;

  /**
   * Called when a sub-state is popped.
   */
  resume?(): void;

  /**
   * Called when this state is exited. Do not perform state transition in this method.
   * @throws AbortStateTransitionError to abort the state transition.
   */
  exit?(): void;

  /**
   * Called when an item is clicked.
   * @param item The clicked item.
   * @param modifier The modifier state. 0 for no modifier, 1-3 typically referring to the item's actions.
   */
  onItemClicked?(item: Item, modifier: number): void;
}
