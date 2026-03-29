import { Sequence } from '../sequence.js';
import { HideElement } from './hide-element.js';
import { ShowElement } from './show-element.js';
import { WaitForClick } from './wait-for-click.js';

/** Temporarily displays a HTML Element until it is clicked. */
export class Report extends Sequence {
  /**
   * Creates a state temporarily displaying a HTML element.
   * @param htmlElement The HTML element to display.
   */
  public constructor(htmlElement: HTMLElement) {
    super([new ShowElement(htmlElement), new WaitForClick(htmlElement), new HideElement(htmlElement)]);
  }
}
