import { Card } from './card.js';
import { CardFace } from './card-face.js';

/** A family that does not require feeding. */
export class SelfSufficientFamily extends Card {
  public override readonly name = 'Self-Sufficient Family';

  public constructor() {
    super({ sides: [new FedFamily()] });
  }
}

class FedFamily extends CardFace {
  public constructor() {
    super({
      canInspect: true,
      name: 'Self-Sufficient Family',
      image: 'fed-family.jpg',
      flavourText:
        '<p><i>This family have secured their needs and are always happy to contribute to the community.</i></p>' +
        '<p>At the start of each year, draw a card.</p>',
      actions: [],
    });
  }
}
