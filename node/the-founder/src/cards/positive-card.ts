import { Card } from './card.js';
import { CardFace } from './card-face.js';

export type PositiveCardParams = {
  readonly name: string;

  /**  */
  readonly image: string;
}

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
    super({
      canInspect: false,
      flavourText: '',
      actions: [],
      icon: 'positive.svg',
      ...params,
    });
  }
}
