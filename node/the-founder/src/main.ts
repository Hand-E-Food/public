import { FoundTown } from './boosters/index.js';
import { NegativeStack, ZIndex } from './containers/index.js';
import {
  type YearEndedProperties,
  GameEvent,
  type YearStartedProperties,
  type YearEndingProperties,
} from './events/index.js';
import { eventHub, game, tutorials, type GameEventListener } from './singleton/index.js';
import {
  CheckMorale,
  DiscardHand,
  DrawCards,
  ModalTitleScreen,
  ModalTutorial,
  ModalYear,
  OpenBoosterPack,
  PlayerPhase,
  Primitive,
} from './states/index.js';

export class Main {
  private validateAllFamiliesFedListener!: GameEventListener;
  private readonly checkMorale = new CheckMorale();
  private readonly discardHand = new DiscardHand();
  private readonly drawCards = new DrawCards();
  private readonly modalYear = new ModalYear();
  private readonly playerPhase = new PlayerPhase();

  public async execute(): Promise<void> {
    document.body.appendChild(game.htmlElement);
    await this.showTitleScreen();
    const listeners = [
      eventHub.add(GameEvent.YearStarted, 50, (props) => this.drawCards.execute(props)),
      eventHub.add(GameEvent.YearEnded, 10, (props) => this.discardHand.execute(props)),
      eventHub.add(GameEvent.YearEnded, 50, (props) => this.checkMorale.execute(props)),
    ];
    this.validateAllFamiliesFedListener = eventHub.add(GameEvent.YearEnding, 10, (props) =>
      this.validateAllFamiliesFed(props),
    );

    await Promise.all([Primitive.fadeIn(this.playerPhase.endYearButton), this.modalYear.execute()]);
    await this.openFirstBoosterPack();
    const gameOver = await this.runGameLoop();

    eventHub.remove(...listeners);
    eventHub.remove(this.validateAllFamiliesFedListener);

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

  private async validateAllFamiliesFed(props: YearEndingProperties): Promise<void> {
    const key = 'HungryFamily';
    if (!tutorials.shouldShow(key)) {
      eventHub.remove(this.validateAllFamiliesFedListener);
      return;
    }
    if (!game.containers.negativeStack.items.find((item) => item.activeSide.name === 'Hungry Family')) return;
    await ModalTutorial.show({
      key,
      paragraphs: [
        'Not all of your townsfolk have been fed. Only fed families will be productive for the community. ' +
          'Hungry families will spend the year taking care of their own needs and not contribute.',
        'Feed as many families as you can each year to maximize productivity.',
      ],
      left: `${NegativeStack.left + NegativeStack.width + 10}px`,
      top: `${10 + 210 / 2}px`,
      width: `200px`,
    });
    eventHub.remove(this.validateAllFamiliesFedListener);
    props.cancel = true;
    props.stop = true;
  }

  private async runGameLoop(): Promise<() => Promise<void>> {
    while (true) {
      const gameOver = await this.invokeYearEnded();
      if (gameOver) return gameOver;
      game.year++;
      await this.modalYear.execute();
      await this.invokeYearStarted();
      await this.playerPhase.execute();
    }
  }

  private async invokeYearEnded(): Promise<(() => Promise<void>) | undefined> {
    const props: YearEndedProperties = { stop: false };
    await eventHub.invoke(GameEvent.YearEnded, props);
    return props.gameOver;
  }

  private async invokeYearStarted(): Promise<void> {
    const props: YearStartedProperties = { stop: false };
    await eventHub.invoke(GameEvent.YearStarted, props);
  }
}
