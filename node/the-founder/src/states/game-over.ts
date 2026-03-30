import { ShowElement } from './animations/show-element.js';

/** Displays the game over screen. */
export class GameOver extends ShowElement {
  public constructor() {
    const htmlElement = document.createElement('div');
    htmlElement.classList.add('game-over');
    htmlElement.innerHTML = `<h1>Game Over</h1>`;
    super(htmlElement);
  }
}
