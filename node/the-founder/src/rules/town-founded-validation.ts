import type { YearEndingProperties } from '../events/index.js';
import { game } from '../singleton/index.js';
import { YearEndingValidation } from './year-ending-validation.js';

export class TownFoundedValidation extends YearEndingValidation {
  public constructor() {
    super(0);
  }

  override async validate(props: YearEndingProperties): Promise<void> {
    if (game.containers.positiveStack.items.length > 0) return;
    props.cancel = true;
    props.stop = true;
  }
}
