import type { Container } from '../container.js';
import type { ItemAction, ItemActionState } from '../item.js';
import { formatQuantities, type ProduceQuantities, Resource } from '../resource.js';
import { game } from '../singleton/index.js';
import { CardBack, CardFace, CardSide, DeckCard, StorageAction } from './common/index.js';

type ResourceParams = {
  readonly image: string;
  readonly name: string;
  readonly flavourText: string;
  readonly produce: ProduceQuantities;
};

class ResourceCard extends DeckCard {
  protected override back = CardBack.town();
  protected override face: CardSide;
  public override readonly name: string;

  public constructor(params: ResourceParams) {
    super();
    this.name = params.name;
    this.face = new ResourceFace(this, params);
    this.initialSide = this.face;
  }
}

class ResourceFace extends CardFace {
  public constructor(card: ResourceCard, params: ResourceParams) {
    super({
      canInspect: true,
      actions: [new ProduceResourcesAction(card, params.produce), new StorageAction(card)],
      ...params,
    });
  }
}

class ProduceResourcesAction implements ItemAction {
  public constructor(
    private readonly card: ResourceCard,
    public readonly produce: ProduceQuantities,
  ) {}

  get state(): ItemActionState {
    const validContainers: (Container | undefined)[] = [game.containers.hand, game.containers.storageItems];
    return validContainers.includes(this.card.container) ? 'enabled' : 'disabled';
  }
  get text(): string {
    return `Produce ${formatQuantities(this.produce)}.`;
  }

  async execute(): Promise<void> {
    for (const [key, quantity] of Object.entries(this.produce)) {
      const resource = Number(key) as Resource;
      game.resources.resources[resource].quantity += quantity;
    }
    await game.containers.discardPile.addItems(this.card);
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
      produce: { [Resource.Food]: 3 },
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
