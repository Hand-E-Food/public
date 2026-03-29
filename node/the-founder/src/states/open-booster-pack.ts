import type { BoosterItemGroup, BoosterPack } from '../boosters/booster-pack.js';
import { ZIndex } from '../containers/index.js';
import { game } from '../game.js';
import { Item } from '../item.js';
import { DestroyItems, HideElement, MoveItems, ShowElement } from './animations/index.js';
import { WaitTime } from './animations/wait-time.js';
import type { GameState } from './game-state.js';
import { Sequence } from './sequence.js';

type Properties = {
  readonly boosterPack: BoosterPack;
  groups?: BoosterItemGroup[];
};

export class OpenBoosterPack extends Sequence {
  public groups!: BoosterItemGroup[];

  constructor(public readonly boosterPack: BoosterPack) {
    const state: Properties = { boosterPack };
    const boosterTray = game.containers.boosterTray;

    super([
      new ShowElement(boosterTray.htmlElement),
      new MoveItems(boosterTray, [boosterPack]),
      new ClickBoosterPack(state),
      new DestroyItems([boosterPack]),
      new SpreadItems(state),
      new ExploreItems(state),
      new WaitTime(Item.transitionTime),
      new HideElement(boosterTray.htmlElement),
    ]);
  }
}

class ClickBoosterPack implements GameState {
  public constructor(private readonly state: Properties) {}

  enter(): void {
    const groups = (this.state.groups = this.state.boosterPack.open());
    const items = groups.flatMap((group) => group.items);
    game.addItems(game.containers.boosterTray, items);
    this.state.boosterPack.htmlElement.style.zIndex = `${ZIndex.Overlay + 99}`;
  }

  onItemClicked(item: Item, _modifier: number): void {
    if (item !== this.state.boosterPack) return;
    game.popState();
  }
}

class SpreadItems implements GameState {
  public constructor(private readonly state: Properties) {}

  enter(): void {
    const transitionTime = Math.max(
      ...this.state.groups!.flatMap((group) => group.items).map((item) => item.transitionTime),
    );
    game.containers.boosterTray.spreadItems();
    setTimeout(() => game.popState(), transitionTime);
  }
}

class ExploreItems implements GameState {
  public constructor(private readonly state: Properties) {}

  resume(): void {
    if (game.containers.boosterTray.items.length === 0) game.popState();
  }

  onItemClicked(item: Item, _modifier: number): void {
    const boosterTray = game.containers.boosterTray;
    if (item.container !== boosterTray) return;
    const group = this.state.groups!.find((group) => group.items.includes(item));
    if (!group) return;
    const items = group.items.filter((item2) => item2.name === item.name).reverse();
    group.container.addItems(items);
    if (boosterTray.items.length === 0) game.popState();
  }
}
