import { Card } from './card.js';
import { NegativeCardFace } from './negative-card-face.js';

export class Discontent extends Card {
  public override readonly name = 'Discontent';

  public constructor() {
    super({ sides: [new DiscontentFace()] });
  }
}

class DiscontentFace extends NegativeCardFace {
  public constructor() {
    super({
      canInspect: true,
      name: 'Discontent',
      image: 'discontent.jpg',
      flavourText: '<p><i>"There must be more to life than work and struggle..."</i></p>',
      actions: [],
    });
  }
}
