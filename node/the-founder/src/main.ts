import { FoundTown } from './boosters/index.js';
import { ZIndex } from './containers/index.js';
import { type EndYearProperties, GameEvent, type NewYearProperties } from './events/index.js';
import { eventHub, game } from './singleton/index.js';
import {
  CheckMorale,
  DiscardHand,
  DrawCards,
  ModalTitleScreen,
  ModalYear,
  OpenBoosterPack,
  PlayerPhase,
  Primitive,
} from './states/index.js';

export class Main {
  private readonly checkMorale = new CheckMorale();
  private readonly discardHand = new DiscardHand();
  private readonly drawCards = new DrawCards();
  private readonly modalYear = new ModalYear();
  private readonly playerPhase = new PlayerPhase();

  public async execute(): Promise<void> {
    document.body.appendChild(game.htmlElement);
    await this.showTitleScreen();
    const listeners = [
      eventHub.add(GameEvent.NewYear, 50, (props) => this.drawCards.execute(props)),
      eventHub.add(GameEvent.EndYear, 10, (props) => this.discardHand.execute(props)),
      eventHub.add(GameEvent.EndYear, 50, (props) => this.checkMorale.execute(props)),
    ];
    await Promise.all([Primitive.fadeIn(this.playerPhase.endYearButton), this.modalYear.execute()]);
    await this.openFirstBoosterPack();
    const gameOver = await this.runGameLoop();
    eventHub.remove(...listeners);
    gameOver();
  }

  private async showTitleScreen(): Promise<void> {
    await new ModalTitleScreen().execute();
  }

  private async openFirstBoosterPack(): Promise<void> {
    const boosterPack = new FoundTown();
    boosterPack.htmlElement.style.top = `calc(100vh + 1px)`;
    boosterPack.htmlElement.style.zIndex = `${ZIndex.Overlay + 1}`;
    game.addItems(undefined, boosterPack);
    await new OpenBoosterPack(boosterPack).execute();
  }

  private async runGameLoop(): Promise<() => Promise<void>> {
    while (true) {
      const gameOver = await this.invokeEndYear();
      if (gameOver) return gameOver;
      game.year++;
      await this.modalYear.execute();
      await this.invokeNewYear();
      await this.playerPhase.execute();
    }
  }

  private async invokeEndYear(): Promise<(() => Promise<void>) | undefined> {
    const props: EndYearProperties = { stop: false };
    await eventHub.invoke(GameEvent.EndYear, props);
    return props.gameOver;
  }

  private async invokeNewYear(): Promise<void> {
    const props: NewYearProperties = { stop: false };
    await eventHub.invoke(GameEvent.NewYear, props);
  }
}
