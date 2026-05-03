import { Card } from '../../cards/common/index.js';
import type { Container } from '../../container.js';
import { Item, type ItemAction, type ItemActionState, type ItemSide } from '../../item.js';
import { formatQuantities, type PayQuantities } from '../../resource.js';
import { game } from '../../singleton/index.js';
import { OpenBoosterPack } from '../../states/index.js';

export interface BoosterPackParams {
  /** This booster pack's image filename. */
  readonly image: string;

  /** This booster pack's name. */
  readonly name: string;

  /** This booster pack's flavour text as HTML. Include `<p>` tags. */
  readonly flavourText: string;

  /** The cost to open this booster pack. */
  readonly cost: PayQuantities;

  /** This booster pack's action text that will open the booster pack. Exclude the cost. */
  readonly actionText: string;
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
    this.actions = [new OpenBoosterPackAction(this, params.actionText, params.cost)];
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

class OpenBoosterPackAction implements ItemAction {
  public constructor(
    private readonly boosterPack: BoosterPack,
    private readonly actionText: string,
    private readonly cost: PayQuantities,
  ) {}

  private get hasZeroCost(): boolean {
    return Object.values(this.cost).every((cost) => cost === 0);
  }
  get state(): ItemActionState {
    return game.resources.has(this.cost) ? 'enabled' : 'disabled';
  }
  get text(): string {
    if (this.hasZeroCost) return this.actionText;
    else return `Pay ${formatQuantities(this.cost)}. ${this.actionText}`;
  }

  async execute(): Promise<void> {
    game.resources.spend(this.cost);
    await new OpenBoosterPack(this.boosterPack as BoosterPack).execute();
  }
}
