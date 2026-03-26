import { CardContainer } from "./card-container.js";

/** Contains cards actively providing negative morale. */
export class NegativeStack extends CardContainer {
    protected arrangeCards(): void {
        const step = 30;
        const left = 85;
        let top = 10 - step;
        let zIndex = 200;
        for (const card of this.cards) {
            top -= step * card.activeSide.morale;
            card.htmlElement.style.left = `${left}px`;
            card.htmlElement.style.top = `${top}px`;
            card.htmlElement.style.zIndex = `${zIndex}`;
            zIndex++;
        }
    }
}
