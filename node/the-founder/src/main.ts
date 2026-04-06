import { FoundTown } from './boosters/index.js';
import { ZIndex } from './containers/index.js';
import { GameEvent, type EndYearProperties, type NewYearProperties } from './events/index.js';
import { eventHub, game } from './singleton/index.js';
import { CheckMorale, DiscardHand, DrawCards, ModalTitleScreen, ModalYear, OpenBoosterPack } from './states/index.js';

export class Main {
  private readonly checkMorale = new CheckMorale();
  private readonly discardHand = new DiscardHand();
  private readonly drawCards = new DrawCards();
  private readonly modalYear = new ModalYear();

  public async execute(): Promise<void> {
    document.body.appendChild(game.htmlElement);
    await this.showTitleScreen();
    const listeners = [
      eventHub.add(GameEvent.NewYear, 50, (props) => this.drawCards.execute(props)),
      eventHub.add(GameEvent.EndYear, 10, (props) => this.discardHand.execute(props)),
      eventHub.add(GameEvent.EndYear, 50, (props) => this.checkMorale.execute(props)),
    ];
    await this.modalYear.execute();
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
      const result = await this.endYear();
      if (result) return result;
      game.year++;
      await this.modalYear.execute();
      await this.newYear();
      await this.playCards();
    }
  }

  private async endYear(): Promise<(() => Promise<void>) | undefined> {
    const props: EndYearProperties = { stop: false };
    await eventHub.invoke(GameEvent.EndYear, props);
    return props.gameOver;
  }

  private async newYear(): Promise<void> {
    const props: NewYearProperties = { stop: false };
    await eventHub.invoke(GameEvent.NewYear, props);
  }

  private async playCards(): Promise<void> {}
}
