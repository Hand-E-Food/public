import { CardFace, type CardFaceParams } from "./card-face.js";

type NegativeCardFaceParams = Omit<CardFaceParams, 'icon'>;

export class NegativeCardFace extends CardFace {
  public override morale: number = -1;

  public constructor(params: NegativeCardFaceParams) {
    super({ ...params, icon: 'negative.svg' });
  }
}