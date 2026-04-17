import { CardSide, type CardSideParams } from './card-side.js';

export interface CardFaceParams extends CardSideParams {
  /** This card's corner icon. */
  readonly icon?: string;
}

/** A card side that has a name and actions. */
export class CardFace extends CardSide {
  /**
   * Creates a card side with a name and actions.
   * @param params This card face's parameters.
   */
  public constructor(params: CardFaceParams) {
    super(params);
    this.htmlElement.classList.add('card-face');
    let innerHtml = '<div class="title">';
    if (params.icon) innerHtml += `<img src="assets/icons/${params.icon}" />`;
    innerHtml += `<span>${params.name}</span></div>`;
    this.htmlElement.innerHTML += innerHtml;
  }
}
