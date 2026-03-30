import type { BoosterItemGroup, BoosterPack } from '../boosters/booster-pack.js';
import { ZIndex } from '../containers/index.js';
import { game } from '../game.js';
import { Item } from '../item.js';
import { type GameState, stateMachine } from '../state-machine.js';
import { DestroyItems, HideElement, MoveItems, ShowElement } from './animations/index.js';
import { WaitTime } from './animations/wait-time.js';
import { ModalTutorial } from './modal-tutorial.js';
import { Sequence } from './sequence.js';

type Properties = {
  readonly boosterPack: BoosterPack;
  groups?: BoosterItemGroup[];
  isQuick: boolean;
};

export class OpenBoosterPack extends Sequence {
  public groups!: BoosterItemGroup[];

  constructor(
    public readonly boosterPack: BoosterPack,
    isQuick = false,
  ) {
    const props: Properties = { boosterPack, isQuick };
    const boosterTray = game.containers.boosterTray;

    super([
      new ShowElement(boosterTray.htmlElement),
      new MoveItems(boosterTray, [boosterPack]),
      new ClickBoosterPack(props),
      new DestroyItems([boosterPack]),
      new SpreadItems(props),
      new ExploreItems(props),
      new WaitTime(Item.transitionTime),
      new HideElement(boosterTray.htmlElement),
    ]);
  }
}

class ClickBoosterPack implements GameState {
  public constructor(private readonly props: Properties) {}

  enter(): void {
    const groups = (this.props.groups = this.props.boosterPack.open());
    const items = groups.flatMap((group) => group.items);
    game.addItems(game.containers.boosterTray, items);
    this.props.boosterPack.htmlElement.style.zIndex = `${ZIndex.Overlay + 99}`;
    if (this.props.isQuick) this.onItemClicked(this.props.boosterPack, 1);
  }

  onItemClicked(item: Item, modifier: number): void {
    if (item !== this.props.boosterPack) return;
    this.props.isQuick = modifier === 1;
    stateMachine.pop();
  }
}

class SpreadItems implements GameState {
  public constructor(private readonly props: Properties) {}

  enter(): void {
    if (this.props.isQuick) {
      stateMachine.pop();
    } else {
      const transitionTime = Math.max(
        ...this.props.groups!.flatMap((group) => group.items).map((item) => item.transitionTime),
      );
      game.containers.boosterTray.spreadItems();
      setTimeout(() => stateMachine.pop(), transitionTime);
    }
  }
}

class ExploreItems implements GameState {
  public constructor(private readonly props: Properties) {}

  enter(): void {
    if (this.props.isQuick) {
      this.distributeAll();
    } else {
      stateMachine.push(
        new ModalTutorial('OpenBoosterPack-ExploreItems', {
          paragraphs: [
            "You've just opened a booster pack! Each booster pack contains cards and/or more booster packs. Click each pile to distribute them to the game.",
            'You will learn the details of each card as it becomes relevent.',
            '<strong>Tip:</strong> You can <strong>right-click</strong> or <strong>shift+click</strong> a booster pack to quickly distribute its contents.',
          ],
          left: 'calc(50vw - 200px)',
          width: '400px',
          bottom: `calc(50vh + 170px)`,
        }),
      );
    }
  }

  resume(): void {
    if (game.containers.boosterTray.items.length === 0) stateMachine.pop();
  }

  onItemClicked(item: Item, _modifier: number): void {
    const boosterTray = game.containers.boosterTray;
    if (item.container !== boosterTray) return;
    const group = this.props.groups!.find((group) => group.items.includes(item));
    if (!group) return;
    const items = group.items.filter((item2) => item2.name === item.name).reverse();
    group.container.addItems(items);
    if (boosterTray.items.length === 0) stateMachine.pop();
  }

  private distributeAll(): void {
    for (const group of this.props.groups!) {
      group.container.addItems(group.items.reverse());
    }
    stateMachine.pop();
  }
}
