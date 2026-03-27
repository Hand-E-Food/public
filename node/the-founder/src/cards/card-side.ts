import type { Card } from "./card.js";

export interface CardSideParams {
    /** This card side's background image. */
    readonly image: string;
}

/** One side of a card. */
export class CardSide {
  /** The card this side belongs to. */
  protected readonly card!: Card;

  /** This card side's HTML element. */
  public readonly htmlElement: HTMLDivElement;

  /** This card side's effect on morale. */
  public morale: number = 0;

  /**
   * Creates a side of a card.
   * @param params This card side's parameters.
   */
  public constructor(params: CardSideParams) {
    this.htmlElement = document.createElement('div');
    this.htmlElement.classList.add('side');
    this.htmlElement.innerHTML = `<img src="assets/${params.image}" />`;
    this.htmlElement.onclick = (event) => this.onCardClicked(event);
  }

  /**
   * Triggered when this card side is clicked.
   * @param event This event details.
   */
  public onCardClicked(_event: MouseEvent): void { }
}
