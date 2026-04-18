import { GameEvent, type YearEndingProperties } from '../events/index.js';
import type { Item } from '../item.js';
import { eventHub, game } from '../singleton/index.js';
import { ModalInspectItem } from './index.js';
import { ManualPromise } from './manual-promise.js';

/** The main part of the player's turn. */
export class PlayerPhase {
  private endYearPromise!: ManualPromise<void>;

  /** The "End Year" button. */
  public readonly endYearButton: HTMLDivElement;

  public constructor() {
    const endYearButton = document.createElement('div');
    endYearButton.classList.add('end-year');
    endYearButton.innerHTML = 'End Year';
    this.endYearButton = endYearButton;
  }

  /** Executes this state. */
  public async execute(): Promise<void> {
    this.endYearPromise = new ManualPromise<void>();
    this.enable();
    await this.endYearPromise;
    this.disable();
  }

  /** Configure to respond to inputs. */
  private enable(): void {
    this.endYearButton.onclick = () => this.onEndYearClicked();
    game.onItemClicked = (item, modifier) => this.onItemClicked(item, modifier);
  }

  /** De-configure to ignore inputs. */
  private disable(): void {
    game.onItemClicked = null;
    this.endYearButton.onclick = null;
  }

  /**
   * Responds to an item being clicked.
   * @param _item The clicked item.
   * @param _modifier The active modifier when the item was clicked.
   */
  private async onItemClicked(item: Item, modifier: number): Promise<void> {
    let execute: () => Promise<void>;
    if (modifier === 0) {
      if (!item.activeSide.canInspect) return;
      this.disable();
      execute = () => new ModalInspectItem({ item }).execute();
      this.enable();
    } else {
      const action = item.activeSide.actions[modifier - 1];
      if (action?.state !== 'enabled') return;
      execute = () => action.execute(item);
    }
    this.disable();
    await execute();
    this.enable();
  }

  private async onEndYearClicked(): Promise<void> {
    const props: YearEndingProperties = { stop: false, cancel: false };
    await eventHub.invoke(GameEvent.YearEnding, props);
    if (!props.cancel) this.endYearPromise.resolve();
  }
}
