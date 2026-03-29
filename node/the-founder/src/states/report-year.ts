import { Report } from './animations/index.js';
import { game } from '../game.js';

/** Displays the current year. */
export class ReportYear extends Report {
  public constructor() {
    const htmlElement = document.createElement('div');
    htmlElement.classList.add('year');
    htmlElement.innerHTML = `<span>${game.year}</span>`;
    super(htmlElement);
  }
}
