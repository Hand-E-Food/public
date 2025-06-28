import { argv } from 'process';
import { Human } from './brain';
import { BrainFactory } from './brain-factory';
import { ConsoleView } from './console-view';
import { Engine } from './engine';
import { GameFactory } from './game-factory';
import { Book, Player } from './model';
import { View } from './view';
import { Author } from './author';
import { Ollama } from 'ollama';

async function main(name1: string = 'Human'): Promise<void> {
    let view: View | undefined;
    try {
        view = new ConsoleView();
        const brainFactory = new BrainFactory(view);
        const ollama = new Ollama();

        const book1 = new Book(name1);
        const brain1 = new Human(book1, view);
        const player1 = new Player(brain1);

        const book2 = new Book('AuthorBot');
        const author2 = new Author(ollama, {
            book: book2,
            characterName: name1,
            genre: undefined,
            style: undefined,
        });
        const brain2 = brainFactory.createCpuBrain(author2, 6);
        const player2 = new Player(brain2);

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

main(argv[2]);
