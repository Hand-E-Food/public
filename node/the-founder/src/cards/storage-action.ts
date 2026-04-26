import type { Item, ItemAction, ItemActionState } from '../item.js';
import { game } from '../singleton/index.js';
import type { Card } from './card.js';

export class StorageAction implements ItemAction {
  public getText(item: Item): string {
    if (item.container === game.containers.hand) {
      return `Move to storage.`;
    } else if (item.container === game.containers.storageItems) {
      return `Discard from storage.`;
    } else {
      return '';
    }
  }

  public getState(item: Item): ItemActionState {
    if (game.containers.storageSpace.items.length === 0) {
      return 'hidden';
    } else if (item.container === game.containers.storageItems) {
      return 'enabled';
    } else if (game.containers.storageItems.canStore(item as Card)) {
      return 'enabled';
    } else {
      return 'disabled';
    }
  }

  public async execute(item: Item): Promise<void> {
    if (item.container === game.containers.hand) {
      game.containers.storageItems.addItems(item);
    } else if (item.container === game.containers.storageItems) {
      game.containers.discardPile.addItems(item);
    }
  }
}
