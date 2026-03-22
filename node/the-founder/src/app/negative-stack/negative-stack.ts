import { Card } from "../card/card";
import { CardStack } from "../card-stack";

export class NegativeStack extends CardStack {
  public left: number = 189;
  public top: number = 30;

  public get morale(): number {
    return this.cards.map(card => card.morale).reduce((prev, curr) => prev - curr, 0);
  }

  public addCard(card: Card): void {
    if (!card.morale || card.morale > 0) throw new Error("Only cards with negative morale can be added to the negative morale stack.");
    this.cards.push(card);
    this.cards.sort((a, b) => a.name.localeCompare(b.name));
    this.update();
  }

  protected update(): void {
    const step = 30;
    let top = this.top - step;
    for (const card of this.cards) {
      top -= card.morale * step; // morale is negative
      card.left = this.left;
      card.top = top;
    }
  }
}
