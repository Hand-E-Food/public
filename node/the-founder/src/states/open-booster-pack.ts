import type { BoosterItemGroup, BoosterPack } from '../boosters/booster-pack.js';
import { Item } from '../item.js';
import { PromiseQueue } from '../promise-queue.js';
import { game } from '../singleton/index.js';
import { Animate } from './animate.js';
import { ModalTutorial } from './modal-tutorial.js';

export class OpenBoosterPack {
  public groups!: BoosterItemGroup[];

  public constructor(
    public readonly boosterPack: BoosterPack,
    private isQuick = false,
  ) {}

  public async execute(): Promise<void> {
    const boosterTray = game.containers.boosterTray;
    await Animate.fadeIn(boosterTray.htmlElement);
    await boosterTray.addBoosterPack(this.boosterPack);
    this.groups = this.boosterPack.open();
    game.addItems(game.containers.boosterTray, ...this.groups.flatMap((group) => group.items));
    if (!this.isQuick) {
      await this.waitClickBoosterPack();
    }
    await game.destroyItems(this.boosterPack);
    if (!this.isQuick) {
      await boosterTray.spreadItems();
      await this.showTutorial();
      await this.exploreItems();
    } else {
      await this.distributeAllItems();
    }
    await Animate.fadeOut(boosterTray.htmlElement);
  }

  private async waitClickBoosterPack(): Promise<void> {
    await new Promise<void>((resolve) => {
      game.onItemClicked = (item, modifier) => {
        if (item !== this.boosterPack) return;
        if (modifier === 1) this.isQuick = true;
        resolve();
      };
    });
    game.onItemClicked = null;
  }

  private showTutorial() {
    return ModalTutorial.show({
      key: 'OpenBoosterPack',
      paragraphs: [
        "You've just opened a booster pack! Each booster pack contains cards and/or more booster packs. Click each " +
          'pile to distribute them to the game.',
        'You will learn the details of each card as it becomes relevent.',
        '<strong>Tip:</strong> You can <strong>right-click</strong> or <strong>shift+click</strong> a booster pack ' +
          'to quickly distribute its contents.',
      ],
      left: 'calc(50vw - 200px)',
      width: '400px',
      bottom: `calc(50vh + 170px)`,
    });
  }

  private async exploreItems(): Promise<void> {
    const promiseQueue = new PromiseQueue();
    game.onItemClicked = (item, modifier) => promiseQueue.push(this.exploreItem(item, modifier));
    const boosterTray = game.containers.boosterTray;
    while (promiseQueue.length > 0 || boosterTray.items.length > 0) await promiseQueue.next();
    await promiseQueue.flush();
    game.onItemClicked = null;
  }

  private async exploreItem(item: Item, _modifier: number): Promise<void> {
    const boosterTray = game.containers.boosterTray;
    if (item.container !== boosterTray) return;
    const group = this.groups.find((group) => group.items.includes(item));
    if (!group) return;
    const items = group.items.filter((item2) => item2.name === item.name).reverse();
    await group.container.addItems(...items);
  }

  private async distributeAllItems(): Promise<void> {
    await Promise.all(this.groups!.map((group) => group.container.addItems(...group.items.reverse())));
  }
}
