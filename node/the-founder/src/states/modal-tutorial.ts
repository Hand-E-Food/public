import { tutorials } from '../singleton/index.js';
import { Modal } from './modal.js';
import { ManualPromise } from './manual-promise.js';

export interface ModalTutorialParams {
  /** This tutorial's unique key. */
  readonly key: string;
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
export class ModalTutorial extends Modal {
  /**
   * If the specified tutorial should be shown, exsecutes a state that temporarily displays the tutorial message.
   * @param params The tutorial's parameters.
   */
  public static async show(params: ModalTutorialParams): Promise<void> {
    if (!tutorials.shouldShow(params.key)) return;
    await new ModalTutorial(params).execute();
  }

  private readonly key: string;
  private readonly promise = new ManualPromise<void>();

  /**
   * Creates a state that temporarily displays a tutorial message.
   * @param params This tutorial's parameters.
   */
  private constructor(params: ModalTutorialParams) {
    const htmlElement = document.createElement('div');
    htmlElement.classList.add('fade', 'tutorial');
    if (params.left) htmlElement.style.left = params.left;
    if (params.width) htmlElement.style.width = params.width;
    if (params.right) htmlElement.style.right = params.right;
    if (params.top) htmlElement.style.top = params.top;
    if (params.height) htmlElement.style.height = params.height;
    if (params.bottom) htmlElement.style.bottom = params.bottom;
    for (const paragraph of params.paragraphs) {
      const p = document.createElement('p');
      p.innerHTML = paragraph;
      htmlElement.appendChild(p);
    }

    const closeElement = document.createElement('p');
    closeElement.classList.add('action');
    closeElement.innerHTML = `Close`;
    closeElement.onclick = () => this.close();
    htmlElement.appendChild(closeElement);

    const closeAlwaysElement = document.createElement('p');
    closeAlwaysElement.classList.add('action');
    closeAlwaysElement.innerHTML = `Don't show this again.`;
    closeAlwaysElement.onclick = () => this.closeAlways();
    htmlElement.appendChild(closeAlwaysElement);

    const closeAllElement = document.createElement('p');
    closeAllElement.classList.add('action');
    closeAllElement.innerHTML = `Don't show any tutorials.`;
    closeAllElement.onclick = () => this.closeAll();
    htmlElement.appendChild(closeAllElement);

    super(htmlElement);
    this.key = params.key;
  }

  private close(): void {
    tutorials.close(this.key);
    this.promise.resolve();
  }

  private closeAlways(): void {
    tutorials.closeAlways(this.key);
    this.promise.resolve();
  }

  private closeAll(): void {
    tutorials.closeAll();
    this.promise.resolve();
  }

  protected override waitClosed(): Promise<void> {
    return this.promise;
  }
}
