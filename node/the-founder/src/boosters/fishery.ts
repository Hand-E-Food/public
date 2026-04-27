import { Discontent, Family, Fish, PositiveCard } from '../cards/index.js';
import { NoFish } from '../cards/no-fish.js';
import { Item } from '../item.js';
import { formatQuantities, Resource } from '../resource.js';
import { game } from '../singleton/index.js';
import { type BoosterItemGroup, BoosterPack, OpenBoosterPackAction } from './booster-pack.js';

export class Fishery extends BoosterPack {
  public constructor() {
    const cost = { [Resource.Wood]: 7, [Resource.Stone]: 1 };
    super({
      image: 'fishery.png',
      name: 'Fishery',
      flavourText:
        '<p><i>The river is full of fish. Another fishery will house a productive family focused on catching ' +
        'fish.</i></p>',
      actions: [
        new OpenBoosterPackAction({
          text: `Pay ${formatQuantities(cost)}. Build a fishery.`,
          cost,
        }),
      ],
    });
  }

  protected override createItems(): BoosterItemGroup[] {
    const fish = Math.floor(game.resourcesPerBooster / 2);
    const noFish = game.resourcesPerBooster - fish;

    return [
      {
        container: game.containers.positiveStack,
        items: [new PositiveCard({ image: 'fishery.png', name: 'Fishery' })],
      },
      {
        container: game.containers.negativeStack,
        items: [new Family()],
      },
      {
        container: game.containers.discardPile,
        items: [
          ...Item.multiple(fish, () => new Fish()),
          ...Item.multiple(noFish, () => new NoFish()),
          new Discontent(),
        ],
      },
    ];
  }
}
