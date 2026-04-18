import { Family, Fish, PositiveCard, SelfSufficientFamily, Wood, YourFamily } from '../cards/index.js';
import { Item, type ItemAction } from '../item.js';
import { game } from '../singleton/index.js';
import { OpenBoosterPack } from '../states/open-booster-pack.js';
import { type BoosterItemGroup, BoosterPack } from './booster-pack.js';

export class FoundTown extends BoosterPack {
  public constructor() {
    super({
      image: 'found-town.jpg',
      name: 'Found Your Town',
      flavourText:
        '<p><i>Your caravan has trundled across the landscape for weeks. You arrive at a land bordered by rich ' +
        'mountains, fresh water, fertile soil, and generous woodlands. This is your promised land.</i></p>',
      actions: [new FoundTownAction()],
    });
  }

  protected override createItems(): BoosterItemGroup[] {
    return [
      {
        container: game.containers.positiveStack,
        items: [
          new PositiveCard({
            name: 'Town Square',
            image: 'town-square.avif',
          }),
          new PositiveCard({
            name: 'Fishery',
            image: 'fishery.jpg',
          }),
          new PositiveCard({
            name: 'Logger',
            image: 'logger.jpg',
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
    ];
  }

  private static createFedFamily(): Family {
    const family = new Family();
    family.flip();
    return family;
  }
}

class FoundTownAction implements ItemAction {
  readonly state = 'enabled';
  readonly text = 'Found a new town.';

  public async execute(item: Item): Promise<void> {
    await new OpenBoosterPack(item as BoosterPack).execute();
  }
}
