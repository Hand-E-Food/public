import type { ItemAction, ItemActionState } from '../item.js';
import { formatQuantities, type PayQuantities, Resource } from '../resource.js';
import { game } from '../singleton/game.js';
import { Card, CardFace, NegativeCardFace } from './common/index.js';

export class Family extends Card {
  private readonly fed = new FedFamily();
  private readonly hungry: HungryFamily;
  public override readonly name = 'Family';

  public constructor(initialSide: 'hungry' | 'fed' = 'hungry') {
    super();
    this.hungry = new HungryFamily(this);
    this.initialSide = initialSide === 'hungry' ? this.hungry : this.fed;
  }

  public async flipToFed(): Promise<void> {
    await this.flipTo(this.fed);
  }

  public async flipToHungry(): Promise<void> {
    await this.flipTo(this.hungry);
  }
}

class HungryFamily extends NegativeCardFace {
  public constructor(card: Family) {
    super({
      canInspect: true,
      name: 'Hungry Family',
      image: 'family-hungry.png',
      flavourText: '<p><i>A hungry family will work to support themselves before helping the community.</i></p>',
      actions: [new FeedFamilyAction(card)],
    });
  }
}

class FedFamily extends CardFace {
  public constructor() {
    super({
      canInspect: true,
      name: 'Fed Family',
      image: 'family-fed.png',
      flavourText:
        '<p><i>A fed family will be productive and help the community grow.</i></p>' +
        '<p>At the start of the year, draw a card and flip this card.</p>',
      actions: [],
    });
  }
}

class FeedFamilyAction implements ItemAction {
  public constructor(private readonly card: Family) {}

  private cost: PayQuantities = { [Resource.Food]: 1 };
  get state(): ItemActionState {
    return game.resources.has(this.cost) ? 'enabled' : 'disabled';
  }
  get text(): string {
    return `Pay ${formatQuantities(this.cost)} and flip this card.`;
  }

  async execute(): Promise<void> {
    if (!game.resources.has(this.cost)) throw new Error('Not enough resources to feed this family.');
    game.resources.spend(this.cost);
    await Promise.all([this.card.flipToFed(), game.containers.fedFamilyStack.addItems(this.card)]);
  }
}
