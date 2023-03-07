import { Human } from './brain';
import { BrainFactory } from './brain-factory';
import { ConsoleView } from './console-view';
import { Engine } from './engine';
import { GameFactory } from './game-factory';
import { Player } from './model';
import { View } from './view';

async function main(): Promise<void> {
    let view: View | undefined;
    try {
        view = new ConsoleView();
        const brainFactory = new BrainFactory();
        const player1 = new Player('Human', new Human(view));
        const player2 = new Player('CPU', brainFactory.createCpuBrain());
        const gameFactory = new GameFactory();
        const game = gameFactory.createGame([player1, player2], false);
        const engine = new Engine(game);
        view.setPlayer(player1);
        const winner = await engine.start();
        view.showGame(game.getPublicKnowledge(true));
        view.showWinner(winner);
    } catch (e) {
        process.stdout.write(`${e}`);
    } finally {
        if (view) view.dispose();
    }
}

main();
