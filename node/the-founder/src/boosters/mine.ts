import { Discontent, Family, Gold, PositiveCard, Stone } from '../cards/index.js';
import { Item } from '../item.js';
import { formatQuantities, type PayQuantities, Resource } from '../resource.js';
import { game } from '../singleton/index.js';
import { type BoosterItemGroup, BoosterPack, OpenBoosterPackAction } from './booster-pack.js';

type Version = { cost: PayQuantities; cards: { stone: number; gold: number; imps: number } };

export class Mine extends BoosterPack {
  private static versions: Version[] = [
    {
      cost: { [Resource.Wood]: 4, [Resource.Stone]: 4, [Resource.Luxury]: 0 },
      cards: { stone: 3, gold: 1, imps: 0 },
    },
    {
      cost: { [Resource.Wood]: 5, [Resource.Stone]: 4, [Resource.Luxury]: 0 },
      cards: { stone: 2, gold: 2, imps: 0 },
    },
    {
      cost: { [Resource.Wood]: 5, [Resource.Stone]: 5, [Resource.Luxury]: 0 },
      cards: { stone: 1, gold: 3, imps: 0 },
    },
    {
      cost: { [Resource.Wood]: 5, [Resource.Stone]: 5, [Resource.Luxury]: 1 },
      cards: { stone: 0, gold: 3, imps: 1 },
    },
    {
      cost: { [Resource.Wood]: 6, [Resource.Stone]: 5, [Resource.Luxury]: 1 },
      cards: { stone: 0, gold: 2, imps: 2 },
    },
    {
      cost: { [Resource.Wood]: 6, [Resource.Stone]: 6, [Resource.Luxury]: 1 },
      cards: { stone: 0, gold: 1, imps: 3 },
    },
    {
      cost: { [Resource.Wood]: 6, [Resource.Stone]: 6, [Resource.Luxury]: 2 },
      cards: { stone: 0, gold: 0, imps: 4 },
    },
  ];

  private readonly version: Version & { index: number };

  public constructor(index: number) {
    if (!Mine.versions[index]) throw new Error('Mine index is out of range.');
    const version = { index, ...Mine.versions[index] };
    const cost = version.cost;
    super({
      image: 'mine.png',
      name: 'Mine',
      flavourText:
        '<p><i>To harvest more stone, you will need to dig into the mountain. What else will you find underneath?' +
        '</i></p>',
      actions: [
        new OpenBoosterPackAction({
          text: `Pay ${formatQuantities(cost)}. Dig a mine.`,
          cost,
        }),
      ],
    });
    this.version = version;
  }

  protected override createItems(): BoosterItemGroup[] {
    const groups: BoosterItemGroup[] = [
      {
        container: game.containers.positiveStack,
        items: [new PositiveCard({ image: 'mine.png', name: 'Mine' })],
      },
      {
        container: game.containers.negativeStack,
        items: [new Family()],
      },
      {
        container: game.containers.discardPile,
        items: [
          ...Item.multiple(this.version.cards.stone, () => new Stone()),
          ...Item.multiple(this.version.cards.gold, () => new Gold()),
          new Discontent(),
          //...Item.multiple(this.version.cards.imps, () => new Imp()),
        ],
      },
    ];

    if (Mine.versions[this.version.index + 1]) {
      groups.push({
        container: game.containers.boosterPacks,
        items: [new Mine(this.version.index + 1)],
      });
    }

    return groups;
  }
}
