import { type CardDrawnProperties, GameEvent } from '../events/index.js';
import type { Item, ItemAction, ItemActionState } from '../item.js';
import { formatQuantities, type PayQuantities, Resource } from '../resource.js';
import { eventHub, type GameEventListener } from '../singleton/event-hub.js';
import { game } from '../singleton/index.js';
import { Card } from './card.js';
import { CardBack } from './card-back.js';
import { NegativeCardFace } from './negative-card-face.js';

export class Discontent extends Card {
  public override readonly name = 'Discontent';

  public constructor() {
    const face = new DiscontentFace();
    super({ sides: [face, CardBack.town()] });
    (face as any).card = this;
  }
}

class DiscontentFace extends NegativeCardFace {
  private readonly card!: Card;
  private readonly listeners: GameEventListener[] = [];

  public constructor() {
    super({
      canInspect: true,
      name: 'Discontent',
      image: 'discontent.png',
      flavourText: '<p><i>Tensions are rising with the daily struggle to survive.</i></p>',
      actions: [new DiscontentAction()],
    });
    this.listeners.push(eventHub.add(GameEvent.CardDrawn, 50, (props) => this.onCardDrawn(props)));
  }

  private async onCardDrawn(props: CardDrawnProperties): Promise<void> {
    if (props.card !== this.card) return;
    await game.containers.negativeStack.addItems(this.card);
  }

  public destroy(): void {
    eventHub.remove(...this.listeners);
  }
}

class DiscontentAction implements ItemAction {
  private readonly cost: PayQuantities = { [Resource.Luxury]: 1 };

  public getText(_item: Item): string {
    return `Pay ${formatQuantities(this.cost)}. Discard this card.`;
  }

  public getState(_item: Item): ItemActionState {
    return game.resources.has(this.cost) ? 'enabled' : 'disabled';
  }

  public async execute(item: Item): Promise<void> {
    game.resources.spend(this.cost);
    await game.containers.discardPile.addItems(item);
  }
}
