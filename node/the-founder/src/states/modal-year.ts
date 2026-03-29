import { game } from '../game.js';
import { Modal } from './animations/index.js';

/** Displays the current year. */
export class ModalYear extends Modal {
  public constructor() {
    const htmlElement = document.createElement('div');
    htmlElement.classList.add('fade', 'year');
    htmlElement.innerHTML = `<span>${game.year}</span>`;
    super(htmlElement);
  }
}
