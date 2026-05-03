import { Discontent, Family, PositiveCard, Stone } from '../cards/index.js';
import { Item } from '../item.js';
import { Resource } from '../resource.js';
import { game } from '../singleton/index.js';
import { type BoosterItemGroup, BoosterPack } from './common/booster-pack.js';
import { Mine } from './mine.js';

export class Quarry extends BoosterPack {
  public constructor() {
    const cost = { [Resource.Wood]: 5 };
    super({
      image: 'quarry.png',
      name: 'Quarry',
      flavourText:
        '<p><i>Stone is sturdy and will allow you to construct better buildings. The quarry houses a productive ' +
        'family with a focus on mining stone.</i></p>',
      actionText: 'Build a quarry.',
      cost,
    });
  }

  protected override createItems(): BoosterItemGroup[] {
    return [
      {
        container: game.containers.positiveStack,
        items: [new PositiveCard({ image: 'quarry.png', name: 'Quarry' })],
      },
      {
        container: game.containers.negativeStack,
        items: [new Family()],
      },
      {
        container: game.containers.discardPile,
        items: [...Item.multiple(game.resourcesPerBooster, () => new Stone()), new Discontent()],
      },
      {
        container: game.containers.boosterPacks,
        items: [new Mine(0)],
      },
    ];
  }
}
