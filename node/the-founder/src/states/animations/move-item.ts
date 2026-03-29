import type { Container } from '../../containers/container.js';
import { game } from '../../game.js';
import { Item } from '../../item.js';
import type { GameState } from '../game-state.js';

/** Moves items to a container and waits until it is finished. */
export class MoveItems implements GameState {
  /**
   * Creates an animation of moving items.
   * @param container The container to move the items to.
   * @param items The items to move.
   */
  public constructor(
    private readonly container: Container,
    private readonly items: Item[],
  ) {}

  enter(): void {
    if (this.items.length === 0) {
      game.popState();
      return;
    }
    this.container.addItems(this.items);
    const transitionTime = Math.max(...this.items.map((item) => item.transitionTime));
    setTimeout(() => game.popState(), transitionTime);
  }
}
