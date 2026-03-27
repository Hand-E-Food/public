import { NegativeCardFace } from './negative-card-face.js';
import { Card } from './card.js';

export class Discontent extends Card {
  public override readonly name = 'Discontent';

  public constructor() {
    super({ sides: [new DiscontentFace()] });
  }
}

class DiscontentFace extends NegativeCardFace {
  public constructor() {
    super({ image: 'discontent.jpg', name: 'Discontent' });
  }
}
