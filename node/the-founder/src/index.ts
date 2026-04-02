import { stateMachine } from './singleton/index.js';
import { NewGame } from './states/new-game.js';

stateMachine.push(new NewGame());
