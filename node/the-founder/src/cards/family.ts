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
    super({
      canInspect: true,
      name: 'Hungry Family',
      image: 'hungry-family.jpg',
      flavourText: '<p><i>A hungry family will work to support themselves before helping the community.</i></p>',
      actions: [],
    });
  }
}

class FedFamily extends CardFace {
  public constructor() {
    super({
      canInspect: true,
      name: 'Fed Family',
      image: 'fed-family.jpg',
      flavourText:
        '<p><i>A fed family will be productive and help the community grow.</i></p>' +
        '<p>At the start of the year, draw a card and flip this card.</p>',
      actions: [],
    });
  }
}
