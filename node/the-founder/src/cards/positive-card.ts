import { Card, CardFace } from './common/index.js';

export type PositiveCardParams = {
  /** This card's name. */
  readonly name: string;

  /** This card's image filename. */
  readonly image: string;
};

/** A card that adds positive morale. */
export class PositiveCard extends Card {
  public override name: string;

  public constructor(params: PositiveCardParams) {
    super();
    this.name = params.name;
    this.initialSide = new PositiveCardFace(params);
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
