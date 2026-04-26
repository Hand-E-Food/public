import { NegativeStack } from '../containers/index.js';
import { type YearEndingProperties } from '../events/index.js';
import { eventHub, game, tutorials } from '../singleton/index.js';
import { ModalTutorial } from '../states/index.js';
import { YearEndingValidation } from './year-ending-validation.js';

export class NegativeMoraleValidation extends YearEndingValidation {
  public constructor() {
    super(20);
  }

  public override async validate(props: YearEndingProperties): Promise<void> {
    const key = 'NegativeMorale';
    if (!tutorials.shouldShow(key)) {
      eventHub.remove(this.listener);
      return;
    }
    if (game.containers.negativeStack.morale <= game.containers.positiveStack.morale) return;
    await ModalTutorial.show({
      key,
      paragraphs: [
        'Your town has more negative morale than positive morale. If you end the year without addressing enough ' +
          'negative morale, you will be run out of town.',
        'Check each negative morale to see what you can do to satisfy your townsfolk.',
      ],
      left: `${NegativeStack.left + NegativeStack.width + 10}px`,
      top: `${10 + 210 / 2}px`,
      width: `200px`,
    });
    props.cancel = true;
    props.stop = true;
    eventHub.remove(this.listener);
  }
}
