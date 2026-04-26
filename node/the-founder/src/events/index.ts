import type { Card } from '../cards/index.js';

/** The events that can be invoked. */
export enum GameEvent {
  /**
   * Invoked at the start of each year.
   * - 50: fed families
   */
  YearStarted,

  /**
   * Invoked when a card is drawn from the draw deck.
   * - 50: discontent
   */
  CardDrawn,

  /**
   * Invoked when the player uses a card.
   */
  CardPlayed,

  /**
   * Invoked when a player ends the year to validate whether requirements have been met.
   * -  0: town founded
   * - 10: validate all families fed
   * - 20: validate morale is not negative
   */
  YearEnding,

  /**
   * Invoked when the player ends the year and all validation passes.
   * - 10: discard resources
   * - 20: discard hand
   * - 50: check morale
   */
  YearEnded,
}

export type GameEventProperties = {
  /** Set this to true to stop invoking lower priority listeners. */
  stop: boolean;
};

export type YearStartedProperties = GameEventProperties & {};

export type CardDrawnProperties = GameEventProperties & {
  /** The card that was drawn. */
  card: Card;
};

export type CardPlayedProperties = GameEventProperties & {
  /** The card that was played. */
  card: Card;
};

export type YearEndingProperties = GameEventProperties & {
  /** Set to true to cancel ending the year. */
  cancel: boolean;
};

export type YearEndedProperties = GameEventProperties & {
  /** The state transition to end the game with. */
  gameOver?: () => Promise<void>;
};
