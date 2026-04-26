import { Card } from './card.js';
import { CardBack } from './card-back.js';

export class StorageSpace extends Card {
  public override readonly name: string = 'Storage Space';

  public constructor() {
    super({ sides: [CardBack.storage()] });
  }
}
