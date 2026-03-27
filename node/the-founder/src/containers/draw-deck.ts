import { Container } from "./container.js";
import { Spacing } from "./constants.js";
import { Card } from "../cards/index.js";

/** The deck of cards to draw from. */
export class DrawDeck extends Container {
    protected override arrange(): void {
        let left = Spacing;
        let top = Spacing + Card.height;
        let zIndex = 0;
        for (let i = 0; i < this.items.length; i++) {
            const item = this.items[i];
            if (!item) continue;
            item.reposition(left, `calc(100vh - ${top}px)`, zIndex);
            zIndex++;
            if (i < 5) {
                left += 1;
                top += 1;
            }
        }
    }
}
