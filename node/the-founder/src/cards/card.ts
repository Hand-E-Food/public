import { Item } from '../item.js';
import type { CardSide } from './card-side.js';

export interface CardParams {
  readonly sides: CardSide[];
}

/** One card. */
export abstract class Card extends Item {
  public static readonly height = 210;
  public static readonly width = 150;

  private _activeSide: number = 0;
  private readonly flipDiv: HTMLDivElement;
  private isFlipped: boolean = false;
  private readonly sides: CardSide[];

  public override readonly height = Card.height;
  public override readonly width = Card.width;

  /**
   * Creates a new card.
   * @param sides This card's one or two sides.
   */
  protected constructor(params: CardParams) {
    if (![1, 2].includes(params.sides.length)) throw new Error('A card must have one or two sides.');
    super();
    this.sides = params.sides;
    this.flipDiv = document.createElement('div');
    let first = true;
    for (const side of this.sides) {
      if (first) first = false;
      else side.htmlElement.classList.add('reverse');
      (side as any).item = this;
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

  /**
   * Flip this card to it's other side.
   *
   * Note: Flip cards before attempting to move them. For example:
   * ```
   * await Promise.all([card.flip, container.addItems(card)])
   * ```
   */
  public async flip(): Promise<void> {
    if (this.sides.length === 1) throw new Error('Cannot flip this card.');
    this._activeSide = 1 - this._activeSide;
    this.isFlipped = !this.isFlipped;
    await this.flipDiv.animate([{ transform: 'rotateY(0deg)' }, { transform: 'rotateY(180deg)' }], {
      direction: this.isFlipped ? 'normal' : 'reverse',
      duration: this.animationDuration,
      fill: 'forwards',
    }).finished;
  }
}
