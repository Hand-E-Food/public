import { Item } from '../../item.js';
import type { CardSide } from './card-side.js';

export abstract class Card extends Item {
  /** A card's standard height. */
  public static readonly height = 210;
  /** A card's standard width. */
  public static readonly width = 150;

  /**
   * Creates a new card.
   * @param sides This card's one or two sides.
   */
  protected constructor() {
    super();
    this.flipDiv = document.createElement('div');
    this.htmlElement.classList.add('item', 'card');
    this.htmlElement.appendChild(this.flipDiv);
  }

  public override get activeSide() {
    return this._activeSide;
  }
  private _activeSide!: CardSide;
  private readonly flipDiv: HTMLDivElement;
  private flipPromise: Promise<void> = Promise.resolve();
  private visibleSide!: CardSide;
  public override readonly height = Card.height;
  public override readonly width = Card.width;

  /** Sets the initial side to display. This must be set exactly once in the concrete constructor. */
  protected set initialSide(side: CardSide) {
    if (this._activeSide) throw new Error('initialSide must be set exactly one.');
    this._activeSide = side;
    this.flipDiv.appendChild(side.htmlElement);
    this.visibleSide = side;
  }

  /**
   * Animates flipping this card to the specified side.
   * @param side The side to flip to. If this side is already face up, this is ignored.
   */
  protected async flipTo(side: CardSide): Promise<void> {
    // If the new side is already the target side, abort.
    if (side === this._activeSide) return;
    // Set that the new side is the ultimate side to flip to, despite in-progress animations.
    this._activeSide = side;
    // Wait while a previous flip is being animated.
    while (this.htmlElement.childElementCount > 1) await this.flipPromise;
    // If the new side is no longer the target, abort.
    if (side !== this._activeSide) return;
    // If the new side is already visible, abort because the same side cannot be added twice.
    if (side === this.visibleSide) return;
    // Add the new side to the card.
    this.flipDiv.appendChild(side.htmlElement);
    this.visibleSide = side;
    // If this is the card's first side, skip animation.
    if (this.flipDiv.childElementCount === 1) return;
    // Animate flipping the card.
    this.flipPromise = this.flip();
    await this.flipPromise;
  }

  /** Animates flipping the card to its second side, and removes the first side. */
  private async flip(): Promise<void> {
    await this.flipDiv.animate([{ transform: 'rotateY(180deg)' }], { duration: this.animationDuration }).finished;
    this.flipDiv.childNodes[0]!.remove();
  }
}
