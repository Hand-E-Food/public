import { CardSide } from "../card-side.js";

export class CardBack {
    private constructor() { }

    public static standard(): CardSide {
        return new CardSide('card-back.avif');
    }
}
