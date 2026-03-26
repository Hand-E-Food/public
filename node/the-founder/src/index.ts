import { Game } from "./game.js";
import { Family, PositiveCard } from "./cards/index.js";

document.body.appendChild(Game.htmlElement);
Game.addCards(
    new PositiveCard({ image: 'town-square.avif', name: 'Town Square' }),
    new PositiveCard({ image: 'fishery.jpg', name: 'Fishery' }),
    new PositiveCard({ image: 'logger.jpg', name: 'Logger' }),
    new Family(),
    new Family(),
    new Family(),
);
