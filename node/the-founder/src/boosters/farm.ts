import { Crop, Discontent, Family, PositiveCard } from '../cards/index.js';
import { Item } from '../item.js';
import { type PayQuantities, Resource } from '../resource.js';
import { game } from '../singleton/index.js';
import { type BoosterItemGroup, BoosterPack } from './common/booster-pack.js';

export class Farm extends BoosterPack {
  public constructor(private readonly index: number) {
    const cost: PayQuantities = Farm.calculateCost(index);
    super({
      image: 'farm.png',
      name: 'Farm',
      flavourText:
        '<p><i>As the population grows, so does the need for food. The farm houses another productive family with a ' +
        'focus on producing food.</i></p>',
      actionText: 'Build a farm.',
      cost,
    });
  }

  private static calculateCost(index: number): PayQuantities {
    return {
      [Resource.Wood]: 5 + index,
      [Resource.Stone]: 0 + index,
      [Resource.Luxury]: 0 + Math.floor(index / 2),
    };
  }

  protected override createItems(): BoosterItemGroup[] {
    return [
      {
        container: game.containers.positiveStack,
        items: [new PositiveCard({ image: 'farm.png', name: 'Farm' })],
      },
      {
        container: game.containers.negativeStack,
        items: [new Family()],
      },
      {
        container: game.containers.discardPile,
        items: [...Item.multiple(game.resourcesPerBooster, () => new Crop()), new Discontent()],
      },
      {
        container: game.containers.boosterPacks,
        items: [new Farm(this.index + 1)],
      },
    ];
  }
}
