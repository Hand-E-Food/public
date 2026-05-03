import { Card, CardFace } from './common/index.js';

export class StorageSpace extends Card {
  public override readonly name: string = 'Storage Space';

  public constructor() {
    super();
    this.initialSide = new CardFace({
      image: 'storage.png',
      canInspect: false,
      name: '',
      flavourText: '',
      actions: [],
    });
  }
}
