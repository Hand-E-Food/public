import { OpenBoosterPack } from './open-booster-pack.js';
import { FoundTown } from '../boosters/index.js';
import type { GameState } from './game-state.js';
import { AnnualCycle } from './annual-cycle.js';
import { Sequence } from './sequence.js';
import { game } from '../game.js';

export class NewGame implements GameState {
  enter(): void {
    document.body.appendChild(game.htmlElement);
    const boosterPack = new FoundTown();
    boosterPack.reposition(`calc(50vw - ${boosterPack.width}px / 2)`, -boosterPack.height, 901);
    game.addItems(undefined, [boosterPack]);
    setTimeout(() => {
      game.nextState(new Sequence([new OpenBoosterPack(boosterPack), new AnnualCycle()]));
    }, 2000);
  }
}
