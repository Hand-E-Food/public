import { Family, PositiveCard } from './cards/index.js';
import { game } from './game.js';

document.body.appendChild(game.htmlElement);
game.addItems(
  game.containers.positiveStack,
  new PositiveCard({ image: 'town-square.avif', name: 'Town Square' }),
  new PositiveCard({ image: 'fishery.jpg', name: 'Fishery' }),
  new PositiveCard({ image: 'logger.jpg', name: 'Logger' }),
);
game.addItems(game.containers.negativeStack, new Family(), new Family(), new Family());
