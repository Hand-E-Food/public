export enum Resource {
  Food,
  Wood,
  Stone,
  Luxury,
  Faith,
  Arcane,
  Wild3,
  Wild6,
}

const alts: { [key in Resource]?: string } = {
  [Resource.Wild3]: 'Any primary resource',
  [Resource.Wild6]: 'Any resource',
};

export function formatQuantities(quantities: Partial<Record<Resource, number>>): string {
  const parts: string[] = [];
  for (const [resource, quantity] of Object.entries(quantities)) {
    if (quantity) parts.push(`${quantity} ${resourceImage(Number(resource) as Resource, 'inline').outerHTML}`);
  }
  return parts.join(', ');
}

export function resourceImage(resource: Resource, ...classList: string[]): HTMLImageElement {
  const img = document.createElement('img');
  img.src = `assets/icons/${Resource[resource].toLowerCase()}.svg`;
  img.alt = alts[resource] || Resource[resource];
  if (classList.length > 0) img.classList.add(...classList);
  return img;
}

export type PayQuantities = Partial<Omit<Record<Resource, number>, Resource.Wild3 | Resource.Wild6>>;
export type ProduceQuantities = Partial<Record<Resource, number>>;
