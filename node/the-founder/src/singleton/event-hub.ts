import type { Card } from '../cards/index.js';
import type { EndYearProperties } from '../states/end-year.js';
import { type GameState, Sequence } from '../states/primitive/index.js';
import { GameEvent } from './game-event.js';

export type GameEventListener = {
  readonly event: GameEvent;
  readonly priority: number;
  readonly createState: (...args: any[]) => GameState;
};

/** Handles event listeners and invocation. */
export class EventHub {
  private readonly listeners: GameEventListener[] = [];

  public add(event: GameEvent.NewYear, priority: number, createState: () => GameState): GameEventListener;
  public add(event: GameEvent.CardDrawn, priority: number, createState: (card: Card) => GameState): GameEventListener;
  public add(event: GameEvent.CardPlayed, priority: number, createState: (card: Card) => GameState): GameEventListener;
  public add(
    event: GameEvent.EndYear,
    priority: number,
    createState: (props: EndYearProperties) => GameState,
  ): GameEventListener;
  /**
   * Adds an event listener.
   * @param event The event to listen to.
   * @param priority This listener's priority. Listeners with lower numbers are entered first.
   * @param createState A function that creates a GameState to enter when the event is invoked.
   * @returns An object representing the listener, which can be used to remove it later.
   */
  public add(event: GameEvent, priority: number, createState: (...args: any[]) => GameState): GameEventListener {
    const listener = { event, priority, createState };
    this.listeners.push(listener);
    this.listeners.sort((a, b) => b.priority - a.priority);
    return listener;
  }

  public invoke(event: GameEvent.NewYear): GameState;
  /**
   * @param card The card that was drawn.
   */
  public invoke(event: GameEvent.CardDrawn, card: Card): GameState;
  /**
   * @param card The card that was played.
   */
  public invoke(event: GameEvent.CardPlayed, card: Card): GameState;
  /**
   * @param props The properties for the end-of-year event.
   */
  public invoke(event: GameEvent.EndYear, props: EndYearProperties): GameState;
  /**
   * Invokes the listeners for the specified event, in order of priority.
   * If a listener returns a value other than undefined, the invocation is stopped and that value is returned.
   * @param event The event to invoke.
   * @param args The arguments to pass to the listeners.
   * @returns The value returned by the first listener that returns a value other than undefined, or undefined if all
   * listeners return undefined.
   */
  public invoke(event: GameEvent, ...args: any[]): GameState {
    return new Sequence(this.listeners.filter((x) => x.event === event).map((x) => x.createState(...args)));
  }

  /**
   * Removes event listeners.
   * @param listeners The listeners to remove.
   */
  public remove(listeners: GameEventListener | GameEventListener[]): void {
    if (!Array.isArray(listeners)) listeners = [listeners];
    for (let i = this.listeners.length - 1; i >= 0; i--) {
      const listener = this.listeners[i];
      if (listeners.includes(listener!)) this.listeners.splice(i, 1);
    }
  }
}

/** A singleton instance of the EventHub. */
export const eventHub = new EventHub();
