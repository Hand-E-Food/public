import { argv } from 'process';
import { AutoAuthor, DummyAuthor } from './author';
import { BookPrinter } from './book-printer';
import { Human } from './brain';
import { BrainFactory } from './brain-factory';
import { Engine } from './engine';
import { GameFactory } from './game-factory';
import { LlmClient, OllamaClient } from './llm';
import { Book, Names, Player } from './model';
import { ConsoleView, View } from './view';
import { promises as fs } from 'fs';
import path from 'path';

function getModel(): Promise<string> {
    return fs.readFile(path.join(__dirname, 'model.txt'), { encoding: 'utf8' });
}

function getTimestamp(): string {
    const now = new Date();

    const year = now.getFullYear();
    const month = String(now.getMonth() + 1).padStart(2, '0');
    const day = String(now.getDate()).padStart(2, '0');
    const hours = String(now.getHours()).padStart(2, '0');
    const minutes = String(now.getMinutes()).padStart(2, '0');

    return `${year}-${month}-${day}-${hours}-${minutes}`;
}

async function main(name1?: string, name2?: string): Promise<void> {
    let llmClient: LlmClient | undefined;
    let view: View | undefined;
    try {
        if (!name1) {
            do name1 = Names.getRandom();
            while (name1 === name2);
        }
        if (!name2) {
            do name2 = Names.getRandom();
            while (name1 === name2);
        }
        
        const model = await getModel();
        llmClient = new OllamaClient(model);
        console.log('Warning up LLM...');
        await llmClient.warmup();
        view = new ConsoleView();
        const brainFactory = new BrainFactory(view);

        const book1 = new Book(name1);
        //const author1 = new DummyAuthor({ book: book1, characterName: name2 });
        const author1 = new AutoAuthor({
            book: book1,
            characterName: name2,
            genre: undefined,
            llmClient,
            style: undefined,
        });
        const brain1 = brainFactory.createCpuBrain(author1, 6);
        //const brain1 = new Human(book1, view);
        const player1 = new Player(brain1);

        const book2 = new Book(name2);
        //const author2 = new DummyAuthor(book2);
        const author2 = new AutoAuthor({
            book: book2,
            characterName: name1,
            genre: undefined,
            llmClient,
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
        llmClient?.dispose();
        view?.dispose();
    }
}

main(argv[2], argv[3]).catch(e => {
    console.error(e);
});
