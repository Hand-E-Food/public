import { BoosterPack, FoundTown } from '../boosters/index.js';
import { ZIndex } from '../containers/z-index.js';
import { game, stateMachine } from '../singleton/index.js';
import { EndOfYear } from './end-of-year.js';
import { ModalYear } from './modal-year.js';
import { OpenBoosterPack } from './open-booster-pack.js';
import { type GameState, Modal, Sequence } from './primitive/index.js';

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
      new EndOfYear(),
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
