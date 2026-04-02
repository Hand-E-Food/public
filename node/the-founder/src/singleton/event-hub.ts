import type { Card } from '../cards/index.js';
import type { GameState } from '../states/primitive/game-state.js';
import { GameEvent } from './game-event.js';

const allEvents = Object.values(GameEvent).filter((x): x is GameEvent => typeof x === 'number') as GameEvent[];

/** Handles event listeners and invocation. */
export class EventHub {
  private readonly listeners = Object.fromEntries(
    allEvents.map((event) => [event, [] as { priority: number; listener: (...args: any[]) => any }[]]),
  ) as Record<GameEvent, { priority: number; listener: (...args: any[]) => any }[]>;

  public add(event: GameEvent.NewYear, priority: number, listener: () => void): void;
  public add(event: GameEvent.CardDrawn, priority: number, listener: (card: Card) => void): void;
  public add(event: GameEvent.CardPlayed, priority: number, listener: (card: Card) => void): void;
  public add(event: GameEvent.EndYear, priority: number, listener: () => GameState): void;
  /**
   * Adds an event listener.
   * @param event The event to listen to.
   * @param priority This listener's priority. Listeners with lower numbers are called first.
   * @param listener The function to call when the event is invoked.
   */
  public add(event: GameEvent, priority: number, listener: (...args: any[]) => any): void {
    this.listeners[event].push({ priority, listener });
    this.listeners[event].sort((a, b) => b.priority - a.priority);
  }

  public invoke(event: GameEvent.NewYear): void;
  /**
   * @param card The card that was drawn.
   */
  public invoke(event: GameEvent.CardDrawn, card: Card): void;
  /**
   * @param card The card that was played.
   */
  public invoke(event: GameEvent.CardPlayed, card: Card): void;
  /**
   * @returns The GameState to transition to, or undefined to transition to the default state.
   */
  public invoke(event: GameEvent.EndYear): GameState;
  /**
   * Invokes the listeners for the specified event, in order of priority.
   * If a listener returns a value other than undefined, the invocation is stopped and that value is returned.
   * @param event The event to invoke.
   * @param args The arguments to pass to the listeners.
   * @returns The value returned by the first listener that returns a value other than undefined, or undefined if all
   * listeners return undefined.
   */
  public invoke(event: GameEvent, ...args: any[]): any {
    for (const { listener } of this.listeners[event]) {
      const result = listener(...args);
      if (result !== undefined) return result;
    }
  }

  /**
   * Removes an event listener.
   * @param listener The listener function to remove.
   */
  public remove(listener: (...args: any[]) => any): void {
    for (const event of allEvents) {
      this.listeners[event] = this.listeners[event].filter((x) => x.listener !== listener);
    }
  }
}

/** A singleton instance of the EventHub. */
export const eventHub = new EventHub();
