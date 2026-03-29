import { CardFace } from './card-face.js';
import { Card } from './card.js';

/** A family that does not require feeding. */
export class SelfSufficientFamily extends Card {
  public override readonly name = 'Self-Sufficient Family';

  public constructor() {
    super({ sides: [new FedFamily()] });
  }
}

class FedFamily extends CardFace {
  public constructor() {
    super({ image: 'fed-family.jpg', name: 'Self-Sufficient' });
  }
}
