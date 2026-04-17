import { NegativeStack } from '../containers/index.js';
import { type YearEndingProperties } from '../events/index.js';
import { eventHub, game, tutorials } from '../singleton/index.js';
import { ModalTutorial } from '../states/index.js';
import { YearEndingValidation } from './year-ending-validation.js';

export class AllFamiliesFedValidation extends YearEndingValidation {
  public constructor() {
    super(10);
  }

  public override async validate(props: YearEndingProperties): Promise<void> {
    const key = 'HungryFamily';
    if (!tutorials.shouldShow(key)) {
      eventHub.remove(this.listener);
      return;
    }
    if (!game.containers.negativeStack.items.find((item) => item.activeSide.name === 'Hungry Family')) return;
    await ModalTutorial.show({
      key,
      paragraphs: [
        'Not all of your townsfolk have been fed. Only fed families will be productive for the community. ' +
          'Hungry families will spend the year taking care of their own needs and not contribute.',
        'Feed as many families as you can each year to maximize productivity.',
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
