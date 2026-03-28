import type { Container } from './containers/container.js';

/** One item, either a booster pack or a single card. */
export abstract class Item {
  /** The height of an item's title bar. */
  public static readonly titleHeight = 30;

  /** The animation transition time of an item in milliseconds. */
  public static readonly transitionTime = 500;

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

  /** The container currently holding this item. */
  public container: Container | undefined = undefined;

  /** This item's height. */
  public abstract readonly height: number;

  /** This item's HTML element. */
  public readonly htmlElement: HTMLDivElement = document.createElement('div');

  /** This item's name. */
  public abstract readonly name: string;

  /** This item's animation transition time in milliseconds. */
  public readonly transitionTime: number = Item.transitionTime;

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
    switch (event.button) {
      case 0:
        // Left click
        switch (keys) {
          case 0:
            return 0;
          case 1:
            return 1;
          case 2:
            return 2;
          case 4:
            return 3;
        }
        break;
      case 1:
        // Middle click
        switch (keys) {
          case 0:
            return 2;
        }
        break;
      case 2:
        // Right click
        switch (keys) {
          case 0:
            return 1;
        }
        break;
    }
    return undefined;
  }

  /**
   * Set this item's physical position.
   * @param left The x-coordinate of this item's left side.
   * @param top The y-coordinate of this item's top side.
   * @param zIndex This item's Z index. Higher numbers are on top.
   */
  public reposition(left: number | string, top: number | string, zIndex: number): void {
    this.htmlElement.style.left = typeof left === 'number' ? `${left}px` : left;
    this.htmlElement.style.top = typeof top === 'number' ? `${top}px` : top;
    setTimeout(() => (this.htmlElement.style.zIndex = `${zIndex}`), 250);
  }
}
