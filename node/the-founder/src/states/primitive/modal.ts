import { AwaitPromise } from './await-promise.js';
import { HideElement } from './hide-element.js';
import { Sequence } from './sequence.js';
import { ShowElement } from './show-element.js';

/** Temporarily displays a HTML Element until it is clicked. */
export class Modal extends Sequence {
  /**
   * Creates a state temporarily displaying a HTML element.
   * @param htmlElement The HTML element to display.
   * @param promise A promise that when resolved, closes the modal. If not provided, the modal will close when the HTML
   * element is clicked.
   */
  public constructor(htmlElement: HTMLElement, promise?: Promise<void>) {
    promise ??= new Promise((resolve) => {
      htmlElement.onclick = () => resolve();
    });
    super([new ShowElement(htmlElement), new AwaitPromise(promise), new HideElement(htmlElement)]);
  }
}
