import { Primitive } from './primitive.js';

/** Displays the game over screen. */
export class GameOver {
  private readonly htmlElement: HTMLElement;

  public constructor() {
    const htmlElement = document.createElement('div');
    htmlElement.classList.add('modal', 'game-over');
    htmlElement.innerHTML = `<h1>Game Over</h1>`;

    this.htmlElement = htmlElement;
  }

  public execute(): Promise<void> {
    return Primitive.fadeIn(this.htmlElement, 2000);
  }
}
