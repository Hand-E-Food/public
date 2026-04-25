import { game } from '../singleton/index.js';

/** Primitive, common game states. */
export class Primitive {
  public static readonly defaultDuration = 500;

  /**
   * Adds the specified element to the game area and fades it in.
   * @param element The HTML element to fade in.
   * @param duration The duration of the fade in milliseconds.
   */
  public static async fadeIn(element: HTMLElement, duration: number = Primitive.defaultDuration): Promise<void> {
    element.style.opacity = '0%';
    game.htmlElement.appendChild(element);
    await element.animate([{}, { opacity: '100%' }], { duration, fill: 'forwards' }).finished;
  }

  /**
   * Fades out the specified element and removes it from the game area.
   * @param element The HTML element to fade out.
   * @param duration The duration of the fade out milliseconds.
   */
  public static async fadeOut(element: HTMLElement, duration: number = Primitive.defaultDuration): Promise<void> {
    await element.animate([{}, { opacity: '0%' }], { duration, fill: 'forwards' }).finished;
    game.htmlElement.removeChild(element);
  }

  /**
   * Causes the specified element to glow briefly.
   * @param element The HTML element to glow.
   * @param duration The duration of the glow.
   */
  public static async glow(element: HTMLElement, duration: number = Primitive.defaultDuration): Promise<void> {
    await element.animate([{}, { filter: 'drop-shadow(0 0 10px gold)' }], {
      direction: 'alternate',
      duration: duration / 2,
      iterations: 2,
    }).finished;
  }

  /**
   * Pauses for a duration.
   * @param milliseconds The duration to pause in milliseconds.
   */
  public static pause(milliseconds: number = Primitive.defaultDuration): Promise<void> {
    return new Promise((resolve) => setTimeout(resolve, milliseconds));
  }
}
