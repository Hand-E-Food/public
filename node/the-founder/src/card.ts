import type { CardSide } from "./card-side.js";

/** One card. */
export class Card {
    private _activeSide: number = 0;
    private readonly flipDiv: HTMLDivElement;
    private readonly sides: CardSide[];

    /** This card's HTML element. */
    public readonly htmlElement: HTMLDivElement;

    /**
     * Creates a new card.
     * @param sides This card's one or two sides.
     */
    public constructor(...sides: CardSide[]) {
        if (sides.length < 1 || sides.length > 2) throw new Error('A card must have one or two sides.');
        this.sides = sides;
        this.flipDiv = document.createElement('div');
        let first = true;
        for (const side of sides) {
            if (first) first = false;
            else side.htmlElement.classList.add('flipped');
            (side as any).card = this;
            this.flipDiv.appendChild(side.htmlElement);
        }
        this.htmlElement = document.createElement("div");
        this.htmlElement.classList.add('card');
        this.htmlElement.appendChild(this.flipDiv);
        this.reposition(200, 100, 0);
    }

    /** This card's face-up side. */
    public get activeSide(): CardSide {
        const side = this.sides[this._activeSide];
        if (!side) throw new Error('A non-existent side is active.');
        return side;
    }

    /** Flip this card to it's other side. */
    public flip(): void {
        if (this.sides.length === 1) throw new Error("Cannot flip this card.");
        const classList = this.flipDiv.classList;
        if (classList.contains('flipped')) {
            classList.remove('flipped');
        } else {
            classList.add('flipped');
        }
        this._activeSide = 1 - this._activeSide;
    }

    /**
     * Set this card's physical position.
     * @param left The x-coordinate of this card's left side.
     * @param top The y-coordinate of this card's top side.
     * @param zIndex This card's Z index. Higher numbers are on top.
     */
    public reposition(left: number, top: number, zIndex: number): void {
        this.htmlElement.style.left = `${left}px`;
        this.htmlElement.style.top = `${top}px`;
        this.htmlElement.style.zIndex = `${zIndex}`;
    }
}
