import { Item } from '../../item.js';
import { game, stateMachine } from '../../singleton/index.js';
import { AwaitTime } from './await-time.js';
import { type GameState } from './game-state.js';

/** Adds a HTML element to the game and fades it in. */
export class ShowElement implements GameState {
  public readonly name: string;

  /**
   * Creates an animation to show a HTML element.
   * @param htmlElement The HTML element to show.
   */
  public constructor(private readonly htmlElement: HTMLElement) {
    this.name = `ShowElement(${htmlElement.tagName})`;
  }

  enter(): void {
    this.htmlElement.style.opacity = '0%';
    game.htmlElement.appendChild(this.htmlElement);
    requestAnimationFrame(() => this.fadeIn());
  }

  private fadeIn(): void {
    this.htmlElement.style.opacity = '100%';
    stateMachine.next(new AwaitTime(Item.transitionTime));
  }
}
