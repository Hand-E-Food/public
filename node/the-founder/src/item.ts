import type { Container } from './containers/container.js';

/** One item, either a booster pack or a single card. */
export abstract class Item {
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
