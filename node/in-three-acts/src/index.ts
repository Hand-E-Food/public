import { Ollama } from 'ollama';
import { argv } from 'process';
import { AutoAuthor, DummyAuthor } from './author';
import { BookPrinter } from './book-printer';
import { Human } from './brain';
import { BrainFactory } from './brain-factory';
import { ConsoleView } from './console-view';
import { Engine } from './engine';
import { GameFactory } from './game-factory';
import { Book, Player } from './model';
import { View } from './view';

function getTimestamp(): string {
    const now = new Date();

    const year = now.getFullYear();
    const month = String(now.getMonth() + 1).padStart(2, '0');
    const day = String(now.getDate()).padStart(2, '0');
    const hours = String(now.getHours()).padStart(2, '0');
    const minutes = String(now.getMinutes()).padStart(2, '0');

    return `${year}-${month}-${day}-${hours}-${minutes}`;
}

async function main(name1: string = 'Human', name2: string = 'AuthorBot'): Promise<void> {
    let view: View | undefined;
    try {
        view = new ConsoleView();
        const brainFactory = new BrainFactory(view);
        const ollama = new Ollama();

        const book1 = new Book(name1);
        //const author1 = new DummyAuthor({ book: book1, characterName: name2 });
        const author1 = new AutoAuthor(ollama, {
            book: book1,
            characterName: name2,
            genre: undefined,
            style: undefined,
        });
        const brain1 = brainFactory.createCpuBrain(author1, 6);
        //const brain1 = new Human(book1, view);
        const player1 = new Player(brain1);

        const book2 = new Book(name2);
        //const author2 = new DummyAuthor(book2);
        const author2 = new AutoAuthor(ollama, {
            book: book2,
            characterName: name1,
            genre: undefined,
            style: undefined,
        });
        const brain2 = brainFactory.createCpuBrain(author2, 6);
        const player2 = new Player(brain2);

        const players = [player1, player2];
        const gameFactory = new GameFactory();
        const game = gameFactory.createGame(players, false);
        const engine = new Engine(game);
        view.startGame();
        view.setPlayer(player1);
        const winner = await engine.start();
        view.showGame(game.getPublicKnowledge(true));
        view.showWinner(winner);
        const bookPrinter = new BookPrinter();
        const timestamp = getTimestamp();
        for (const player of players) {
            bookPrinter.printBook(player.book, `${timestamp} ${player.book.authorName}.md`);
        }
        await view.waitForClose();
    } finally {
        if (view) view.dispose();
    }
}

main(argv[2], argv[3]).catch(e => {
    console.error(e);
});
