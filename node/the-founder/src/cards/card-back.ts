import { CardSide } from './card-side.js';

/** Factory methods for creating the back side of cards. */
export class CardBack {
  private constructor() {}

  /** The card back for an event. */
  public static event(): CardSide {
    return new CardSide({ image: 'event.avif' });
  }

  /** The card back for the draw deck. */
  public static drawDeck(): CardSide {
    return new CardSide({ image: 'card-back.avif' });
  }
}
