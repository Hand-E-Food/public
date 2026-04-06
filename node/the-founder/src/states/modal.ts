import { Primitive } from './primitive.js';

export class Modal {
  protected readonly fadeDuration: number = 500;

  public constructor(protected readonly htmlElement: HTMLElement) {}

  public async execute(): Promise<void> {
    await Primitive.fadeIn(this.htmlElement, this.fadeDuration);
    await this.waitClosed();
    await Primitive.fadeOut(this.htmlElement, this.fadeDuration);
  }

  protected waitClosed(): Promise<void> {
    return new Promise((resolve) => {
      this.htmlElement.onclick = () => resolve();
    });
  }
}
