import { StorageSpace as StorageSpaceCard } from '../cards/index.js';
import { formatQuantities, type PayQuantities, Resource } from '../resource.js';
import { game } from '../singleton/index.js';
import { type BoosterItemGroup, BoosterPack, OpenBoosterPackAction } from './booster-pack.js';

export class StorageSpace extends BoosterPack {
  public constructor(private readonly index: number) {
    if (index < 0) throw new Error('Storage index is out of range.');

    const cost = StorageSpace.calculateCost(index);
    super({
      image: 'storage.png',
      name: 'Storage',
      flavourText:
        '<p><i>Increase your storage space to retain cards from one year to the next.</i></p>' +
        '<p><i>Store excess food and drink for difficult years, materials required for larger projects.</i></p>',
      actions: [
        new OpenBoosterPackAction({
          text: `Pay ${formatQuantities(cost)}. Increase your storage capacity.`,
          cost,
        }),
      ],
    });
  }

  private static calculateCost(index: number): PayQuantities {
    const cost: PayQuantities = { [Resource.Wood]: 0, [Resource.Stone]: 0, [Resource.Luxury]: 0 };
    const increase: (keyof PayQuantities)[] = [
      Resource.Wood,
      Resource.Stone,
      Resource.Wood,
      Resource.Stone,
      Resource.Luxury,
    ];
    while (index >= 0) {
      const resource = increase[index % increase.length]!;
      cost[resource] += 2;
      index--;
    }
    return cost;
  }

  protected override createItems(): BoosterItemGroup[] {
    return [
      {
        container: game.containers.storageSpace,
        items: [new StorageSpaceCard()],
      },
      {
        container: game.containers.boosterPacks,
        items: [new StorageSpace(this.index + 1)],
      },
    ];
  }
}
