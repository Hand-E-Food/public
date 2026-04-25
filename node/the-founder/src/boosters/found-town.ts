import { Family, Fish, PositiveCard, SelfSufficientFamily, Wood, YourFamily } from '../cards/index.js';
import { Item } from '../item.js';
import { game } from '../singleton/index.js';
import { type BoosterItemGroup, BoosterPack, OpenBoosterPackAction } from './booster-pack.js';
import { Farm } from './farm.js';
import { Quarry } from './quarry.js';
import { Vineyard } from './vineyard.js';

export class FoundTown extends BoosterPack {
  public constructor() {
    super({
      image: 'found-town.png',
      name: 'Found Your Town',
      flavourText:
        '<p><i>Your caravan has trundled across the landscape for weeks. You arrive at a land bordered by rich ' +
        'mountains, fresh water, fertile soil, and generous woodlands. This is your promised land.</i></p>',
      actions: [
        new OpenBoosterPackAction({
          text: 'Found a new town.',
          cost: {},
        }),
      ],
    });
  }

  protected override createItems(): BoosterItemGroup[] {
    return [
      {
        container: game.containers.positiveStack,
        items: [
          new PositiveCard({
            name: 'Town Square',
            image: 'town-square.png',
          }),
          new PositiveCard({
            name: 'Fishery',
            image: 'fishery.png',
          }),
          new PositiveCard({
            name: 'Logging Camp',
            image: 'logging-camp.png',
          }),
        ],
      },
      {
        container: game.containers.fedFamilyStack,
        items: [new YourFamily(), new SelfSufficientFamily(), ...Item.multiple(2, FoundTown.createFedFamily)],
      },
      {
        container: game.containers.discardPile,
        items: [
          ...Item.multiple(game.resourcesPerBooster, () => new Fish()),
          ...Item.multiple(game.resourcesPerBooster, () => new Wood()),
        ],
      },
      {
        container: game.containers.boosterPacks,
        items: [new Farm(), new Quarry(), new Vineyard()],
      },
    ];
  }

  private static createFedFamily(): Family {
    const family = new Family();
    family.flip();
    return family;
  }
}
