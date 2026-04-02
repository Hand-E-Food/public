import type { Container } from '../../containers/container.js';
import { Item } from '../../item.js';
import { stateMachine } from '../../singleton/index.js';
import type { GameState } from './game-state.js';
import { AwaitTime } from './index.js';

/** Moves items to a container and waits until it is finished. */
export class MoveItems implements GameState {
  protected readonly items: Item[];

  public readonly name: string;

  /**
   * Creates an animation of moving items.
   * @param container The container to move the items to.
   * @param items The items to move.
   */
  // Use the spread operation to ensure the array is not modified during the operation.
  public constructor(
    protected readonly container: Container,
    ...items: Item[]
  ) {
    this.items = items;
    this.name = `MoveItems(${items.map((item) => item.name).join(', ')})`;
  }

  enter(): void {
    if (this.items.length === 0) {
      stateMachine.pop();
    } else {
      this.container.addItems(...this.items);
      const transitionTime = Math.max(...this.items.map((item) => item.transitionTime));
      stateMachine.next(new AwaitTime(transitionTime));
    }
  }
}
