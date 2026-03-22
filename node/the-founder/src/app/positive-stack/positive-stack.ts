import { Card } from "../card/card";
import { CardStack } from "../card-stack";

export class PositiveStack extends CardStack {
  public left: number = 252;
  public top: number = 30;

  public get morale(): number {
    return this.cards.map(card => card.morale).reduce((prev, curr) => prev + curr, 0);
  }

  public addCard(card: Card): void {
    if (!card.morale || card.morale < 0) throw new Error("Only cards with positive morale can be added to the positive morale stack.");
    this.cards.push(card);
    this.update();
  }

  protected override update(): void {
    const step = 30;
    let top = this.top - step;
    for (const card of this.cards) {
      top += card.morale * step;
      card.left = this.left;
      card.top = top;
    }
  }
}
