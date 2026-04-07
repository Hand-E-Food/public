import { Card } from './card.js';
import { CardFace } from './card-face.js';

export class YourFamily extends Card {
  public override readonly name = 'Your Family';

  public constructor() {
    super({ sides: [new YourFamilyFace()] });
  }
}

class YourFamilyFace extends CardFace {
  public constructor() {
    super({
      canInspect: true,
      name: 'Your Family',
      image: 'fed-family.jpg',
      flavourText:
        '<p><i>Your beloved family. As mayor, your needs are catered for and your family is always happy to ' +
        'contribute to the community.</i></p>' +
        '<p>At the start of each year, draw a card.</p>',
      actions: [],
    });
  }
}
