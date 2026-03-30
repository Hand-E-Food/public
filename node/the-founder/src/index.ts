import { stateMachine } from './state-machine.js';
import { NewGame } from './states/index.js';

stateMachine.push(new NewGame());
