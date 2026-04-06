import { game } from '../singleton/index.js';
import { Modal } from './modal.js';

/** Displays the current year. */
export class ModalYear extends Modal {
  private readonly year: HTMLSpanElement;

  public constructor() {
    const htmlElement = document.createElement('div');
    htmlElement.classList.add('year');

    const year = document.createElement('span');
    htmlElement.appendChild(year);

    super(htmlElement);
    this.year = year;
  }

  public override async execute(): Promise<void> {
    this.year.textContent = game.year.toString();
    await super.execute();
  }
}
