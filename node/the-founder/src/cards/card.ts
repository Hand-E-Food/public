import { Item } from '../item.js';
import type { CardSide } from './card-side.js';

/** One card. */
export class Card extends Item {
  public static readonly height = 250;
  public static readonly width = 150;

  private _activeSide: number = 0;
  private readonly flipDiv: HTMLDivElement;
  private readonly sides: CardSide[];

  public override readonly height = Card.height;
  public override readonly width = Card.width;

  /**
   * Creates a new card.
   * @param sides This card's one or two sides.
   */
  public constructor(...sides: CardSide[]) {
    if (sides.length < 1 || sides.length > 2) throw new Error('A card must have one or two sides.');
    super();
    this.sides = sides;
    this.flipDiv = document.createElement('div');
    let first = true;
    for (const side of sides) {
      if (first) first = false;
      else side.htmlElement.classList.add('flipped');
      (side as any).card = this;
      this.flipDiv.appendChild(side.htmlElement);
    }
    this.htmlElement.classList.add('item', 'card');
    this.htmlElement.appendChild(this.flipDiv);
  }

  /** This card's face-up side. */
  public get activeSide(): CardSide {
    const side = this.sides[this._activeSide];
    if (!side) throw new Error('A non-existent side is active.');
    return side;
  }

  /** Flip this card to it's other side. */
  public flip(): void {
    if (this.sides.length === 1) throw new Error('Cannot flip this card.');
    const classList = this.flipDiv.classList;
    if (classList.contains('flipped')) {
      classList.remove('flipped');
    } else {
      classList.add('flipped');
    }
    this._activeSide = 1 - this._activeSide;
  }
}
