import { type CardDrawnProperties, GameEvent } from '../events/index.js';
import { eventHub, game, type GameEventListener } from '../singleton/index.js';
import { CardBack, CardFace, CardSide, DeckCard } from './common/index.js';

/** A special resource card that produces nothing. */
export class NoFish extends DeckCard {
  protected override back = CardBack.town();
  protected override face: CardSide;
  public override name = 'No Fish';

  public constructor() {
    super();
    this.face = new NoFishFace(this);
    this.initialSide = this.face;
  }
}

class NoFishFace extends CardFace {
  private readonly listeners: GameEventListener[];

  public constructor(private readonly card: NoFish) {
    super({
      canInspect: true,
      image: 'no-fish.png',
      name: 'No Fish',
      flavourText:
        '<p><i>Despite their best efforts, the fishermen have been unable to catch any fish.</i></p>' +
        '<p>When you draw this card, immediately discard it.</p>',
      actions: [],
    });
    this.listeners = [eventHub.add(GameEvent.CardDrawn, 10, (props) => this.onCardDrawn(props))];
  }

  private async onCardDrawn(props: CardDrawnProperties): Promise<void> {
    if (props.card !== this.card) return;
    game.containers.discardPile.addItems(props.card);
  }

  public destroy(): void {
    eventHub.remove(...this.listeners);
    this.listeners.splice(0, this.listeners.length);
  }
}
