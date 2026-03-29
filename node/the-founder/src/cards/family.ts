import { Card } from './card.js';
import { CardFace } from './card-face.js';
import { NegativeCardFace } from './negative-card-face.js';

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
}

class FedFamily extends CardFace {
  public constructor() {
    super({ image: 'fed-family.jpg', name: 'Fed Family' });
  }
}
