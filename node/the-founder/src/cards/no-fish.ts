import { type CardDrawnProperties, GameEvent } from '../events/index.js';
import { eventHub, game, type GameEventListener } from '../singleton/index.js';
import { Card } from './card.js';
import { CardBack } from './card-back.js';
import { CardFace } from './card-face.js';

export class NoFish extends Card {
  public override name: string = 'No Fish';

  public constructor() {
    const face = new NoFishFace();
    super({
      sides: [face, CardBack.town()],
    });
    (face as any).card = this;
  }
}

class NoFishFace extends CardFace {
  private readonly card!: Card;
  private readonly listeners: GameEventListener[];

  public constructor() {
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
