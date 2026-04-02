import { type Card, Family } from '../cards/index.js';
import { eventHub, game, GameEvent, stateMachine } from '../singleton/index.js';
import { FlipCards, type GameState, NoOp, Sequence } from './primitive/index.js';

export class DrawCards implements GameState {
  public readonly name: string = 'DrawCards';

  enter(): void {
    const states: GameState[] = [];
    const fedFamilies = game.containers.fedFamilyStack;
    const negativeStack = game.containers.negativeStack;
    for (let i = fedFamilies.items.length - 1; i >= 0; i--) {
      const family = fedFamilies.items[i] as Card;
      if (family instanceof Family) states.push(new FlipCards(negativeStack, family));
      states.push(new DrawCard());
    }
    stateMachine.next(new Sequence(states));
  }
}

class DrawCard implements GameState {
  public readonly name: string = 'DrawCard';

  enter(): void {
    stateMachine.push(game.containers.drawDeck.items.length === 0 ? new Shuffle() : new NoOp());
  }

  resume(): void {
    const card = game.containers.drawDeck.items[0] as Card;
    if (card) {
      stateMachine.next(
        new Sequence([new FlipCards(game.containers.hand, card), eventHub.invoke(GameEvent.CardDrawn, card)]),
      );
    } else {
      stateMachine.pop();
    }
  }
}

class Shuffle implements GameState {
  public readonly name: string = 'Shuffle';

  enter(): void {
    const cards = game.containers.discardPile.items as Card[];
    if (cards.length === 0) {
      stateMachine.pop();
    } else {
      this.shuffle(cards);
      stateMachine.next(new FlipCards(game.containers.drawDeck, ...cards));
    }
  }

  private shuffle(cards: Card[]): void {
    for (let i = cards.length - 1; i > 0; i--) {
      const j = Math.floor(Math.random() * (i + 1));
      const temp = cards[i]!;
      cards[i] = cards[j]!;
      cards[j] = temp;
    }
  }
}
