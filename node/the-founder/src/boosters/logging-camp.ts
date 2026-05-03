import { Discontent, Family, PositiveCard, Wood } from '../cards/index.js';
import { Item } from '../item.js';
import { type PayQuantities, Resource } from '../resource.js';
import { game } from '../singleton/index.js';
import { type BoosterItemGroup, BoosterPack } from './common/booster-pack.js';

export class LoggingCamp extends BoosterPack {
  public constructor(private readonly index: number) {
    const cost: PayQuantities = {
      [Resource.Wood]: 5 + index,
      [Resource.Stone]: 0 + index,
      [Resource.Luxury]: 0 + Math.floor(index / 2),
    };
    super({
      image: 'logging-camp.png',
      name: 'Logging Camp',
      flavourText:
        '<p><i>As the population grows, so does the need for wood. The logging camp houses another productive family ' +
        'with a focus on producing wood.</i></p>',
      actionText: 'Build a logging camp.',
      cost,
    });
  }

  protected override createItems(): BoosterItemGroup[] {
    return [
      {
        container: game.containers.positiveStack,
        items: [new PositiveCard({ image: 'logging-camp.png', name: 'Logging Camp' })],
      },
      {
        container: game.containers.negativeStack,
        items: [new Family()],
      },
      {
        container: game.containers.discardPile,
        items: [...Item.multiple(game.resourcesPerBooster, () => new Wood()), new Discontent()],
      },
      {
        container: game.containers.boosterPacks,
        items: [new LoggingCamp(this.index + 1)],
      },
    ];
  }
}
