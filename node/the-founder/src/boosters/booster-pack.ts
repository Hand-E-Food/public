import { Card } from '../cards/index.js';
import type { Container } from '../containers/index.js';
import { Item, type ItemAction, type ItemActionState, type ItemSide } from '../item.js';
import type { PayQuantities } from '../resource.js';
import { game } from '../singleton/index.js';
import { OpenBoosterPack } from '../states/index.js';

export interface BoosterPackParams {
  readonly image: string;
  readonly name: string;
  /** This booster pack's flavour text as HTML. Include `<p>` tags. */
  readonly flavourText: string;
  readonly actions: ItemAction[];
}

/** A booster pack containing more items. */
export abstract class BoosterPack extends Item implements ItemSide {
  public static readonly height = Card.height + 20;
  public static readonly width = Card.width + 10;

  private isOpen: boolean = false;

  public readonly activeSide: ItemSide = this;
  public override readonly height = BoosterPack.height;
  public override readonly width = BoosterPack.width;
  public readonly canInspect = true;
  public override readonly name: string;
  public readonly image: string;
  public readonly flavourText: string;
  public readonly actions: ItemAction[];

  public constructor(params: BoosterPackParams) {
    super();
    this.name = params.name;
    this.image = params.image;
    this.flavourText = params.flavourText;
    this.actions = params.actions;
    this.htmlElement.classList.add('item', 'booster', 'side');
    this.htmlElement.innerHTML += `<img src="assets/${params.image}" />`;
    this.htmlElement.innerHTML += `<div class='title'><span>${params.name}</span></div>`;
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

export type OpenBoosterPackActionParams = {
  readonly text: string;
  readonly cost: PayQuantities;
};

export class OpenBoosterPackAction implements ItemAction {
  private readonly text: string;

  protected readonly cost: PayQuantities;

  public constructor(params: OpenBoosterPackActionParams) {
    this.cost = params.cost;
    this.text = params.text;
  }

  public getText(_item: Item): string {
    return this.text;
  }

  public getState(_item: Item): ItemActionState {
    return game.resources.has(this.cost) ? 'enabled' : 'disabled';
  }

  public async execute(item: Item): Promise<void> {
    game.resources.spend(this.cost);
    await new OpenBoosterPack(item as BoosterPack).execute();
  }
}
