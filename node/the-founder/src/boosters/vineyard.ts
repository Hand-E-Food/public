import { Discontent, Family, PositiveCard, Wine } from '../cards/index.js';
import { Item } from '../item.js';
import { formatQuantities, Resource } from '../resource.js';
import { game } from '../singleton/index.js';
import { type BoosterItemGroup, BoosterPack, OpenBoosterPackAction } from './booster-pack.js';

export class Vineyard extends BoosterPack {
  public constructor() {
    const cost = { [Resource.Wood]: 6 };
    super({
      image: 'vineyard.png',
      name: 'Vineyard',
      flavourText:
        '<p><i>What is work without reward. The vineyard houses a productive family with a focus on making wine.' +
        '</i></p>',
      actions: [
        new OpenBoosterPackAction({
          text: `Pay ${formatQuantities(cost)}. Build a vineyard.`,
          cost,
        }),
      ],
    });
  }

  protected override createItems(): BoosterItemGroup[] {
    return [
      {
        container: game.containers.positiveStack,
        items: [new PositiveCard({ image: 'vineyard.png', name: 'Vineyard' })],
      },
      {
        container: game.containers.negativeStack,
        items: [new Family()],
      },
      {
        container: game.containers.discardPile,
        items: [...Item.multiple(game.resourcesPerBooster, () => new Wine()), new Discontent()],
      },
    ];
  }
}
