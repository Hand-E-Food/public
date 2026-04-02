import { Card } from './card.js';
import { CardFace } from './card-face.js';
import { NegativeCardFace } from './negative-card-face.js';

export class YourFamily extends Card {
  public override readonly name = 'Your Family';

  public constructor() {
    super({ sides: [new YourHappyFamily(), new YourBetrayedFamily()] });
  }
}

class YourHappyFamily extends CardFace {
  public constructor() {
    super({ image: 'fed-family.jpg', name: 'Your Family' });
  }
}

class YourBetrayedFamily extends NegativeCardFace {
  public constructor() {
    super({ image: 'hungry-family.jpg', name: 'Your Family' });
  }
}
