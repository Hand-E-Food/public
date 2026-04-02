/** The events that can be invoked. */
export enum GameEvent {
  /**
   * Invoked at the start of each year.
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
   * - 50: check morale
   */
  EndYear,
}
