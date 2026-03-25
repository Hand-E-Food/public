import type { Card } from "./card";

export class CardSide {
    protected readonly card!: Card;
    public readonly htmlElement: HTMLDivElement;

    public constructor(image: string) {
        this.htmlElement = document.createElement('div');
        this.htmlElement.classList.add('card-side');
        this.htmlElement.innerHTML = `<img src="assets/${image}" />`;
        this.htmlElement.onclick = (event) => this.onCardClicked(event);
    }

    public onCardClicked(event: MouseEvent): void { }
}
