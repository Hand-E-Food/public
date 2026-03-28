import type { Container } from '../containers/index.js';
import { Card } from '../cards/index.js';
import { Item } from '../item.js';

export interface BoosterPackParams {
  readonly image: string;
  readonly name: string;
}

/** A booster pack containing more items. */
export abstract class BoosterPack extends Item {
  public static readonly height = Card.height + 20;
  public static readonly width = Card.width + 10;

  private isOpen: boolean = false;

  public override readonly height = BoosterPack.height;
  public override readonly name: string;
  public override readonly width = BoosterPack.width;

  public constructor(params: BoosterPackParams) {
    super();
    this.name = params.name;
    this.htmlElement.classList.add('item', 'booster', 'side');
    this.htmlElement.innerHTML += `<img src="assets/${params.image}" /><span class='title'>${params.name}</span>`;
  }

  public open(): BoosterItemGroup[] {
    if (this.isOpen) throw new Error('Booster pack is already open.');
    this.isOpen = true;
    return this.createItems();
  }

  protected abstract createItems(): BoosterItemGroup[];
}

export type BoosterItemGroup = {
  readonly container: Container;
  readonly items: Item[];
};
