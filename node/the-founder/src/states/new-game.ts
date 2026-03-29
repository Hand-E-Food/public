import { BoosterPack, FoundTown } from '../boosters/index.js';
import { ZIndex } from '../containers/z-index.js';
import { game } from '../game.js';
import { Report } from './animations/index.js';
import { AnnualCycle } from './annual-cycle.js';
import type { GameState } from './game-state.js';
import { OpenBoosterPack } from './open-booster-pack.js';
import { ReportYear } from './report-year.js';
import { Sequence } from './sequence.js';

export class NewGame extends Sequence {
  public constructor() {
    document.body.appendChild(game.htmlElement);
    const boosterPack = new FoundTown();
    boosterPack.reposition(`calc(50vw - ${boosterPack.width}px / 2)`, -boosterPack.height, ZIndex.Overlay + 1);

    super([
      new ReportTitleScreen(),
      new ReportYear(),
      new AddBoosterPack(boosterPack),
      new OpenBoosterPack(boosterPack),
      new AnnualCycle(),
    ]);
  }
}

class ReportTitleScreen extends Report {
  public constructor() {
    const htmlElement = document.createElement('div');
    htmlElement.classList.add('title-screen');
    htmlElement.onclick = () => game.popState();
    htmlElement.innerHTML =
      '<h1>The Founder</h1>' +
      '<p>Click to start</p>' +
      '<footer>Copyright 2026 Mark Richardson, All rights reserved.</footer>';
    super(htmlElement);
  }
}

class AddBoosterPack implements GameState {
  public constructor(private readonly boosterPack: BoosterPack) {}

  enter(): void {
    game.addItems(undefined, [this.boosterPack]);
    game.popState();
  }
}
