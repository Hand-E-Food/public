import type { BoosterItemGroup, BoosterPack } from '../boosters/booster-pack.js';
import { DestroyItems } from './animations/destroy-items.js';
import { ShowElement } from './animations/show-element.js';
import { HideElement } from './animations/hide-element.js';
import { MoveItems } from './animations/move-item.js';
import type { GameState } from './game-state.js';
import { Sequence } from './sequence.js';
import type { Item } from '../item.js';
import { game } from '../game.js';

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
      new HideElement(boosterTray.htmlElement),
    ]);
  }
}

class ClickBoosterPack implements GameState {
  public constructor(private readonly state: Properties) {}

  enter(): void {
    this.state.boosterPack.htmlElement.style.zIndex = '1000';
    const groups = (this.state.groups = this.state.boosterPack.open());
    const items = groups.flatMap((group) => group.items);
    game.addItems(game.containers.boosterTray, items);
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
  private isEnabled: boolean = true;

  public constructor(private readonly state: Properties) {}

  enter(): void {
    this.isEnabled = true;
  }

  pause(): void {
    this.isEnabled = false;
  }

  resume(): void {
    if (game.containers.boosterTray.items.length > 0) this.isEnabled = true;
    else game.popState();
  }

  exit(): void {
    this.isEnabled = false;
  }

  onItemClicked(item: Item, _modifier: number): void {
    if (!this.isEnabled) return;
    const group = this.state.groups!.find((group) => group.items.includes(item));
    if (!group) return;
    const items = group.items.filter((item2) => item2.name === item.name);
    game.pushState(new MoveItems(group.container, items));
  }
}
