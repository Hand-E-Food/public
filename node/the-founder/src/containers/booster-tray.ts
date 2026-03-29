import type { Item } from '../item.js';
import { Spacing } from './constants.js';
import { Container } from './container.js';
import { ZIndex } from './z-index.js';

export class BoosterTray extends Container {
  public readonly htmlElement: HTMLDivElement;

  public constructor() {
    super();
    this.htmlElement = document.createElement('div');
    this.htmlElement.classList.add('fade', 'booster-tray');
  }

  override addItems(items: Item[]): void {
    super.addItems(items);
    let zIndex = ZIndex.Overlay + this.items.length;
    for (const item of items) {
      item.reposition(`calc(50vw - ${item.width / 2}px)`, `calc(50vh - ${item.height / 2}px)`, zIndex);
      zIndex--;
    }
  }

  protected arrange(): void {
    // Handle arrangement in `addItems` and `spreadItems` methods.
    // `removeItem` should not rearrange the remaining items.
  }

  public spreadItems(): void {
    // Group items of the same type together. Spread out each group.
    const totalWidth = this.items.reduce(
      (width, item, index, array) => width + Spacing + (array[index - 1]?.name === item.name ? 0 : item.width),
      -Spacing,
    );
    let left = -totalWidth / 2;
    let prevName: string = '';
    for (const item of this.items) {
      if (prevName) left += Spacing + (item.name === prevName ? 0 : item.width);
      item.htmlElement.style.left = `calc(50vw + ${left}px)`;
      prevName = item.name;
    }
  }
}
