import { Card } from "../card.js";
import { CardFace } from "./card-face.js";

export class Family extends Card {
    public constructor() {
        super(
            new HungryFamily(),
            new FedFamily(),
        );
    }
}

class HungryFamily extends CardFace {
    public constructor() {
        super('family.jpg', 'Hungry Family');
    }

    public override onCardClicked(event: MouseEvent): void {
        this.card.flip();
    }
}

class FedFamily extends CardFace {
    public constructor() {
        super('family.jpg', 'Fed Family');
    }

    public override onCardClicked(event: MouseEvent): void {
        this.card.flip();
    }
}
