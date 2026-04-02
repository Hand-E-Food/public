import { BoosterPack, FoundTown } from '../boosters/index.js';
import { ZIndex } from '../containers/z-index.js';
import { eventHub, game, GameEvent, type GameEventListener } from '../singleton/index.js';
import { CheckMorale } from './check-morale.js';
import { DrawCards } from './draw-cards.js';
import { EndYear } from './end-year.js';
import { ModalYear } from './modal-year.js';
import { OpenBoosterPack } from './open-booster-pack.js';
import { Instant, Modal, Sequence } from './primitive/index.js';

export class NewGame extends Sequence {
  private readonly listeners: GameEventListener[] = [];

  public override readonly name: string = 'NewGame';

  public constructor() {
    document.body.appendChild(game.htmlElement);
    const boosterPack = NewGame.createFirstBoosterPack();

    super([
      new ModalTitleScreen(),
      new ModalYear(),
      new AddBoosterPack(boosterPack),
      new OpenBoosterPack(boosterPack),
      new EndYear(),
    ]);
  }

  private static createFirstBoosterPack() {
    const boosterPack = new FoundTown();
    boosterPack.reposition(`calc(50vw - ${boosterPack.width}px / 2)`, -boosterPack.height, ZIndex.Overlay + 1);
    return boosterPack;
  }

  override enter(): void {
    this.listeners.push(
      eventHub.add(GameEvent.NewYear, 50, () => new DrawCards()),
      eventHub.add(GameEvent.EndYear, 50, (props) => new CheckMorale(props)),
    );
    super.enter();
  }

  exit(): void {
    eventHub.remove(this.listeners);
  }
}

class ModalTitleScreen extends Modal {
  public override readonly name: string = 'ModalTitleScreen';

  public constructor() {
    const htmlElement = document.createElement('div');
    htmlElement.classList.add('title-screen');
    htmlElement.innerHTML =
      '<h1>The Founder</h1><footer>Copyright &copy; 2026 Mark Richardson, All rights reserved.</footer>';
    super(htmlElement);
  }
}

class AddBoosterPack extends Instant {
  public readonly name: string;

  public constructor(private readonly boosterPack: BoosterPack) {
    super();
    this.name = `AddBoosterPack(${boosterPack.name})`;
  }

  override execute(): void {
    game.addItems(undefined, this.boosterPack);
  }
}
