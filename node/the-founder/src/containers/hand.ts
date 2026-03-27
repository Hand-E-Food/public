import { Card } from "../cards/index.js";
import { Spacing } from "./constants.js";
import { Container } from "./container.js";

export class Hand extends Container {
  protected override arrange(): void {
    const step = Card.width + Spacing / 2;
    let left = Spacing * 2 + 5 + Card.width;
    const top = Spacing + Card.height;
    let zIndex = 0;
    for (const item of this.items) {
      item.reposition(left, `calc(100vh - ${top}px)`, zIndex);
      left += step;
      zIndex++;
    }
  }
}
