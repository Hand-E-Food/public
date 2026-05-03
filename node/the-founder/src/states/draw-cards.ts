import type { DeckCard } from '../cards/common/index.js';
import { Family } from '../cards/index.js';
import { GameEvent, type YearStartedProperties } from '../events/index.js';
import { eventHub, game } from '../singleton/index.js';
import { Animate } from './animate.js';

export class DrawCards {
  public async execute(_props: YearStartedProperties): Promise<void> {
    const fedFamilies = game.containers.fedFamilyStack;
    for (let i = fedFamilies.items.length - 1; i >= 0; i--) {
      await this.animateFamily(fedFamilies.items[i] as Family);
      await this.drawCard();
    }
  }

  private async animateFamily(family: Family) {
    const promises: Promise<void>[] = [Animate.glow(family.htmlElement, family.animationDuration)];
    if (family instanceof Family) promises.push(family.flipToHungry(), game.containers.negativeStack.addItems(family));
    await Promise.all(promises);
  }

  private async drawCard(): Promise<void> {
    if (game.containers.drawDeck.items.length === 0) await this.shuffleDiscardPile();
    const card = game.containers.drawDeck.items.pop() as DeckCard;
    if (!card) return;
    await Promise.all([card.flipUp(), game.containers.hand.addItems(card)]);
    await eventHub.invoke(GameEvent.CardDrawn, { stop: false, card });
  }

  private async shuffleDiscardPile(): Promise<void> {
    const cards = game.containers.discardPile.items as DeckCard[];
    if (cards.length === 0) return;

    for (let i = cards.length - 1; i > 0; i--) {
      const j = Math.floor(Math.random() * (i + 1));
      const temp = cards[i]!;
      cards[i] = cards[j]!;
      cards[j] = temp;
    }
    await Promise.all([...cards.map((card) => card.flipDown()), game.containers.drawDeck.addItems(...cards)]);
  }
}
