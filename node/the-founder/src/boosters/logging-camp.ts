import { Discontent, Family, PositiveCard, Wood } from '../cards/index.js';
import { Item } from '../item.js';
import { formatQuantities, type PayQuantities, Resource } from '../resource.js';
import { game } from '../singleton/index.js';
import { type BoosterItemGroup, BoosterPack, OpenBoosterPackAction } from './booster-pack.js';

export class LoggingCamp extends BoosterPack {
  public constructor(private readonly index: number) {
    const cost: PayQuantities = LoggingCamp.calculateCost(index);
    super({
      image: 'logging-camp.png',
      name: 'Logging Camp',
      flavourText:
        '<p><i>As the population grows, so does the need for wood. The logging camp houses another productive family ' +
        'with a focus on producing wood.</i></p>',
      actions: [
        new OpenBoosterPackAction({
          text: `Pay ${formatQuantities(cost)}. Build a logging camp.`,
          cost,
        }),
      ],
    });
  }

  private static calculateCost(index: number): PayQuantities {
    return {
      [Resource.Wood]: 6 + index,
      [Resource.Stone]: 0 + index,
      [Resource.Luxury]: 0 + Math.floor(index / 2),
    };
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
