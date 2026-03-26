import { CardContainer } from "./card-container.js";

/** Contains families that have been fed. */
export class FedFamilyStack extends CardContainer {
    protected arrangeCards(): void {
        const step = 30;
        const left = 245;
        let top = 10 - step;
        let zIndex = 300;
        for (const card of this.cards) {
            top += step;
            card.htmlElement.style.left = `${left}px`;
            card.htmlElement.style.top = `${top}px`;
            card.htmlElement.style.zIndex = `${zIndex}`;
            zIndex++;
        }
    }
}
