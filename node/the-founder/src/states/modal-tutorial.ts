import { stateMachine } from '../singleton/index.js';
import { ManualPromise } from '../util/index.js';
import { type GameState, Modal } from './primitive/index.js';

export interface ModalTutorialParams {
  /** The paragraphs to display in the tutorial. */
  readonly paragraphs: string[];
  readonly left?: string;
  readonly width?: string;
  readonly right?: string;
  readonly top?: string;
  readonly height?: string;
  readonly bottom?: string;
}

/** Temporarily displays a tutorial message. */
export class ModalTutorial implements GameState {
  private static allClosed: boolean = false;
  private static readonly closed: Set<string> = new Set();

  private promise!: ManualPromise<void>;

  /**
   * Creates a state that temporarily displays a tutorial message.
   * @param key This tutorial's key.
   * @param params This tutorial's parameters.
   */
  public constructor(
    private readonly key: string,
    private readonly params: ModalTutorialParams,
  ) {}

  enter(): void {
    if (ModalTutorial.allClosed || ModalTutorial.closed.has(this.key)) {
      stateMachine.pop();
      return;
    }

    this.promise = new ManualPromise<void>();

    const htmlElement = document.createElement('div');
    htmlElement.classList.add('fade', 'tutorial');
    if (this.params.left) htmlElement.style.left = this.params.left;
    if (this.params.width) htmlElement.style.width = this.params.width;
    if (this.params.right) htmlElement.style.right = this.params.right;
    if (this.params.top) htmlElement.style.top = this.params.top;
    if (this.params.height) htmlElement.style.height = this.params.height;
    if (this.params.bottom) htmlElement.style.bottom = this.params.bottom;
    for (const paragraph of this.params.paragraphs) {
      const p = document.createElement('p');
      p.innerHTML = paragraph;
      htmlElement.appendChild(p);
    }

    const closeOneElement = document.createElement('p');
    closeOneElement.classList.add('action');
    closeOneElement.innerHTML = `"Thank you."`;
    closeOneElement.onclick = () => this.closeOne();
    htmlElement.appendChild(closeOneElement);

    const closeAllElement = document.createElement('p');
    closeAllElement.classList.add('action');
    closeAllElement.innerHTML = `"I do not require your tutelage."`;
    closeAllElement.onclick = () => this.closeAll();
    htmlElement.appendChild(closeAllElement);

    stateMachine.next(new Modal(htmlElement, this.promise));
  }

  private closeOne(): void {
    ModalTutorial.closed.add(this.key);
    this.promise.resolve();
  }

  private closeAll(): void {
    ModalTutorial.allClosed = true;
    this.promise.resolve();
  }
}
