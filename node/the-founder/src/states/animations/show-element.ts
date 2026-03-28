import type { GameState } from '../game-state.js';
import { game } from '../../game.js';

/** Adds a HTML element to the game and fades it in. */
export class ShowElement implements GameState {
  /**
   * Creates an animation to show a HTML element.
   * @param htmlElement The HTML element to show.
   */
  public constructor(private readonly htmlElement: HTMLElement) {}

  enter(): void {
    this.htmlElement.style.opacity = '0%';
    game.htmlElement.appendChild(this.htmlElement);
    setTimeout(() => this.fadeIn(), 1);
  }

  private fadeIn(): void {
    this.htmlElement.style.opacity = '100%';
    setTimeout(() => game.popState(), 500);
  }
}
