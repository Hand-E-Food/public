import { stateMachine } from '../../singleton/index.js';
import type { GameState } from './game-state.js';

/** Fades out a HTML element and removes it from the game. */
export class HideElement implements GameState {
  /**
   * Creates an animation to hide a HTML element.
   * @param htmlElement The HTML element to show.
   */
  public constructor(private readonly htmlElement: HTMLElement) {}

  enter(): void {
    this.htmlElement.style.opacity = '0%';
    setTimeout(() => stateMachine.pop(), 500);
  }

  exit(): void {
    this.htmlElement.remove();
  }
}
