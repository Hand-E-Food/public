import { FoundTown } from './boosters/index.js';
import { GameEvent, type YearEndedProperties, type YearStartedProperties } from './events/index.js';
import { AllFamiliesFedValidation, NegativeMoraleValidation, TownFoundedValidation } from './rules/index.js';
import { eventHub, game } from './singleton/index.js';
import { Animate, DrawCards, ModalTitleScreen, ModalYear, PlayerPhase, YearEnded } from './states/index.js';

export class Main {
  private readonly drawCards = new DrawCards();
  private readonly modalYear = new ModalYear();
  private readonly playerPhase = new PlayerPhase();
  private readonly yearEnded = new YearEnded();

  public async execute(): Promise<void> {
    document.body.appendChild(game.htmlElement);
    const listeners = [
      eventHub.add(GameEvent.YearStarted, 50, (props) => this.drawCards.execute(props)),
      eventHub.add(GameEvent.YearEnded, 10, (props) => this.yearEnded.discardResources(props)),
      eventHub.add(GameEvent.YearEnded, 20, (props) => this.yearEnded.discardHand(props)),
      eventHub.add(GameEvent.YearEnded, 50, (props) => this.yearEnded.checkMorale(props)),
      new AllFamiliesFedValidation().listener,
      new NegativeMoraleValidation().listener,
      new TownFoundedValidation().listener,
    ];
    await this.showTitleScreen();
    await this.showFirstBoosterPack();
    const gameOver = await this.runGameLoop();
    eventHub.remove(...listeners);
    await gameOver();
  }

  private async showTitleScreen() {
    await new ModalTitleScreen().execute();
  }

  private async showFirstBoosterPack() {
    const boosterPack = new FoundTown();
    await Promise.all([
      game.addItems(game.containers.boosterPacks, boosterPack),
      Animate.fadeIn(boosterPack.htmlElement),
      Animate.fadeIn(this.playerPhase.endYearButton),
    ]);
  }

  private async runGameLoop(): Promise<() => Promise<void>> {
    let gameOver: (() => Promise<void>) | undefined = undefined;
    while (!gameOver) {
      game.year++;
      await this.modalYear.execute();
      await this.invokeYearStarted();
      await this.playerPhase.execute();
      gameOver = await this.invokeYearEnded();
    }
    return gameOver;
  }

  private async invokeYearStarted(): Promise<void> {
    const props: YearStartedProperties = { stop: false };
    await eventHub.invoke(GameEvent.YearStarted, props);
  }

  private async invokeYearEnded(): Promise<(() => Promise<void>) | undefined> {
    const props: YearEndedProperties = { stop: false };
    await eventHub.invoke(GameEvent.YearEnded, props);
    return props.gameOver;
  }
}
