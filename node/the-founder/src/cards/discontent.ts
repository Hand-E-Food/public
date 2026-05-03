import { type CardDrawnProperties, GameEvent } from '../events/index.js';
import type { ItemAction, ItemActionState } from '../item.js';
import { formatQuantities, type PayQuantities, Resource } from '../resource.js';
import { eventHub, game, type GameEventListener } from '../singleton/index.js';
import { CardBack, CardSide, DeckCard, NegativeCardFace } from './common/index.js';

export class Discontent extends DeckCard {
  public constructor() {
    super();
    this.face = new DiscontentFace(this);
  }

  protected back = CardBack.town();
  protected face: CardSide;
  override readonly name = 'Discontent';
}

class DiscontentFace extends NegativeCardFace {
  public constructor(private readonly card: Discontent) {
    super({
      canInspect: true,
      name: 'Discontent',
      image: 'discontent.png',
      flavourText:
        '<p><i>Tensions are rising with the daily struggle to survive.</i></p>' +
        '<p>When you draw this card, immediately add it to the negative stack.</p>',
      actions: [new DiscontentAction(card)],
    });
    this.listeners.push(eventHub.add(GameEvent.CardDrawn, 50, (props) => this.onCardDrawn(props)));
  }

  private readonly listeners: GameEventListener[] = [];

  private async onCardDrawn(props: CardDrawnProperties): Promise<void> {
    if (props.card !== this.card) return;
    await game.containers.negativeStack.addItems(this.card);
  }

  public destroy(): void {
    eventHub.remove(...this.listeners);
  }
}

class DiscontentAction implements ItemAction {
  public constructor(private readonly card: Discontent) {}

  private readonly cost: PayQuantities = { [Resource.Luxury]: 1 };
  get state(): ItemActionState {
    return game.resources.has(this.cost) ? 'enabled' : 'disabled';
  }
  get text(): string {
    return `Pay ${formatQuantities(this.cost)}. Discard this card.`;
  }

  async execute(): Promise<void> {
    game.resources.spend(this.cost);
    await game.containers.discardPile.addItems(this.card);
  }
}
