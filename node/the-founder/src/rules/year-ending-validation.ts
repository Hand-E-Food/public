import { GameEvent, type YearEndingProperties } from '../events/index.js';
import { eventHub, type GameEventListener } from '../singleton/index.js';

export abstract class YearEndingValidation {
  /** The event listener for the YearEnding event. */
  public readonly listener: GameEventListener;

  protected constructor(priority: number) {
    this.listener = eventHub.add(GameEvent.YearEnding, priority, (props) => this.validate(props));
  }

  /**
   * Validates that the year can be ended.
   * @param props The YearEnding event properties.
   */
  protected abstract validate(props: YearEndingProperties): Promise<void>;
}
