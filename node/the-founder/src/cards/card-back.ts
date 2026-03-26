import { CardSide } from "../card-side.js";

/** Factory methods for creating the back side of cards. */
export class CardBack {
    private constructor() { }

    /** The standard card back. */
    public static standard(): CardSide {
        return new CardSide({ image: 'card-back.avif' });
    }
}
