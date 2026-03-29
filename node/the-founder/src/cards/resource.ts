import { Card } from './card.js';
import { CardBack } from './card-back.js';
import { CardFace } from './card-face.js';

interface ResourceParams {
  readonly image: string;
  readonly name: string;
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
    super(params);
  }
}

export class Fish extends Resource {
  public constructor() {
    super({ image: 'fish.jpg', name: 'Fish' });
  }
}

export class Wood extends Resource {
  public constructor() {
    super({ image: 'wood.jpg', name: 'Wood' });
  }
}
