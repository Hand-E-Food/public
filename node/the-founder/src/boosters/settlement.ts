import { BoosterPack, type BoosterItemGroup } from './booster-pack.js';
import { Family, Fish, PositiveCard, Wood } from '../cards/index.js';
import { Item } from '../item.js';
import { game } from '../game.js';

export class Settlement extends BoosterPack {
  public constructor() {
    super({ image: 'settlement.jpg', name: 'Settlement' });
  }

  protected override createItems(): BoosterItemGroup[] {
    return [
      {
        container: game.containers.positiveStack,
        items: [
          new PositiveCard({ name: 'Town Square', image: 'town-square.avif' }),
          new PositiveCard({ name: 'Fishery', image: 'fishery.jpg' }),
          new PositiveCard({ name: 'Logger', image: 'logger.jpg' }),
        ],
      },
      {
        container: game.containers.negativeStack,
        items: [...Item.multiple(3, () => new Family())],
      },
      {
        container: game.containers.discardPile,
        items: [
          ...Item.multiple(game.resourcesPerBooster, () => new Fish()),
          ...Item.multiple(game.resourcesPerBooster, () => new Wood()),
        ],
      },
    ];
  }
}
