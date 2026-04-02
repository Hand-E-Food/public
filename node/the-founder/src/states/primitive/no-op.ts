import { Instant } from './instant.js';

/** Does nothing. Useful as a counterpart to an optional state so that following logic can go in `resume`. */
export class NoOp extends Instant {
  public readonly name: string = 'NoOp';

  override execute(): void {}
}
