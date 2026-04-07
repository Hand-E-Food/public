import { Card } from './card.js';
import { CardBack } from './card-back.js';
import { CardFace } from './card-face.js';

type ResourceParams = {
  readonly image: string;
  readonly name: string;
  readonly flavourText: string;
}

class Resource extends Card {
  public override readonly name: string;

  public constructor(params: ResourceParams) {
    super({
      sides: [new ResourceFace(params), CardBack.drawDeck()],
    });
    this.name = params.name;
  }
}

class ResourceFace extends CardFace {
  public constructor(params: ResourceParams) {
    super({
      canInspect: true,
      actions: [],
      ...params,
    });
  }
}

export class Fish extends Resource {
  public constructor() {
    super({
      name: 'Fish',
      image: 'fish.jpg',
      flavourText: '',
    });
  }
}

export class Wood extends Resource {
  public constructor() {
    super({
      name: 'Wood',
      image: 'wood.jpg',
      flavourText: '',
    });
  }
}
