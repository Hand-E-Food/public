const ICON = {
    AMBULANCE: '🟥',
    BOOST: '➕',
    EVACUEE: '🚗',
    FIRE: '🔥',
    POLICE: '🔷',
};

const suburbs = [
    'Asandale',
    'Barrish',
    'Cadbridge',
    'Diamond',
    'Diamond',
    'East Diamond',
    'Falls',
    'Falls',
    'Gallant',
    'Houston',
    'Ington',
    'Ington',
    'Ington',
    'Jewel Beach',
    'Kingside',
    'Kingside',
    'Kingside',
    'Lower Rilden',
    'Lower Rilden',
    'Metro Centre',
    'Metro Centre',
    'Metro Centre',
    'North Tuskan',
    'North Tuskan',
    'Port Pelican',
    'Queenside',
    'Rilden Park',
    'Rilden Park',
    'Soulville',
    'Tuskan',
    'Upper Zenith',
    'West Xenia',
    'Xenia',
    'Xenia',
    'Yellowmoose',
    'Zenith',
];

class Button {
    node;

    onclick;

    constructor(text) {
        let span = document.createElement('span');
        span.appendChild(document.createTextNode(text));
        this.node = document.createElement('div');
        this.node.classList.add('cell', 'button');
        this.node.appendChild(span);
        this.node.onclick = (e) => this.onclick ? this.onclick(e) : true;
    }
}

class Card {
    node;

    onclick;

    constructor(text, cardType) {
        let span = document.createElement('span');
        span.appendChild(document.createTextNode(text));
        this.node = document.createElement('div');
        this.node.classList.add('cell', 'card', cardType);
        this.node.appendChild(span);
        this.node.onclick = (e) => this.onclick ? this.onclick(e) : true;
    }
}

class HorizontalLine {
    node;

    constructor() {
        this.node = document.createElement('div');
        this.node.classList.add('cell', 'line');
    };
}

class CardPile {
    cards = [];
    node;

    constructor() {
        this.node = document.createElement('div');
        this.node.classList.add('panel');
    }

    addCards(cards) {
        cards.forEach(card => {
            this.node.insertBefore(card.node, this.node.firstChild);
            this.cards.push(card);
        });

        let line = new HorizontalLine();
        this.node.insertBefore(line.node, this.node.firstChild);
    }

    clearCards() {
        let result = this.cards;
        this.cards = [];
        while (this.node.firstChild) {
            this.node.firstChild.remove();
        }
    }
}

class Controls {
    node;

    onNewGame;
    onClearCards;
    onDrawCards;

    constructor() {
        this.node = document.createElement('div');
        this.node.classList.add('panel');

        let newGameButton = new Button('New Game');
        newGameButton.onclick = (e) => this.onNewGame ? this.onNewGame(e) : true;
        this.node.appendChild(newGameButton.node);

        let clearCardsButton = new Button('Clear Cards');
        clearCardsButton.onclick = (e) => this.onClearCards ? this.onClearCards(e) : true;
        this.node.appendChild(clearCardsButton.node);

        let drawCardsButton = new Button('Draw Cards');
        drawCardsButton.onclick = (e) => this.onDrawCards ? this.onDrawCards(e) : true;
        this.node.appendChild(drawCardsButton.node);
    }
}

class Game {
    cardDeck;
    cardPile;

    constructor() {
        let body = document.getElementById('body');

        let controls = new Controls();
        body.appendChild(controls.node);

        this.cardPile = new CardPile();
        body.appendChild(this.cardPile.node);

        controls.onNewGame = (e) => this.newGameClicked();
        controls.onClearCards = (e) => this.cardPile.clearCards();
        controls.onDrawCards = (e) => this.drawCards(4);

        this.newGame();
    }

    newGameClicked() {
        if (confirm('Start a new game?')) {
            this.newGame();
        }
    }

    newGame() {
        this.cardPile.clearCards();
        
        let decks = [
            this.generateEvacueeCards(),
            this.generateDangerCards(),
            this.generateStormCards(),
        ];

        decks = decks.map(shuffle);

        decks = decks.map(deck => divide(deck, 3));
        
        decks = [
            [ ...decks[0][0], ...decks[0][1], ...decks[1][0] ],
            [ ...decks[0][2], ...decks[1][1], ...decks[2][0] ],
            [ ...decks[1][2], ...decks[2][1], ...decks[2][2] ],
        ];

        decks = decks.map(shuffle);

        this.cardDeck = [ ...decks[0], ...decks[1], ...decks[2] ];
    }

    generateEvacueeCards() {
        const actions = [
            ICON.EVACUEE,
        ];
        return this.generateCards('type1', actions);
    }

    generateDangerCards() {
        const actions = [
            ICON.AMBULANCE,
            ICON.FIRE,
            ICON.POLICE,
        ];
        return this.generateCards('type2', actions);
    }

    generateStormCards() {
        const actions = [
            ICON.AMBULANCE + ICON.FIRE,
            ICON.AMBULANCE + ICON.POLICE,
            ICON.FIRE + ICON.POLICE,
            ICON.AMBULANCE + ICON.BOOST,
            ICON.FIRE + ICON.BOOST,
            ICON.POLICE + ICON.BOOST,
        ];
        return this.generateCards('type3', actions);
    }

    generateCards(type, actions) {
        const count = suburbs.length / actions.length;
        
        let events = actions.map(action => { return new Array(count).fill(action) });
        events = events.flat();
        events = shuffle(events);

        return suburbs.map(suburb => {
            return new Card(`${suburb}: ${events.pop()}`, type);
        });
    }

    drawCards(count) {
        let cards = [];
        while(count > 0 && this.cardDeck.length > 0) {
            count--;
            let card = this.cardDeck.shift();
            cards.push(card);
        }
        if (cards.length > 0) {
            this.cardPile.addCards(cards);
        } else {
            alert('Deck is empty.');
            return;
        }
    }
}

window.addEventListener("beforeunload", function(e) {
    e.preventDefault();
    e.returnValue = '';
});

window.onload = () => {
    window.onload = undefined;
    game = new Game();
};

function shuffle(array) {
    for (let i = array.length - 1; i > 0; i--) {
        const j = Math.floor(Math.random() * (i + 1));
        [array[i], array[j]] = [array[j], array[i]];
    }
    return array;
}

function divide(array, parts) {
    let result = [];
    for (let i = parts; i > 0; i--) {
        result.push(array.splice(0, Math.ceil(array.length / i)));
    }
    return result;
}
