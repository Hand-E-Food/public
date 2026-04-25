import { CardSide } from './card-side.js';

/** Factory methods for creating the back side of cards. */
export class CardBack extends CardSide {
  private constructor(params: { image: string }) {
    super({
      ...params,
      canInspect: false,
      name: '',
      flavourText: '',
      actions: [],
    });
  }

  /** The card back for an event. */
  public static event(): CardSide {
    return new CardBack({ image: 'event.png' });
  }

  /** The card back for the draw deck. */
  public static drawDeck(): CardSide {
    return new CardBack({ image: 'town.png' });
  }
}
