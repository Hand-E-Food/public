import { NegativeCardFace } from './negative-card-face.js';
import { CardFace } from './card-face.js';
import { game } from '../game.js';
import { Card } from './card.js';

export class Family extends Card {
  public override readonly name = 'Family';

  public constructor() {
    super({ sides: [new HungryFamily(), new FedFamily()] });
  }
}

class HungryFamily extends NegativeCardFace {
  public constructor() {
    super({ image: 'hungry-family.jpg', name: 'Hungry Family' });
  }

  public override onCardClicked(_event: MouseEvent): void {
    this.card.flip();
    game.containers.fedFamilyStack.addItems(this.card);
  }
}

class FedFamily extends CardFace {
  public constructor() {
    super({ image: 'fed-family.jpg', name: 'Fed Family' });
  }

  public override onCardClicked(_event: MouseEvent): void {
    this.card.flip();
    game.containers.negativeStack.addItems(this.card);
  }
}
