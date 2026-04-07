import { game } from '../singleton/index.js';
import { Modal } from './modal.js';

/** Displays the current year. */
export class ModalYear extends Modal {
  private readonly year: HTMLSpanElement;

  public constructor() {
    const modal = document.createElement('div');
    modal.classList.add('modal', 'year');

    const year = document.createElement('h1');
    modal.appendChild(year);

    super(modal);
    this.year = year;
  }

  public override async execute(): Promise<void> {
    this.year.textContent = game.year.toString();
    await super.execute();
  }
}
