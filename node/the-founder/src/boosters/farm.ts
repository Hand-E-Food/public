import { Crop, Discontent, Family, PositiveCard } from '../cards/index.js';
import { Item } from '../item.js';
import { formatQuantities, type PayQuantities, Resource } from '../resource.js';
import { game } from '../singleton/index.js';
import { type BoosterItemGroup, BoosterPack, OpenBoosterPackAction } from './booster-pack.js';

export class Farm extends BoosterPack {
  public constructor() {
    const cost: PayQuantities = { [Resource.Wood]: 6 };
    super({
      image: 'farm.png',
      name: 'Farm',
      flavourText:
        '<p><i>As the population grows, so does the need for food. The farm houses another productive family with a ' +
        'focus on producing food.</i></p>',
      actions: [
        new OpenBoosterPackAction({
          text: `Pay ${formatQuantities(cost)}. Build a farm.`,
          cost,
        }),
      ],
    });
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
    ];
  }
}
