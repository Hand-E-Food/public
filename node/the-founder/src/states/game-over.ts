import { ShowElement } from './primitive/show-element.js';

/** Displays the game over screen. */
export class GameOver extends ShowElement {
  public override readonly name: string = 'GameOver';

  public constructor() {
    const htmlElement = document.createElement('div');
    htmlElement.classList.add('game-over');
    htmlElement.innerHTML = `<h1>Game Over</h1>`;
    super(htmlElement);
  }
}
