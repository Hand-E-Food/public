import { Modal } from './modal.js';

export class ModalTitleScreen extends Modal {
  public constructor() {
    const htmlElement = document.createElement('div');
    htmlElement.classList.add('title-screen');
    htmlElement.innerHTML =
      '<h1>The Founder</h1><footer>Copyright &copy; 2026 Mark Richardson, All rights reserved.</footer>';

    super(htmlElement);
  }
}
