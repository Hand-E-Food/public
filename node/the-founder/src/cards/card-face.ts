import { CardSide } from "../card-side.js";

export class CardFace extends CardSide {
    public constructor(image: string, name: string) {
        super(image);
        this.htmlElement.classList.add('card-face');
        this.htmlElement.innerHTML += `<span>${name}</span>`
    }
}
