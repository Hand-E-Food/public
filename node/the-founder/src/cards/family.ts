import type { Item, ItemAction, ItemActionState } from '../item.js';
import { formatQuantities, type PayQuantities, Resource } from '../resource.js';
import { game } from '../singleton/game.js';
import { Card } from './card.js';
import { CardFace } from './card-face.js';
import { NegativeCardFace } from './negative-card-face.js';

export class Family extends Card {
  public override readonly name = 'Family';

  public constructor() {
    super({ sides: [new HungryFamily(), new FedFamily()] });
  }
}

class HungryFamily extends NegativeCardFace {
  public constructor() {
    super({
      canInspect: true,
      name: 'Hungry Family',
      image: 'family-hungry.png',
      flavourText: '<p><i>A hungry family will work to support themselves before helping the community.</i></p>',
      actions: [new FeedFamilyAction()],
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
  private cost: PayQuantities = { [Resource.Food]: 1 };

  get state(): ItemActionState {
    return game.resources.has(this.cost) ? 'enabled' : 'disabled';
  }

  get text(): string {
    return `Pay ${formatQuantities(this.cost)} and flip this card.`;
  }

  async execute(item: Item): Promise<void> {
    const card = item as Card;
    if (!game.resources.has(this.cost)) throw new Error('Not enough resources to feed this family.');
    game.resources.spend(this.cost);
    await Promise.all([card.flip(), game.containers.fedFamilyStack.addItems(card)]);
  }
}
