import { Family, Fish, PositiveCard, SelfSufficientFamily, Wood } from '../cards/index.js';
import { BoosterPack, type BoosterItemGroup } from './booster-pack.js';
import { Item } from '../item.js';
import { game } from '../game.js';

export class FoundTown extends BoosterPack {
  public constructor() {
    super({ image: 'found-town.jpg', name: 'Found Your Town' });
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
        container: game.containers.fedFamilyStack,
        items: [...Item.multiple(3, () => new SelfSufficientFamily()), ...Item.multiple(2, FoundTown.createFedFamily)],
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

  private static createFedFamily(): Family {
    const family = new Family();
    family.flip();
    return family;
  }
}
