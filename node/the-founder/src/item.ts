import type { Container } from './containers/container.js';
/** One item, either a booster pack or a single card. */
export abstract class Item {
  /** The default animation duration of an item in milliseconds. */
  public static readonly animationDuration = 500;

  /** The height of an item's title bar. */
  public static readonly titleHeight = 30;

  /**
   * Creates multiple copies of an item.
   * @param count The number of copies to include.
   * @param constructor The constructor to call.
   * @returns A list of `count` items created by `constructor`.
   */
  public static multiple(count: number, constructor: () => Item): Item[] {
    const items: Item[] = [];
    for (let i = 0; i < count; i++) items.push(constructor());
    return items;
  }

  /** This item's animation duration in milliseconds. */
  public readonly animationDuration: number = Item.animationDuration;

  /** The container currently holding this item. */
  public container: Container | undefined = undefined;

  /** This item's height. */
  public abstract readonly height: number;

  /** This item's HTML element. */
  public readonly htmlElement: HTMLDivElement = document.createElement('div');

  /** This item's name. */
  public abstract readonly name: string;

  /** This item's width. */
  public abstract readonly width: number;

  protected constructor() {
    this.htmlElement.onauxclick = (event) => this.onClicked(event);
    this.htmlElement.onclick = (event) => this.onClicked(event);
    this.htmlElement.oncontextmenu = (event) => event.preventDefault();
  }

  private onClicked(event: MouseEvent): void {
    const modifier = Item.getModifier(event);
    if (modifier === undefined) return;
    this.onClickedListener?.(this, modifier);
  }

  public onClickedListener: { (item: Item, modifier: number): void } | undefined;

  private static getModifier(event: MouseEvent): number | undefined {
    const keys = (event.shiftKey ? 1 : 0) | (event.ctrlKey ? 2 : 0) | (event.altKey ? 4 : 0);
    switch (keys) {
      case 0: // Click
        switch (event.button) {
          case 0: // Left click
            return 0;
          case 1: // Middle click
            return 2;
          case 2: // Right click
            return 1;
        }
      case 1: // Shift + click
        return 1;
      case 2: // Ctrl + click
        return 2;
      case 4: // Alt + click
        return 3;
    }
    return undefined;
  }

  /**
   * Moves this item to a new location.
   * @param to The location to move it to.
   * @param zIndex This item's new Z-index.
   */
  public async move(
    to: { left?: string; top?: string; width?: string; height?: string },
    zIndex?: number,
  ): Promise<void> {
    if (zIndex !== undefined) {
      setTimeout(() => {
        this.htmlElement.style.zIndex = `${zIndex}`;
      }, this.animationDuration / 2);
    }
    await this.htmlElement.animate([{}, to], { duration: this.animationDuration, fill: 'forwards' }).finished;
  }
}
