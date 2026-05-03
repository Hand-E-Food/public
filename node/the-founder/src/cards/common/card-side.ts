import type { ItemAction, ItemSide } from '../../item.js';

export interface CardSideParams {
  /**
   * True if this card side can be inspected.
   * @default true
   */
  readonly canInspect: boolean;

  /** This card side's name. */
  readonly name: string;

  /** This card side's background image. */
  readonly image: string;

  /** This card side's flavour text as HTML. Include `<p>` tags. */
  readonly flavourText: string;

  /** This card side's actions. */
  readonly actions: ItemAction[];
}

/** One side of a card. */
export class CardSide implements ItemSide {
  public readonly canInspect: boolean;
  public readonly flavourText: string;
  /** This card side's HTML element. */
  public readonly htmlElement: HTMLDivElement;
  public readonly image: string;
  public readonly name: string;
  public readonly actions: ItemAction[];

  /** This card side's effect on morale. */
  public morale: number = 0;

  /**
   * Creates a side of a card.
   * @param params This card side's parameters.
   */
  public constructor(params: CardSideParams) {
    this.canInspect = params.canInspect ?? true;
    this.name = params.name;
    this.image = params.image;
    this.flavourText = params.flavourText;
    this.actions = params.actions;

    this.htmlElement = document.createElement('div');
    this.htmlElement.classList.add('side');
    this.htmlElement.innerHTML = `<img src="assets/${params.image}" />`;
    this.htmlElement.onclick = (event) => this.onCardClicked(event);
  }

  /**
   * Triggered when this card side is clicked.
   * @param event This event details.
   */
  public onCardClicked(_event: MouseEvent): void {}
}
