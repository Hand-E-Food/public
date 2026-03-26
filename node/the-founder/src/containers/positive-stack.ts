import { CardContainer } from "./card-container.js";

/** Contains cards actively providing positive morale. */
export class PositiveStack extends CardContainer {
    protected arrangeCards(): void {
        const step = 30;
        const left = 10;
        let top = 10 - step;
        let zIndex = 100;
        for (const card of this.cards) {
            top += step * card.activeSide.morale;
            card.htmlElement.style.left = `${left}px`;
            card.htmlElement.style.top = `${top}px`;
            card.htmlElement.style.zIndex = `${zIndex}`;
            zIndex++;
        }
    }
}
