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

const SUBURBS = [
    // Falls catchment
    'Asandale',
    'Asandale',
    'Barrish',
    'Barrish',
    'Falls',
    'Gallant',
    'Kingside',
    'Kingside',
    // Diamond catchment
    'Cadbridge',
    'Diamond',
    'East Diamond',
    'East Diamond',
    'Houston',
    'Irvington',
    'Irvington',
    'Jewel Beach',
    'North Tuskan',
    'North Tuskan',
    'Port Pelican',
    'Upper Zenith',
    'Upper Zenith',
    'Zenith',
    // Xavier catchment
    'Lower Rilden',
    'Lower Rilden',
    'Metro Central',
    'Metro Central',
    'Metro Central',
    'Queenside',
    'Rilden Park',
    'Rilden Park',
    'Soulville',
    'Tuskan',
    'West Xavier',
    'Xavier',
    'Xavier',
    'Yellowmoose',
];

class Button {
    _node;
    onclick;

    get node() { return this._node; }

    get text() { return this._textNode.nodeValue; }
    set text(value) { this._textNode.nodeValue = value; }

    constructor(text) {
        this._textNode = document.createTextNode(text);

        let span = document.createElement('span');
        span.appendChild(this._textNode);
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
    _drawCardsButton;
    _drawCount;
    _drawnCards = [];
    _drawnCardsNode;
    _cardCountTextNode;
    _isSeparatorQueued = false;
    _node;

    onCardsDrawn;

    get cards() { return this._deckCards; }
    set cards(value) {
        this._deckCards = value;
        this._clearDrawnCards();
        this._refreshCount();
    }

    get drawCount() { return this._drawCount; }
    set drawCount(value) {
        if (this._drawCount === value) {
            return;
        }
        this._drawCount = value;
        this._drawCardButton.text = `Draw ${value}`;
    }

    get node() { return this._node; }

    constructor(innerHTML) {
        let titleSpan = document.createElement('span');
        titleSpan.innerHTML = innerHTML;

        let titleDiv = document.createElement('div');
        titleDiv.classList.add('cell', 'title');
        titleDiv.appendChild(titleSpan);

        this._drawCardButton = new Button('Draw');
        this._drawCardButton.onclick = (e) => this.drawCards(this.drawCount); 

        this._cardCountTextNode = document.createTextNode('0');

        let cardCountSpan = document.createElement('span');
        cardCountSpan.appendChild(this._cardCountTextNode);

        let cardCountDiv = document.createElement('div');
        cardCountDiv.classList.add('cell', 'count');
        cardCountDiv.appendChild(cardCountSpan);

        let controls = document.createElement('div');
        controls.classList.add('game');
        controls.appendChild(this._drawCardButton.node);
        controls.appendChild(cardCountDiv);

        this._drawnCardsNode = document.createElement('div');

        this._node = document.createElement('div');
        this._node.classList.add('deck');
        this._node.appendChild(titleDiv);
        this._node.appendChild(controls);
        this._node.appendChild(this._drawnCardsNode);

        this.drawCount = 1;
    }

    addSeparator() {
        this._isSeparatorQueued = !this._isSeparatorLast;
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

        if (this._isSeparatorQueued) {
            if (this._drawnCardsNode.firstChild) {
                let line = new HorizontalLine();
                this._drawnCardsNode.insertBefore(line.node, this._drawnCardsNode.firstChild);
            }
            this._isSeparatorQueued = false;
        }

        while (count > 0) {
            count--;
            let card = this._deckCards.shift();
            this._drawnCards.push(card);
            this._drawnCardsNode.insertBefore(card.node, this._drawnCardsNode.firstChild);
        }
        this._refreshCount();

        if (this.onCardsDrawn) {
            this.onCardsDrawn();
        }
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

        let emergencyDeck = new Deck('1. Respond</p>2. Emergency');
        emergencyDeck.cards = this._generateEmergencyCards();
        emergencyDeck.drawCount = 3;

        let evacueeDeck = new Deck('3. Evacuate</p>4. Evacuees');
        evacueeDeck.cards = this._generateEvacueeCards();

        emergencyDeck.onCardsDrawn = () => {
            emergencyDeck.addSeparator();
            evacueeDeck.addSeparator();
        }

        this._node = document.createElement('div');
        this._node.classList.add('game');
        this._node.appendChild(emergencyDeck.node);
        this._node.appendChild(evacueeDeck.node);
    }

    _generateEvacueeCards() {
        return [
            this._generateCards('evacuee', '🚘').shuffle(),
            this._generateCards('evacuee', '🚘').shuffle()
        ].flat();
    }

    _generateEmergencyCards() {
        let decks = [
            this._generateCards('emergency1', '🎲').divide(6),
            this._generateCards('emergency2', '🎲🎲').divide(6),
        ];

        decks = [
            [ decks[0].pop(), decks[0].pop(), decks[0].pop() ],
            [ decks[0].pop(), decks[0].pop(), decks[1].pop() ],
            [ decks[0].pop(), decks[1].pop(), decks[1].pop() ],
            [ decks[1].pop(), decks[1].pop(), decks[1].pop() ],
        ];

        return decks
            .map(deck => deck.flat().shuffle())
            .flat();
    }

    _generateCards(type, action) {
        return SUBURBS
            .map(suburb => new Card(suburb, action, type))
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
