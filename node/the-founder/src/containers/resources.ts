import { type PayQuantities, Resource, resourceImage } from '../resource.js';
import { Spacing } from './constants.js';
import { DiscardPile } from './discard-pile.js';
import { DrawDeck } from './draw-deck.js';

export class Resources {
  public static readonly left = DrawDeck.left + DrawDeck.width + Spacing;
  public static readonly right = DiscardPile.right + DiscardPile.width + Spacing;
  public static readonly bottom = Spacing;
  public static readonly height = 40;

  /** This container's visual element. */
  public readonly htmlElement: HTMLDivElement;

  /** The produced resources. */
  public readonly resources = Object.fromEntries(
    Object.values(Resource)
      .filter((value): value is Resource => typeof value === 'number')
      .map((resource) => [resource, new ResourceData(resource)]),
  ) as Record<Resource, ResourceData>;

  public constructor() {
    const htmlElement = document.createElement('div');
    htmlElement.classList.add('resources');
    htmlElement.style.left = `${Resources.left}px`;
    htmlElement.style.right = `${Resources.right}px`;
    htmlElement.style.bottom = `${Resources.bottom}px`;
    htmlElement.style.height = `${Resources.height}px`;

    htmlElement.append(...Object.values(this.resources).map((resource) => resource.htmlElement));
    this.htmlElement = htmlElement;
  }

  /**
   * Checks if there are sufficient resources to pay the specified quantities, including using wild resources.
   * @param quantities The resources to spend.
   * @returns True if there are sufficient resources to pay the specified quantities.
   */
  public has(quantities: PayQuantities): boolean {
    let wild = 0;
    wild += this.hasResource(Resource.Food, quantities);
    wild += this.hasResource(Resource.Wood, quantities);
    wild += this.hasResource(Resource.Stone, quantities);
    wild = this.hasResource(Resource.Wild3, { [Resource.Wild3]: wild });
    wild += this.hasResource(Resource.Luxury, quantities);
    wild += this.hasResource(Resource.Faith, quantities);
    wild += this.hasResource(Resource.Arcane, quantities);
    wild = this.hasResource(Resource.Wild6, { [Resource.Wild6]: wild });
    return wild === 0;
  }
  /**
   * Measures how much wild resource is needed to cover the cost of a purchase.
   * @param resource The resource to measure.
   * @param quantities The amount of resources required.
   * @returns The amount of resource missing.
   */
  private hasResource(resource: Resource, quantities: Partial<Record<Resource, number>>): number {
    return Math.max(0, (quantities[resource] ?? 0) - this.resources[resource].quantity);
  }

  /**
   * Reduces the resource by the specified quantities, using wild resources as needed.
   * Assumes that there are sufficient resources to pay the cost.
   * @param quantities The quantities of each resource to spend.
   */
  public spend(quantities: PayQuantities): void {
    let wild = 0;
    wild += this.spendResource(Resource.Food, quantities);
    wild += this.spendResource(Resource.Wood, quantities);
    wild += this.spendResource(Resource.Stone, quantities);
    wild = this.spendResource(Resource.Wild3, { [Resource.Wild3]: wild });
    wild += this.spendResource(Resource.Luxury, quantities);
    wild += this.spendResource(Resource.Faith, quantities);
    wild += this.spendResource(Resource.Arcane, quantities);
    wild = this.spendResource(Resource.Wild6, { [Resource.Wild6]: wild });
    if (wild < 0) throw new Error('Spent more resources than available.');
  }
  /**
   * Measures how much wild resource is needed to cover the cost of a purchase.
   * @param resource The resource to measure.
   * @param quantities The amount of resources required.
   * @returns The amount of resource missing.
   */
  private spendResource(resource: Resource, quantities: Partial<Record<Resource, number>>): number {
    if (quantities[resource] === undefined) return 0;
    const resourceQuantity = this.resources[resource].quantity - quantities[resource];
    if (resourceQuantity < 0) {
      this.resources[resource].quantity = 0;
      return -resourceQuantity;
    } else {
      this.resources[resource].quantity = resourceQuantity;
      return 0;
    }
  }
}

class ResourceData {
  private readonly text: HTMLSpanElement;

  /** This component's visual element. */
  public readonly htmlElement: HTMLSpanElement;

  /**
   * Creates a new resource data instance.
   * @param resource The resource for which to display data.
   */
  constructor(resource: Resource) {
    const htmlElement = document.createElement('span');
    htmlElement.classList.add('resource', 'hidden');

    const text = document.createElement('span');
    text.innerText = '0';
    htmlElement.appendChild(text);

    htmlElement.appendChild(resourceImage(resource));

    this.htmlElement = htmlElement;
    this.text = text;
  }

  private _quantity = 0;
  /** Gets the quantity of the resource. */
  public get quantity(): number {
    return this._quantity;
  }
  /** Sets the quantity of the resource. */
  public set quantity(value: number) {
    if (value < this._quantity) {
      this.text.animate([{ color: '#ff0000' }, {}], { duration: 500 });
    } else if (value > this._quantity) {
      this.text.animate([{ color: '#0080ff' }, {}], { duration: 500 });
    }
    this.text.innerText = value.toString();
    this.htmlElement.classList.remove('hidden');
    this._quantity = value;
  }
}
