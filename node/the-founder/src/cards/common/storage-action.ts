import type { DeckCard } from './deck-card.js';
import type { ItemAction, ItemActionState } from '../../item.js';
import { game } from '../../singleton/index.js';

export class StorageAction implements ItemAction {
  public constructor(private readonly card: DeckCard) {}

  get state(): ItemActionState {
    if (game.containers.storageSpace.items.length === 0) {
      return 'hidden';
    } else if (this.card.container === game.containers.storageItems) {
      return 'enabled';
    } else if (game.containers.storageItems.canStore(this.card)) {
      return 'enabled';
    } else {
      return 'disabled';
    }
  }
  get text(): string {
    if (this.card.container === game.containers.storageItems) {
      return `Discard from storage.`;
    } else {
      return `Move to storage.`;
    }
  }

  async execute(): Promise<void> {
    if (this.card.container === game.containers.hand) {
      game.containers.storageItems.addItems(this.card);
    } else if (this.card.container === game.containers.storageItems) {
      game.containers.discardPile.addItems(this.card);
    }
  }
}
