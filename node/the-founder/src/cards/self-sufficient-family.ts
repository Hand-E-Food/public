import { Card, CardFace } from './common/index.js';

/** A family that does not require feeding. */
export class SelfSufficientFamily extends Card {
  public override readonly name = 'Self-Sufficient Family';

  public constructor() {
    super();
    this.initialSide = new CardFace({
      canInspect: true,
      name: 'Self-Sufficient Family',
      image: 'family-fed.png',
      flavourText:
        '<p><i>This family have secured their needs and are always happy to contribute to the community.</i></p>' +
        '<p>At the start of each year, draw a card.</p>',
      actions: [],
    });
  }
}
