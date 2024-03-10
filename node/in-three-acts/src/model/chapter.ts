import { Suit } from "./suit";

export class Chapter {
    public readonly name: string;
    public readonly wild: boolean;
    public readonly inspires: Suit[];

    public suit: Suit;

    public constructor(suit: Suit, wild: boolean, inspires: Suit[], name: string) {
        if (inspires.length !== 2 || inspires[0] === inspires[1]) throw new Error("A chapter must inspire two different suits.");
        this.name = name;
        this.suit = suit;
        this.inspires = inspires;
        this.wild = wild;
    }
}
