import type { BoosterPack } from '../boosters/index.js';
import type { Item } from '../item.js';
import { Spacing } from './constants.js';
import { Container } from './container.js';
import { ZIndex } from './z-index.js';

export class BoosterTray extends Container {
  public readonly htmlElement: HTMLDivElement;

  public constructor() {
    super();
    this.htmlElement = document.createElement('div');
    this.htmlElement.classList.add('modal', 'booster-tray');
  }

  public async addBoosterPack(item: BoosterPack): Promise<void> {
    const promises: Promise<void>[] = [super.addItems(item)];
    const zIndex = ZIndex.Overlay + 99;
    promises.push(
      item.move({ left: `calc(50vw - ${item.width / 2}px)`, top: `calc(50vh - ${item.height / 2}px)` }, zIndex),
    );
    await Promise.all(promises);
  }

  override async addItems(...items: Item[]): Promise<void> {
    const promises: Promise<void>[] = [super.addItems(...items)];
    let zIndex = ZIndex.Overlay + this.items.length + items.length;
    for (const item of items) {
      promises.push(
        item.move({ left: `calc(50vw - ${item.width / 2}px)`, top: `calc(50vh - ${item.height / 2}px)` }, zIndex),
      );
      zIndex--;
    }
    await Promise.all(promises);
  }

  protected async arrange(): Promise<void> {
    // Handle arrangement in `addItems` and `spreadItems` methods.
    // `removeItem` should not rearrange the remaining items.
  }

  public async spreadItems(): Promise<void> {
    const promises: Promise<void>[] = [];
    // Group items of the same type together. Spread out each group.
    const totalWidth = this.items.reduce(
      (width, item, index, array) => width + Spacing + (array[index - 1]?.name === item.name ? 0 : item.width),
      -Spacing,
    );
    let left = -totalWidth / 2;
    let prevName: string = '';
    for (const item of this.items) {
      if (prevName) left += Spacing + (item.name === prevName ? 0 : item.width);
      promises.push(item.move({ left: `calc(50vw + ${left}px)` }));
      prevName = item.name;
    }
    await Promise.all(promises);
  }
}
