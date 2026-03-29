import { game } from './game.js';
import { NewGame } from './states/index.js';

game.pushState(new NewGame());
