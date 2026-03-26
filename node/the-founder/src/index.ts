import { game } from "./game.js";
import { Family, PositiveCard } from "./cards/index.js";

document.body.appendChild(game.htmlElement);
game.addCards(
    game.containers.positiveStack,
    new PositiveCard({ image: 'town-square.avif', name: 'Town Square' }),
    new PositiveCard({ image: 'fishery.jpg', name: 'Fishery' }),
    new PositiveCard({ image: 'logger.jpg', name: 'Logger' }),
);
game.addCards(
    game.containers.negativeStack,
    new Family(),
    new Family(),
    new Family(),
);
