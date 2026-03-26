import { Card } from "./card.js";
import { CardFace } from "./card-face.js";
import { NegativeCardFace } from "./negative-card-face.js";
import { game } from "../game.js";

export class Family extends Card {
    public constructor() {
        super(
            new HungryFamily(),
            new FedFamily(),
        );
    }
}

class HungryFamily extends NegativeCardFace {
    public constructor() {
        super({ image: 'hungry-family.jpg', name: 'Hungry Family' });
    }

    public override onCardClicked(event: MouseEvent): void {
        this.card.flip();
        game.containers.fedFamilyStack.addCards(this.card);
    }
}

class FedFamily extends CardFace {
    public constructor() {
        super({ image: 'fed-family.jpg', name: 'Fed Family' });
    }

    public override onCardClicked(event: MouseEvent): void {
        this.card.flip();
        game.containers.negativeStack.addCards(this.card);
    }
}
