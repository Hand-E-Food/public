import type { Container } from "./containers/container.js";

/** One item, either a booster pack or a single card. */
export abstract class Item {
  public static readonly titleHeight = 30;

  /** The container currently holding this item. */
  public container: Container | undefined = undefined;

  /** This item's HTML element. */
  public readonly htmlElement: HTMLDivElement;

    /** This item's width. */
    public abstract readonly width: number;

    /** This item's height. */
    public abstract readonly height: number;

    /**
     * Creates a new item.
     */
    public constructor() {
      this.htmlElement = document.createElement("div");
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
      setTimeout(() => this.htmlElement.style.zIndex = `${zIndex}`, 250);
    }
}
