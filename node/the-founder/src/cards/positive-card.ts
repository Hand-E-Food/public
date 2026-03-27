import { CardFace, type CardFaceParams } from './card-face.js';
import { Card } from './card.js';

type PositiveCardParams = Omit<CardFaceParams, 'icon'>;

export class PositiveCard extends Card {
  public override name: string;

  public constructor(params: PositiveCardParams) {
    super({ sides: [new PositiveCardFace(params)] });
    this.name = params.name;
  }
}

class PositiveCardFace extends CardFace {
  public override morale: number = 1;

  public constructor(params: PositiveCardParams) {
    super({ ...params, icon: 'positive.svg' });
  }
}
