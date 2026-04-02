import { game } from '../singleton/index.js';
import { Modal } from './primitive/index.js';

/** Displays the current year. */
export class ModalYear extends Modal {
  public override readonly name: string;

  public constructor() {
    const htmlElement = document.createElement('div');
    htmlElement.classList.add('fade', 'year');
    htmlElement.innerHTML = `<span>${game.year}</span>`;
    super(htmlElement);
    this.name = `ModalYear(${game.year})`;
  }
}
