Array.prototype.divide = function(parts) {
    let arrays = [];
    for (let i = parts; i > 0; i--) {
        arrays.push(this.splice(0, Math.ceil(this.length / i)));
    }
    return arrays;
}

Array.prototype.shuffle = function() {
    for (let i = this.length - 1; i > 0; i--) {
        const j = Math.floor(Math.random() * (i + 1));
        [this[i], this[j]] = [this[j], this[i]];
    }
    return this;
}

const ICON = {
    AMBULANCE: '🟥',
    BOOST: '➕',
    CARD: '🃏',
    EVACUEE: '🚘',
    FIRE: '🟡',
    POLICE: '🔷',
};

const SUBURBS = [
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
    _node;
    onclick;

    get node() { return this._node; }

    constructor(text) {
        let span = document.createElement('span');
        span.appendChild(document.createTextNode(text));
        this._node = document.createElement('div');
        this._node.classList.add('cell', 'button');
        this._node.appendChild(span);
        this._node.onclick = (e) => this.onclick ? this.onclick(e) : true;
    }
}

class Card {
    _node;
    onclick;

    get node() { return this._node; }

    constructor(suburb, icons, cardType) {
        let span = document.createElement('span');
        span.appendChild(document.createTextNode(suburb));
        span.appendChild(document.createElement('br'));
        span.appendChild(document.createTextNode(icons));
        this._node = document.createElement('div');
        this._node.classList.add('cell', 'card', cardType);
        this._node.appendChild(span);
        this._node.onclick = (e) => this.onclick ? this.onclick(e) : true;
    }
}

class Deck {
    _cards = [];
    _cardsNode;
    drawCount = 1;
    _cardCountTextNode;
    _node;

    get cards() { return this._cards; }
    set cards(value) {
        this._cards = value;
        this._cardCountTextNode.nodeValue = value.length;
    }

    get node() { return this._node; }

    constructor(title) {
        let titleSpan = document.createElement('span');
        titleSpan.appendChild(document.createTextNode(title));

        let titleDiv = document.createElement('div');
        titleDiv.classList.add('cell', 'title');
        titleDiv.appendChild(titleSpan);

        let button = new Button(`Draw card`);
        button.onclick = (e) => this.drawCards(this.drawCount); 

        this._cardCountTextNode = document.createTextNode('0');

        let cardCountSpan = document.createElement('span');
        cardCountSpan.appendChild(this._cardCountTextNode);

        let cardCountDiv = document.createElement('div');
        cardCountDiv.classList.add('cell', 'count');
        cardCountDiv.appendChild(cardCountSpan);

        let controls = document.createElement('div');
        controls.classList.add('game');
        controls.appendChild(button.node);
        controls.appendChild(cardCountDiv);

        this._cardsNode = document.createElement('div');

        this._node = document.createElement('div');
        this._node.classList.add('deck');
        this._node.appendChild(titleDiv);
        this._node.appendChild(controls);
        this._node.appendChild(this._cardsNode);
    }

    drawCards(count) {
        if (count <= 0) {
            return;
        }
        if (this._cards.length === 0) {
            alert('Deck is empty.');
            return;
        }
        if (count > this._cards.length) {
            count = this._cards.length;
        }

        while (count > 0) {
            count--;
            let card = this._cards.shift();
            this._cardsNode.insertBefore(card.node, this._cardsNode.firstChild);
        }
        this._cardCountTextNode.nodeValue = this._cards.length;

        let line = new HorizontalLine();
        this._cardsNode.insertBefore(line.node, this._cardsNode.firstChild);
    }
}

class Game {
    _node;
    
    get node() { return this._node; }

    constructor() {

        let evacueeDeck = new Deck('Evacuee');
        evacueeDeck.cards = this._generateEvacueeCards();

        let dangerDeck = new Deck('Danger');
        dangerDeck.cards = this._generateDangerCards();
        dangerDeck.drawCount = 3;

        this._node = document.createElement('div');
        this._node.classList.add('game');
        this._node.appendChild(evacueeDeck.node);
        this._node.appendChild(dangerDeck.node);
    }

    _generateEvacueeCards() {
        return this._generateCards('evacuee',
            ICON.EVACUEE,
        ).shuffle();
    }

    _generateDangerCards() {
        let decks = [
            this._generateCards('danger1',
                ICON.AMBULANCE,
                ICON.FIRE,
                ICON.POLICE,
            ).divide(3),
            this._generateCards('danger2',
                ICON.AMBULANCE + ICON.FIRE,
                ICON.AMBULANCE + ICON.POLICE,
                ICON.FIRE      + ICON.POLICE,
                ICON.BOOST     + ICON.AMBULANCE,
                ICON.BOOST     + ICON.FIRE,
                ICON.BOOST     + ICON.POLICE,
            ).divide(3),
        ];

        decks = [
            [ decks[0][0], decks[0][1] ],
            [ decks[0][2], decks[1][0] ],
            [ decks[1][1], decks[1][2] ],
        ];

        return decks
            .map(deck => deck.flat().shuffle())
            .flat();
    }

    _generateCards(type, ...actions) {
        const count = SUBURBS.length / actions.length;
        
        actions = actions
            .map(action => new Array(count).fill(action))
            .flat()
            .shuffle();

        return SUBURBS
            .map(suburb => new Card(suburb, actions.shift(), type))
            .shuffle();
    }
}

class HorizontalLine {
    _node;

    get node() { return this._node; }

    constructor() {
        this._node = document.createElement('div');
        this._node.classList.add('cell', 'line');
    };
}

window.addEventListener("beforeunload", function(e) {
    e.preventDefault();
    e.returnValue = '';
});

window.onload = (e) => {
    window.onload = undefined;
    let game = new Game();
    document.body.appendChild(game.node);
};
