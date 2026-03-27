import { game } from "../game.js";
import { Item } from "../item.js";

export interface BoosterPackParams {
    readonly image: string,
    readonly name?: string,
}

/** A booster pack containing more items. */
export abstract class BoosterPack extends Item {
    public static readonly height = 270;
    public static readonly width = 170;

    public override readonly height = BoosterPack.height;
    public override readonly width = BoosterPack.width;

    public constructor(params: BoosterPackParams) {
        super();
        this.htmlElement.classList.add('item', 'booster', 'side');
        let innerHtml = `<img src="assets/${params.image}" />`;
        if (params.name) innerHtml += `<span class='title'>${params.name}</span>`;
        this.htmlElement.innerHTML += innerHtml;
    }

    public open(): void {
        const items = this.createItems();
        game.addItems(game.containers.boosterTray, ...items);
    }

    protected abstract createItems(): Item[];
}
