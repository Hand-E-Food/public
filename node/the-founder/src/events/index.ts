import type { Card } from '../cards/index.js';

/** The events that can be invoked. */
export enum GameEvent {
  /**
   * Invoked at the start of each year.
   * - 50: fed families
   */
  NewYear,

  /**
   * Invoked when a card is drawn from the draw deck.
   */
  CardDrawn,

  /**
   * Invoked when the player uses a card.
   */
  CardPlayed,

  /**
   * Invoked when the player ends the year.
   * - 10: discard hand
   * - 50: check morale
   */
  EndYear,
}

export type GameEventProperties = {
  /** Set this to true to stop invoking lower priority listeners. */
  stop: boolean;
};

export type NewYearProperties = GameEventProperties & {};

export type CardDrawnProperties = GameEventProperties & {
  /** The card that was drawn. */
  card: Card;
};

export type CardPlayedProperties = GameEventProperties & {
  /** The card that was played. */
  card: Card;
};

export type EndYearProperties = GameEventProperties & {
  /** The state transition to end the game with. */
  gameOver?: () => Promise<void>;
};
