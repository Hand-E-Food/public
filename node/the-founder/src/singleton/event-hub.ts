import type {
  CardDrawnProperties,
  CardPlayedProperties,
  EndYearProperties,
  GameEvent,
  GameEventProperties,
  NewYearProperties,
} from '../events/index.js';

export type GameEventListener = {
  readonly event: GameEvent;
  readonly priority: number;
  readonly execute: Execute<any>;
};

type Execute<T extends GameEventProperties> = (props: T) => Promise<void>;

/** Handles event listeners and invocation. */
export class EventHub {
  private readonly listeners: GameEventListener[] = [];

  public add(
    event: GameEvent.NewYear,
    priority: number,
    execute: Execute<GameEventProperties & NewYearProperties>,
  ): GameEventListener;
  public add(
    event: GameEvent.CardDrawn,
    priority: number,
    execute: Execute<GameEventProperties & CardDrawnProperties>,
  ): GameEventListener;
  public add(
    event: GameEvent.CardPlayed,
    priority: number,
    execute: Execute<GameEventProperties & CardPlayedProperties>,
  ): GameEventListener;
  public add(
    event: GameEvent.EndYear,
    priority: number,
    execute: Execute<GameEventProperties & EndYearProperties>,
  ): GameEventListener;
  /**
   * Adds an event listener.
   * @param event The event to listen to.
   * @param priority This listener's priority. Listeners with lower numbers are entered first.
   * @param createState A function that creates a GameState to enter when the event is invoked.
   * @returns An object representing the listener, which can be used to remove it later.
   */
  public add<T extends GameEventProperties>(
    event: GameEvent,
    priority: number,
    execute: Execute<T>,
  ): GameEventListener {
    const listener: GameEventListener = { event, priority, execute };
    this.listeners.push(listener);
    this.listeners.sort((a, b) => b.priority - a.priority);
    return listener;
  }

  public async invoke(event: GameEvent.NewYear, props: NewYearProperties): Promise<void>;
  public async invoke(event: GameEvent.CardDrawn, props: CardDrawnProperties): Promise<void>;
  public async invoke(event: GameEvent.CardPlayed, props: CardPlayedProperties): Promise<void>;
  public async invoke(event: GameEvent.EndYear, props: EndYearProperties): Promise<void>;
  /**
   * Invokes the listeners for the specified event, in order of priority.
   * @param event The event to invoke.
   * @param props The event's properties.
   */
  public async invoke(event: GameEvent, props: any): Promise<void> {
    props = { stop: false, ...props };
    for (const listener of this.listeners) {
      if (listener.event === event) {
        await listener.execute(props);
        if (props.stop) break;
      }
    }
  }

  /**
   * Removes event listeners.
   * @param listeners The listeners to remove.
   */
  public remove(...listeners: GameEventListener[]): void {
    for (let i = this.listeners.length - 1; i >= 0; i--) {
      const listener = this.listeners[i];
      if (listeners.includes(listener!)) this.listeners.splice(i, 1);
    }
  }
}

/** A singleton instance of the EventHub. */
export const eventHub = new EventHub();
