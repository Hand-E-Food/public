import { Game } from "./game.js";
import { Family } from "./cards/index.js";

document.body.appendChild(Game.htmlElement);
Game.addCard(new Family());

