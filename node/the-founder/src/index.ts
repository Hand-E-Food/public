import { Settlement } from './boosters/index.js';
import { game } from './game.js';

document.body.appendChild(game.htmlElement);
game.addItems(game.containers.boosterTray, [new Settlement()]);
