import { Card } from "./card/card";

export abstract class CardStack {
  public readonly cards: Card[] = [];

  /**
   * Adds the specified card to this stack.
   * @param card The card to add.
   * @throws The card is not valid in this stack.
   */
  public abstract addCard(card: Card): void;

  /**
   * Removes the specified card form this stack.
   * @param card The card to remove.
   * @throws The card is not in this stack.
   */
  public removeCard(card: Card): void {
    const i = this.cards.indexOf(card);
    if (i === -1) throw new Error("Cannot remove a card that is not in this stack.");
    this.cards.splice(i, 1);
    this.update();
  }

  /**
   * Called when there is a change to the cards in this stack. Updates all of this stack's cards.
   */
  protected abstract update(): void;
}
