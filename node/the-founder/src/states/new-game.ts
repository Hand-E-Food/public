import { BoosterPack, FoundTown } from '../boosters/index.js';
import { ZIndex } from '../containers/z-index.js';
import { game } from '../game.js';
import { type GameState, stateMachine } from '../state-machine.js';
import { Modal } from './animations/index.js';
import { AnnualCycle } from './annual-cycle.js';
import { ModalYear } from './modal-year.js';
import { OpenBoosterPack } from './open-booster-pack.js';
import { Sequence } from './sequence.js';

export class NewGame extends Sequence {
  public constructor() {
    document.body.appendChild(game.htmlElement);
    const boosterPack = new FoundTown();
    boosterPack.reposition(`calc(50vw - ${boosterPack.width}px / 2)`, -boosterPack.height, ZIndex.Overlay + 1);

    super([
      new ModalTitleScreen(),
      new ModalYear(),
      new AddBoosterPack(boosterPack),
      new OpenBoosterPack(boosterPack),
      new AnnualCycle(),
    ]);
  }
}

class ModalTitleScreen extends Modal {
  public constructor() {
    const htmlElement = document.createElement('div');
    htmlElement.classList.add('title-screen');
    htmlElement.onclick = () => stateMachine.pop();
    htmlElement.innerHTML =
      '<h1>The Founder</h1><footer>Copyright &copy; 2026 Mark Richardson, All rights reserved.</footer>';
    super(htmlElement);
  }
}

class AddBoosterPack implements GameState {
  public constructor(private readonly boosterPack: BoosterPack) {}

  enter(): void {
    game.addItems(undefined, [this.boosterPack]);
    stateMachine.pop();
  }
}
