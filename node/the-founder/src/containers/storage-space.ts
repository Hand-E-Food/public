import { Card } from '../cards/index.js';
import { Item } from '../item.js';
import { ModalTutorial } from '../states/modal-tutorial.js';
import { Spacing } from './constants.js';
import { Container } from './container.js';
import { StorageItems } from './storage-items.js';
import { ZIndex } from './z-index.js';

/** The constructed storage facilities. */
export class StorageSpace extends Container {
  public static readonly left = StorageItems.left;
  public static readonly bottom = StorageItems.bottom + Item.titleHeight;
  public static readonly right = StorageItems.right;
  public static readonly height = Card.height;

  protected override async arrange(): Promise<void> {
    const promises: Promise<void>[] = [];
    const step = Card.width + Spacing / 2;
    let left = -StorageSpace.right - Card.width;
    const top = -StorageSpace.bottom - StorageSpace.height;
    let zIndex = ZIndex.LowerStack;
    for (const item of this.items) {
      promises.push(item.move({ left: `calc(100vw + ${left}px)`, top: `calc(100vh + ${top}px)` }, zIndex));
      left -= step;
      zIndex++;
    }
    await Promise.all(promises);
    if (this.items.length > 0) {
      await ModalTutorial.show({
        key: 'StorageSpace',
        paragraphs: [
          'You can now store cards from one year to the next. Each storage card allows you to store any one card ' +
            'from your hand.',
          'Once a card is in storage, it can be used jsut like it was in your hand, or it can be discarded to free ' +
            'up space for a different card.',
          'Click on the card and choose the action to store it or discard it. You can also use ' +
            '<strong>middle-click</strong> or <strong>ctrl+click</strong> as a shortcut.',
        ],
        bottom: `${StorageSpace.bottom + StorageSpace.height + Spacing}px`,
        right: `${StorageSpace.right}px`,
        width: '300px',
      });
    }
  }
}
