import { game } from '../../game.js';
import type { GameState } from '../game-state.js';

/** Waits for the specified element to be clicked. */
export class WaitForClick implements GameState {
  /**
   * Creates a state that waits for `htmlElement` to be clicked.
   * @param htmlElement The HTML element waiting to be clicked.
   */
  public constructor(htmlElement: HTMLElement) {
    htmlElement.onclick = () => game.popState();
  }
}
