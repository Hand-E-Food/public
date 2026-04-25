import type { Container } from '../containers/index.js';
import type { Item, ItemAction, ItemActionState } from '../item.js';
import { type ProduceQuantities, Resource, resourceImage } from '../resource.js';
import { game } from '../singleton/index.js';
import { Card } from './card.js';
import { CardBack } from './card-back.js';
import { CardFace } from './card-face.js';

type ResourceParams = {
  readonly image: string;
  readonly name: string;
  readonly flavourText: string;
  readonly produce: ProduceQuantities;
};

class ResourceCard extends Card {
  public override readonly name: string;

  public constructor(params: ResourceParams) {
    const face = new ResourceFace(params);
    super({
      sides: [face, CardBack.drawDeck()],
    });
    (face.actions[0] as any).item = this;
    this.name = params.name;
  }
}

class ResourceFace extends CardFace {
  public constructor(params: ResourceParams) {
    super({
      canInspect: true,
      actions: [new ProduceResourcesAction(params.produce)],
      ...params,
    });
  }
}

class ProduceResourcesAction implements ItemAction {
  private readonly item!: ResourceCard;

  get state(): ItemActionState {
    const validContainers: (Container | undefined)[] = [game.containers.hand];
    return validContainers.includes(this.item.container) ? 'enabled' : 'disabled';
  }

  text: string;

  public constructor(private readonly produce: ProduceQuantities) {
    let text = 'Produce';
    for (const [key, quantity] of Object.entries(produce)) {
      const resource = Number(key) as Resource;
      const img = resourceImage(resource, 'inline');
      text += ` ${quantity} ${img.outerHTML}`;
    }
    text += '.';
    this.text = text;
  }

  async execute(_item: Item): Promise<void> {
    for (const [key, quantity] of Object.entries(this.produce)) {
      const resource = Number(key) as Resource;
      game.resources.resources[resource].quantity += quantity;
    }
    await game.containers.discardPile.addItems(this.item);
  }
}

export class Crop extends ResourceCard {
  public constructor() {
    super({
      name: 'Crop',
      image: 'crop.png',
      flavourText: '',
      produce: { [Resource.Food]: 2 },
    });
  }
}

export class Fish extends ResourceCard {
  public constructor() {
    super({
      name: 'Fish',
      image: 'fish.png',
      flavourText: '',
      produce: { [Resource.Food]: 2 },
    });
  }
}

export class Livestock extends ResourceCard {
  public constructor() {
    super({
      name: 'Livestock',
      image: 'livestock.png',
      flavourText: '',
      produce: { [Resource.Food]: 4 },
    });
  }
}

export class Gold extends ResourceCard {
  public constructor() {
    super({
      name: 'Gold',
      image: 'gold.png',
      flavourText: '',
      produce: { [Resource.Luxury]: 2 },
    });
  }
}

export class Stone extends ResourceCard {
  public constructor() {
    super({
      name: 'Stone',
      image: 'stone.png',
      flavourText: '',
      produce: { [Resource.Stone]: 2 },
    });
  }
}

export class Wine extends ResourceCard {
  public constructor() {
    super({
      name: 'Wine',
      image: 'wine.png',
      flavourText: '',
      produce: { [Resource.Luxury]: 2 },
    });
  }
}

export class Wood extends ResourceCard {
  public constructor() {
    super({
      name: 'Wood',
      image: 'wood.png',
      flavourText: '',
      produce: { [Resource.Wood]: 2 },
    });
  }
}
