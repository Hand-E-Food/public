import { Card } from '../cards/common/index.js';
import { Container } from '../container.js';
import { game } from '../singleton/index.js';
import { Spacing } from './constants.js';
import { Hand } from './hand.js';
import { ZIndex } from './z-index.js';

/** The items held in storage between years. */
export class StorageItems extends Container {
  private readonly rules: ((card: Card) => boolean)[] = [
    (card) => this.hasAvailableSpace(card),
    (card) => this.isInHand(card),
  ];

  public static readonly left = Hand.left;
  public static readonly bottom = Hand.bottom;
  public static readonly right = Hand.right;
  public static readonly height = Card.height;

  protected override async arrange(): Promise<void> {
    const promises: Promise<void>[] = [];
    const step = Card.width + Spacing / 2;
    let left = -StorageItems.right - Card.width;
    const top = -StorageItems.bottom - StorageItems.height;
    let zIndex = ZIndex.UpperStack;
    for (const item of this.items) {
      promises.push(item.move({ left: `calc(100vw + ${left}px)`, top: `calc(100vh + ${top}px)` }, zIndex));
      left -= step;
      zIndex++;
    }
    await Promise.all(promises);
  }

  /**
   * Checks whether the specified card can be stored.
   * @param card The card to store.
   * @returns True if the card can be stored.
   */
  public canStore(card: Card): boolean {
    return this.rules.every((rule) => rule(card));
  }

  private isInHand(card: Card): boolean {
    return card.container === game.containers.hand;
  }

  private hasAvailableSpace(_card: Card): boolean {
    const totalSpace = game.containers.storageSpace.items.length;
    const usedSpace = this.items.length;
    return totalSpace > usedSpace;
  }

  /**
   * Adds a new rule to validate whether a card can be stored.
   * @param rule The rule to apply.
   */
  public addRule(rule: (card: Card) => boolean): void {
    this.rules.push(rule);
  }

  /**
   * Revokes a previously added rule.
   * @param rule The rule to revoke.
   */
  public revokeRule(rule: (card: Card) => boolean): void {
    for (let i = this.rules.length - 1; i >= 0; i--) {
      if (this.rules[i] === rule) this.rules.splice(i, 1);
    }
  }
}
