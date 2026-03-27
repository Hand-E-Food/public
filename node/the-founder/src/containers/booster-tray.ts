import { Container } from "./container.js";
import type { Item } from "../item.js";
import { Spacing } from "./constants.js";

export class BoosterTray extends Container {
    override addItems(...items: Item[]): void {
        super.addItems(...items);
        let zIndex = 900 + this.items.length;
        for (const item of items) {
            zIndex--;
            item.reposition(`calc(50vw - ${item.width / 2}px)`, `calc(50vh - ${item.height / 2}px)`, zIndex);
        }
    }

    protected arrange(): void {
        // Handle arrangement in `addItems` and `spreadItems` methods. `removeItem` should not rearrange the remaining items.
    }

    public spreadItems(): void {
        // Group items of the same type together. Spread out each group.
        const totalWidth = this.items.reduce(
            (width, item, index, array) => width + Spacing + (typeof array[index - 1] === typeof item ? 0 : item.width),
            -Spacing);
        let left = -totalWidth / 2;
        for (const item of this.items) {
            item.htmlElement.style.left = `calc(50vw - ${left}px)`;
            left += item.width + Spacing;
        }
    }
}
