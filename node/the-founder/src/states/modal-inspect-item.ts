import type { Item,ItemAction } from '../item.js';
import { ManualPromise } from './manual-promise.js';
import { Modal } from './modal.js';

export interface ModalInspectItemParams {
  readonly item: Item;
}

/** Displays the current year. */
export class ModalInspectItem extends Modal {
  private executedAction?: ItemAction;
  private readonly item: Item;
  private readonly promise = new ManualPromise<void>();

  public constructor(params: ModalInspectItemParams) {
    const side = params.item.activeSide;

    const modal = document.createElement('div');
    modal.classList.add('modal', 'inspect-item');
    super(modal);
    this.item = params.item;

    const img = document.createElement('img');
    img.src = `assets/${side.image}`;
    modal.appendChild(img);
    
    const textPanel = document.createElement('div');
    textPanel.classList.add('text-panel');
    textPanel.innerHTML = `<h2>${side.name}</h2>${side.flavourText}`;
    modal.appendChild(textPanel);

    for (const action of side.actions) {
      if (!action.isVisible) continue;
      const button = document.createElement('p');
      button.classList.add('action');
      button.innerHTML = action.text;
      if (action.isEnabled) button.onclick = () => this.onActionClicked(action);
      else button.classList.add('disabled');
      textPanel.appendChild(button);
    }

    const closeButton = document.createElement('span');
    closeButton.classList.add('action', 'close');
    closeButton.innerHTML = '⨉';
    closeButton.onclick = () => this.onCloseClicked();
    modal.appendChild(closeButton);
  }

  public override async execute(): Promise<void> {
    await super.execute();
    if (this.executedAction) await this.executedAction.execute(this.item);
  }

  protected override async waitClosed(): Promise<void> {
    await this.promise;
  }

  private onActionClicked(logic: ItemAction): void {
    this.executedAction = logic;
    this.promise.resolve();
  }

  private onCloseClicked(): void {
    this.promise.resolve();
  }
}
