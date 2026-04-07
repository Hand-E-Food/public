import { tutorials } from '../singleton/index.js';
import { ManualPromise } from './manual-promise.js';
import { Modal } from './modal.js';

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
    const modal = document.createElement('div');
    modal.classList.add('modal', 'text-panel', 'tutorial');
    if (params.left) modal.style.left = params.left;
    if (params.width) modal.style.width = params.width;
    if (params.right) modal.style.right = params.right;
    if (params.top) modal.style.top = params.top;
    if (params.height) modal.style.height = params.height;
    if (params.bottom) modal.style.bottom = params.bottom;
    for (const paragraph of params.paragraphs) {
      const p = document.createElement('p');
      p.innerHTML = paragraph;
      modal.appendChild(p);
    }

    function createButton(text: string, onclick: () => void): void {
      const button = document.createElement('p');
      button.classList.add('action');
      button.innerHTML = text;
      button.onclick = onclick;
      modal.appendChild(button);
    }
    createButton(`Close`, () => this.close());
    createButton(`Don't show this again.`, () => this.closeAlways());
    createButton(`Don't show any tutorials.`, () => this.closeAll());

    super(modal);
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
