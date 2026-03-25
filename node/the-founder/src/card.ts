import type { CardSide } from "./card-side.js";

export class Card {
    private readonly flipDiv: HTMLDivElement;
    public readonly htmlElement: HTMLDivElement;

    public constructor(side1: CardSide, side2?: CardSide) {
        (side1 as any).card = this;
        this.flipDiv = document.createElement('div');
        this.flipDiv.appendChild(side1.htmlElement);
        if (side2) {
            (side2 as any).card = this;
            side2.htmlElement.classList.add('flipped');
            this.flipDiv.appendChild(side2.htmlElement);
        }
        this.htmlElement = document.createElement("div");
        this.htmlElement.classList.add('card');
        this.htmlElement.appendChild(this.flipDiv);
        this.reposition(200, 100, 0);
    }

    public flip(): void {
        if (this.flipDiv.children.length < 2) throw new Error("Cannot flip this card.");
        const classList = this.flipDiv.classList;
        if (classList.contains('flipped')) {
            classList.remove('flipped');
        } else {
            classList.add('flipped');
        }
    }

    public reposition(left: number, top: number, zIndex: number): void {
        this.htmlElement.style.left = `${left}px`;
        this.htmlElement.style.top = `${top}px`;
        this.htmlElement.style.zIndex = `${zIndex}`;
    }
}
