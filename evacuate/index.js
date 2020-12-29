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
    // Falls catchment
    'Asandale',
    'Barrish',
    'Falls',
    'Falls',
    'Gallant',
    'Kingside',
    'Kingside',
    'Kingside',
    // Diamond catchment
    'Cadbridge',
    'Diamond',
    'Diamond',
    'East Diamond',
    'Houston',
    'Ington',
    'Ington',
    'Ington',
    'Jewel Beach',
    'North Tuskan',
    'North Tuskan',
    'Port Pelican',
    'Upper Zenith',
    'Zenith',
    // Xenia catchment
    'Lower Rilden',
    'Lower Rilden',
    'Metro Centre',
    'Metro Centre',
    'Metro Centre',
    'Queenside',
    'Rilden Park',
    'Rilden Park',
    'Soulville',
    'Tuskan',
    'West Xenia',
    'Xenia',
    'Xenia',
    'Yellowmoose',
    // 48 cards
    //'Asandale',
    //'East Diamond',
    //'Falls',
    //'Jewel Beach',
    //'Metro Centre',
    //'North Tuskan',
    //'Queenside',
    //'Tuskan',
    //'Upper Zenith',
    //'West Xenia',
    //'Xenia',
    //'Zenith',
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
    _deckCards = [];
    _drawnCards = [];
    _drawnCardsNode;
    drawCount = 1;
    _cardCountTextNode;
    _node;

    get cards() { return this._deckCards; }
    set cards(value) {
        this._deckCards = value;
        this._clearDrawnCards();
        this._refreshCount();
    }

    get node() { return this._node; }

    constructor(title, canRecycle) {
        let titleSpan = document.createElement('span');
        titleSpan.appendChild(document.createTextNode(title));

        let titleDiv = document.createElement('div');
        titleDiv.classList.add('cell', 'title');
        titleDiv.appendChild(titleSpan);

        let drawCardButton = new Button('Draw');
        drawCardButton.onclick = (e) => this.drawCards(this.drawCount); 

        this._cardCountTextNode = document.createTextNode('0');

        let cardCountSpan = document.createElement('span');
        cardCountSpan.appendChild(this._cardCountTextNode);

        let cardCountDiv = document.createElement('div');
        cardCountDiv.classList.add('cell', 'count');
        cardCountDiv.appendChild(cardCountSpan);

        let controls = document.createElement('div');
        controls.classList.add('game');
        controls.appendChild(drawCardButton.node);
        controls.appendChild(cardCountDiv);

        this._drawnCardsNode = document.createElement('div');

        this._node = document.createElement('div');
        this._node.classList.add('deck');
        this._node.appendChild(titleDiv);
        this._node.appendChild(controls);

        if (canRecycle) {
            let recycleDeckButton = new Button('Recycle');
            recycleDeckButton.onclick = (e) => this.recycleDeck();
            this._node.appendChild(recycleDeckButton.node);
        }

        this._node.appendChild(this._drawnCardsNode);
    }

    drawCards(count) {
        if (count <= 0) {
            return;
        }
        if (this._deckCards.length === 0) {
            alert('Deck is empty.');
            return;
        }
        if (count > this._deckCards.length) {
            count = this._deckCards.length;
        }

        while (count > 0) {
            count--;
            let card = this._deckCards.shift();
            this._drawnCards.push(card);
            this._drawnCardsNode.insertBefore(card.node, this._drawnCardsNode.firstChild);
        }
        this._refreshCount();

        let line = new HorizontalLine();
        this._drawnCardsNode.insertBefore(line.node, this._drawnCardsNode.firstChild);
    }

    recycleDeck() {
        this._deckCards = [ this._deckCards, this._drawnCards ]
            .flat()
            .shuffle();

        this._clearDrawnCards();
        this._refreshCount();
    }

    _clearDrawnCards() {
        this._drawnCards = [];
        while (this._drawnCardsNode.firstChild) {
            this._drawnCardsNode.firstChild.remove();
        }
    }

    _refreshCount() {
        this._cardCountTextNode.nodeValue = this._deckCards.length;
    }
}

class Game {
    _node;
    
    get node() { return this._node; }

    constructor() {

        let evacueeDeck = new Deck('Evacuee', true);
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
        const COUNT = SUBURBS.length / actions.length;
        
        actions = actions
            .map(action => new Array(COUNT).fill(action))
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
