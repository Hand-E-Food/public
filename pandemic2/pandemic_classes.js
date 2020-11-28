class InfectionCard {
    _color;
    _deck;
    _div;
    infection;
    _name;
    _span;
    _upgrade;

    onclick;

    constructor(data) {
        this._span = document.createElement('span');
        this._div = document.createElement('div');
        this._div.classList.add('button');
        this._div.classList.add('cell');
        this._div.appendChild(this._span);
        this._div.onclick = (e) => this.onclick ? this.onclick(e) : true;

        if (data) {
            this.color = data.color;
            this.infection = data.infection;
            this._name = data.name;
            this._upgrade = data.upgrade ? data.upgrade : null;
        }
        this.refreshText();
    }

    get color() { return this._color; }
    set color(value) {
        if (this._color === value) {
            return;
        }
        if (this._color) {
            this._div.classList.remove(this._color);
        }
        this._color = value;
        if (this._color) {
            this._div.classList.add(this._color);
        }
    }

    get deck() { return this._deck; }
    set deck(value)
    {
        if (_deck === value) {
            return;
        }
        if (_deck) {
            this._deck.removeCard(this);
        }
        this._deck = value;
        if (this._deck) {
            this._deck.addCard(this);
        }
    }

    get name() { return this._name; }
    set name(value) {
        if (this._name === value) {
            return;
        }
        this._name = value;
        this.refreshText();
    }

    get node() { return this._div; }

    get saveData() {
        return new {
            color: this.color,
            infection: this.infection,
            name: this.name,
            upgrade: this.upgrade,
        };
    }

    get upgrade() { return this._upgrade; }
    set upgrade(value) {
        if (this._upgrade === value) {
            return;
        }
        this._upgrade = value;
        this.refreshText();
    }

    refreshText() {
        while (this._span.firstChild) {
            this._span.firstChild.remove();
        }
        this._span.appendChild(document.createTextNode(card.name));
        if (card.upgrade) {
            this._span.appendChild(document.createElement('br'));
            var cardUpgrade = document.createElement('span');
            cardUpgrade.classList.add('upgrade');
            cardUpgrade.appendChild(document.createTextNode(card.upgrade));
            this._span.appendChild(cardUpgrade);
        }
    }
}

class InfectionDeck {
    _cards = new Array();
    _div;
    _name;
    parent = null;
    _span;

    onselected = null;

    constructor(data) {
        this._span = document.createElement('span');
        this._span.id = 'countdown';
    
        var countdownDiv = document.createElement('div');
        countdownDiv.classList.add('cell');
        countdownDiv.appendChild(this._span);
        countdownDiv.onclick = (e) => this.onselected ? this.onselected() : true;
    
        this._div = document.createElement('div');
        this._div.classList.add('phase');
        this._div.classList.add('section');
        this._div.appendChild(countdownDiv);
    
        if (data) {
            this.name = data.name;
            if (data.cards) {
                data.cards
                    .map(cardData => new InfectionCard(cardData))
                    .forEach(addCard);
            }
        }

        this.refreshText();
    }

    get count() { return this._cards.length; }
    get name() { return this._name; }
    get node() { return this._div; }

    get saveData() {
        return new {
            name: this._name,
            cards: this._cards.map(card => card.saveData),
        };
    }

    addCard(card) {
        var sibling = null;
        var i = 0;
        for (; i < this._cards.length; i++) {
            var other = this._cards[i];
            if (card.infection < other.infection) {
                sibling = other;
                break;
            } else if (card.infection > other.infection) {
            } else if (card.name < other.name) {
                sibling = other;
                break;
            }
        }

        this._cards.splice(i, 0, card);
        this._div.insertBefore(card, sibling);
    }

    refreshText() {
        while (this._span.firstChild) {
            this._span.firstChild.remove();
        }
        this._span.appendChild(document.createTextNode(_name))
    }

    removeCard(card) {
        for (var i = this._cards.length - 1; i >= 0; i--) {
            if (this._cards[i] === card) {
                this._cards.splice(i, 1);
                break;
            }
        }
        card.remove();
    }
}

class InfectionDecks {
    _decks = new Array();
    _div;

    constructor(data) {
        this._div = document.createElement('div');

        if (data) {
            data
                .map(deckData => new InfectionDeck(deckData))
                .forEach(this.addToBottom);
        }
    }

    get node() { return this._div; }

    get saveData() {
        return this._decks.map(deck => deck.saveData);
    }

    addToBottom(deck) {
        deck.parent = this;
        this._decks.push(deck);
        this._div.appendChild(deck.node);
    }

    addToTop(deck) {
        deck.parent = this;
        this._decks.unshift(deck);
        this._div.prepend(deck.node);
    }

    getDeckByName(name) {
        return this._decks.find(deck => deck.name === name);
    }

    refreshText() {
        this._decks.forEach(deck => deck.refreshText());
    }

    remove(deck) {
        var i = this._decks.indexOf(deck);
        if (i < 0) {
            return;
        }
        deck.node.remove();
        this._decks.splice(i, 1);
        deck.parent = null;
    }
}

class PlayerDeck {
    _cityCards;
    _fundingCards;
    _players = new Array();
    _resourceCards;

    constructor(data) {
        if (data) {
            this.cityCards = data.cityCards;
            this.fundingCards = data.fundingCards;
            this.resourceCards = data.resourceCards;
            if (data.players) {
                data.players.forEach(addPlayer);
            }
        }
    }

    get cityCards() { return this._cityCards; }
    set cityCards(value) { this._cityCards = value; }

    get epidemicCards() {
        if (this.cityCards <= 36) {
            return 5;
        } else if (this.cityCards <= 44) {
            return 6;
        } else if (this.cityCards <= 51) {
            return 7;
        } else if (this.cityCards <= 57) {
            return 8;
        } else if (this.cityCards <= 62) {
            return 9;
        } else {
            return 10;
        }
    }

    get fundingCards() { return this._fundingCards; }
    set fundingCards(value) { this._fundingCards = value; }

    get node() { return this._div; }

    get players() { return this._players; }

    get resourceCards() { return this._resourceCards; }
    get resourceCards(value) { this._resourceCards = value; }

    get saveData() {
        return new {
            players: this._players,
            cityCards: this.cityCards,
            resourceCards: this.resourceCards,
            fundingCards: this.fundingCards,
        };
    }

    get startingCards() {
        var playerCount = this.players.length;
        if (playerCount === 2) {
            return 2 * 4;
        } else if (playerCount === 3) {
            return 3 * 3;
        } else if (playerCount === 4) {
            return 4 * 2;
        } else {
            return 0;
        }
    }

    get totalCards() {
        return this.cityCards + this.resourceCards + this.fundingCards + this.epidemicCards - this.startingCards;
    }

    addPlayer(color) {
        this._players.push(color);
    }

    removePlayer(color) {
        var i = this._players.indexOf(color);
        if (i < 0) {
            return;
        }
        this._players.splice(i, 1);
    }
}

class Game {
    infectionDecks;
    _div;
    playerDeck;

    constructor(data) {
        if (data) {
            _players = data.players;
            cityCards = data.cityCards;
            fundingCards = data.fundingCards;
            resourceCards = data.resourceCards;
            decks = new InfectionDecks(data.decks);
        }
    }

    get node() { return this._div; }

    get players() { return this._players; }

    get saveData() {
        return new {
            infectionDecks: this.infectionDecks.saveData,
            playerDeck: this.playerDeck.saveData,
        };
    }
}

const initialGame = new Game(new {
    cityCards: 36,
    resourceCards: 8,
    fundingCards: 4,
    infectionPhases: 0,
    players: [ 'blue', 'brown', 'pink', 'white' ],
    decks: [
        {
            name: 'Infection',
            cards: [
                { infection: true, color: 'black' , name: 'Cairo' },
                { infection: true, color: 'black' , name: 'Cairo' },
                { infection: true, color: 'black' , name: 'Cairo' },
                { infection: true, color: 'black' , name: 'Istanbul' },
                { infection: true, color: 'black' , name: 'Istanbul' },
                { infection: true, color: 'black' , name: 'Istanbul' },
                { infection: true, color: 'yellow', name: 'Jacksonville' },
                { infection: true, color: 'yellow', name: 'Jacksonville' },
                { infection: true, color: 'yellow', name: 'Jacksonville' },
                { infection: true, color: 'yellow', name: 'Lagos' },
                { infection: true, color: 'yellow', name: 'Lagos' },
                { infection: true, color: 'yellow', name: 'Lagos' },
                { infection: true, color: 'blue'  , name: 'London' },
                { infection: true, color: 'blue'  , name: 'London' },
                { infection: true, color: 'blue'  , name: 'London' },
                { infection: true, color: 'blue'  , name: 'New York' },
                { infection: true, color: 'blue'  , name: 'New York' },
                { infection: true, color: 'blue'  , name: 'New York' },
                { infection: true, color: 'yellow', name: 'São Paulo' },
                { infection: true, color: 'yellow', name: 'São Paulo' },
                { infection: true, color: 'yellow', name: 'São Paulo' },
                { infection: true, color: 'black' , name: 'Tripoli' },
                { infection: true, color: 'black' , name: 'Tripoli' },
                { infection: true, color: 'black' , name: 'Tripoli' },
                { infection: true, color: 'blue'  , name: 'Washington' },
                { infection: true, color: 'blue'  , name: 'Washington' },
                { infection: true, color: 'blue'  , name: 'Washington' },
            ]
        },
        { name: 'Inoculated' },
        { name: 'Game End' },
        { name: 'Reserve' },
        { name: 'Destroyed' },
    ]
});

var game;

window.onload = () => {
    window.onload = undefined;

    loadGame();
    bodyDiv.appendChild(game.node);
};

function loadGame() {
    var json = window.localStorage.getItem('Game');
    if (json) {
        try {
            game = JSON.parse(json);
            return;
        }
        catch { }
    }

    game = initialGame;
    saveGame();
}

function saveGame() {
    window.localStorage.setItem('Game', JSON.stringify(this.saveData));
}

