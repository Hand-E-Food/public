const LOCAL_STORAGE_KEY = 'pandemic0';
const PURPOSE = {
    DESTROYED: 'Destroyed',
    DISCARD: 'Discard Pile',
    GAME_END: 'Game End',
    THREAT: 'Threat',
};

class ThreatCard {
    _affiliation;
    _deck;
    deckName;
    _div;
    _incident;
    _name;
    _region;
    _span;

    onclick;

    constructor(data) {
        this._span = document.createElement('span');
        this._div = document.createElement('div');
        this._div.classList.add('button');
        this._div.classList.add('cell');
        this._div.appendChild(this._span);
        this._div.onclick = (e) => this.onclick ? this.onclick(e) : true;

        this.affiliation = data.affiliation;
        this.deckName = data.deck;
        this.incident = data.incident;
        this.name = data.name;
        this.region = data.region;
    }

    get affiliation() { return this._affiliation; }
    set affiliation(value) {
        if (this._affiliation === value) {
            return;
        }
        if (this._affiliation) {
            this._div.classList.remove(this._affiliation.toLowerCase());
        }
        this._affiliation = value;
        if (this._affiliation) {
            this._div.classList.add(this._affiliation.toLowerCase());
        }
    }

    get deck() { return this._deck; }
    set deck(value)
    {
        if (this._deck === value) {
            return;
        }
        if (this._deck) {
            this._deck.removeCard(this);
        }
        this._deck = value;
        this.deckName = this._deck.saveName;
        if (this._deck) {
            this._deck.addCard(this);
        }
    }

    get incident() { return this._incident; }
    set incident(value) {
        if (this._incident === value) {
            return;
        }
        this._incident = value;
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

    get region() { return this._region; }
    set region(value) {
        if (this._region === value) {
            return;
        }
        if (this._region) {
            this._div.classList.remove(this._region.replace(' ', '-').toLowerCase());
        }
        this._region = value;
        if (this._region) {
            this._div.classList.add(this._region.replace(' ', '-').toLowerCase());
        }
    }

    get saveData() {
        return {
            affiliation: this.affiliation,
            deck: this.deckName,
            incident: this.incident,
            name: this.name,
            region: this.region,
        };
    }

    refreshText() {
        while (this._span.firstChild) {
            this._span.firstChild.remove();
        }
        this._span.appendChild(document.createTextNode(this._name));
    }
}

class ThreatDeck {
    _cards = new Array();
    _countNode;
    _displayName;
    _div;
    _heading;
    _headingNode;
    _name;
    parent = null;
    purpose;
    _selected = false;

    onselected = null;

    constructor(purpose) {
        this._headingNode = document.createElement('span');

        this._countNode = document.createElement('span');
        this._countNode.style.fontSize = 'smaller';

        var titleSpan = document.createElement('span');
        titleSpan.appendChild(this._headingNode);
        titleSpan.appendChild(document.createElement('br'));
        titleSpan.appendChild(this._countNode);

        var titleDiv = document.createElement('div');
        titleDiv.classList.add('cell');
        titleDiv.appendChild(titleSpan);
        titleDiv.onclick = (e) => this.onselected ? this.onselected() : true;
    
        this._div = document.createElement('div');
        this._div.classList.add('phase', 'section');
        this._div.appendChild(titleDiv);

        this.purpose = purpose;
        this.heading = purpose;
        this.updateCount();
    }

    get count() { return this._cards.length; }

    get heading() { return this._heading; }
    set heading(value) {
        if (this._heading === value) {
            return;
        }
        this._heading = value;
        this._headingNode.innerHTML = value;
    }

    get node() { return this._div; }

    get saveName() {
        switch(this.purpose) {
            case PURPOSE.DISCARD:
            case PURPOSE.GAME_END:
                return PURPOSE.THREAT;
            default:
                return this.purpose;
        }
    }

    get selected() { return this._selected; }
    set selected(value) {
        if (this._selected === value) {
            return;
        }
        this._selected = value;
        if (this._selected) {
            this._div.classList.add('selected');
        } else {
            this._div.classList.remove('selected');
        }
    }

    addCard(card) {
        var sibling = null;
        var i = 0;
        for (; i < this._cards.length; i++) {
            var other = this._cards[i];
            if (card.name < other.name) {
                sibling = other;
                break;
            } else if (card.name > other.name) {
            } else if (card.incident && !other.incident) {
                sibling = other;
                break;
            }
        }

        this._cards.splice(i, 0, card);
        if (sibling) {
            this._div.insertBefore(card.node, sibling.node);
        } else {
            this._div.appendChild(card.node);
        }
        card.parent = this;
        this.updateCount();
    }

    removeCard(card) {
        for (var i = this._cards.length - 1; i >= 0; i--) {``
            if (this._cards[i] === card) {
                this._cards.splice(i, 1);
                break;
            }
        }
        card.node.remove();
        card.parent = null;
        this.updateCount();
    }

    updateCount() {
        this._countNode.innerHTML = `(${this.count})`;
    }
}

class ThreatDecks {
    _decks = new Array();
    _div;
    _selectedDeck = null;

    constructor() {
        this._div = document.createElement('div');
    }

    get count() { return this._decks.length; }

    get discardPile() { return this._decks.find(deck => deck.purpose === PURPOSE.DISCARD); }

    get drawPile() { return this._decks.filter(deck => deck.purpose === PURPOSE.THREAT); }

    get node() { return this._div; }

    get selectedDeck() { return this._selectedDeck; }
    set selectedDeck(value) {
        if (this._selectedDeck === value) {
            return;
        }
        if (this._selectedDeck) {
            this._selectedDeck.selected = false;
        }
        this._selectedDeck = value;
        if (this._selectedDeck) {
            this._selectedDeck.selected = true;
        }
    }

    startNewGame(cards) {
        while(this._decks.length > 0) {
            this.remove(this._decks[0]);
        }

        [ PURPOSE.DISCARD, PURPOSE.THREAT, PURPOSE.GAME_END, PURPOSE.DESTROYED ]
            .forEach(purpose => this.addToBottom(new ThreatDeck(purpose)), this);

        this.selectedDeck = this._decks[0];

        cards.forEach(card => card.deck = this._decks.find(deck => deck.purpose === card.deckName), this);
    }

    addToBottom(deck) {
        deck.parent = this;
        this._decks.push(deck);
        this._div.appendChild(deck.node);
        deck.onselected = e => this.selectedDeck = deck;
    }

    addToTop(deck) {
        deck.parent = this;
        this._decks.unshift(deck);
        this._div.prepend(deck.node);
        deck.onselected = e => this.selectedDeck = deck;
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
        delete deck.onselected;
        if (deck.selected) {
            this.selectedDeck = null;
        }
    }
}

class AddCardPanel {
    _div;
    _name = '';
    _nameInput;

    onAddCard;
    
    constructor() {
        this._nameInput = document.createElement('input');
        this._nameInput.id = 'addCardNameInput';
        this._nameInput.type = 'text';
        this._nameInput.onchange = e => this._name = this._nameInput.value;
        
        const nameLabel = document.createElement('label');
        nameLabel.for = this._nameInput.id;
        nameLabel.classList.add('label');
        nameLabel.appendChild(document.createTextNode('Name: '));

        const addCardSpan = document.createElement('span');
        addCardSpan.appendChild(document.createTextNode('Add Card'));

        const addCardDiv = document.createElement('div');
        addCardDiv.classList.add('green', 'cell');
        addCardDiv.style.float = 'right';
        addCardDiv.appendChild(addCardSpan);
        addCardDiv.onclick = e => this.addCard();

        this._div = document.createElement('div');
        this._div.classList.add('input');
        this._div.appendChild(nameLabel);
        this._div.appendChild(this._nameInput);
        this._div.appendChild(document.createElement('br'));
        this._div.appendChild(addCardDiv);
    }

    get name() { return this._name; }
    set name(value) {
        if (this._name === value) {
            return;
        }
        this._name = value;
        this._nameInput.value = value;
    }

    get node() { return this._div; }

    addCard() {
        if (!this.name || !this.onAddCard) {
            return;
        }
        const card = new ThreatCard({
            name: this.name,
            affiliation: 'Infection',
        });
        this.onAddCard(card);
        this.name = '';
    }
}

class PlayerDeck {
    _cardStacks;
    _cityCards;
    _cityCardsInput;
    _currentPlayer;
    _escalationCards;
    _escalationCardsInput;
    _escalationTurnsOutput;
    _fundingCards;
    _fundingCardsInput;
    _newGameButton;
    _objectiveCards;
    _objectiveCardsInput;
    _pawn;
    _playerCardsSpan;
    _playerInputs;
    _playersInput;
    _players = new Array();
    _resourceCards;
    _resourceCardsInput;

    constructor() {
        this._cityCardsInput = document.getElementById('cityCardsInput');
        this._escalationCardsInput = document.getElementById('escalationCardsInput');
        this._escalationTurnsOutput = document.getElementById('escalationTurnsOutput');
        this._fundingCardsInput = document.getElementById('fundingCardsInput');
        this._newGameButton = document.getElementById('newGameButton');
        this._objectiveCardsInput = document.getElementById('objectiveCardsInput');
        this._pawn = document.getElementById('pawn');
        this._playersInput = document.getElementById('playersInput');
        this._playerCardsSpan = document.getElementById('playerCardsSpan');
        this._resourceCardsInput = document.getElementById('resourceCardsInput');
        this._playerInputs = [...document.getElementsByName('playerInput')];

        this._cityCardsInput.onchange       = e => this._cityCards       = parseInt(this._cityCardsInput.value      );
        this._escalationCardsInput.onchange = e => this._escalationCards = parseInt(this._escalationCardsInput.value);
        this._fundingCardsInput.onchange    = e => this._fundingCards    = parseInt(this._fundingCardsInput.value   );
        this._objectiveCardsInput.onchange  = e => this._objectiveCards  = parseInt(this._objectiveCardsInput.value );
        this._resourceCardsInput.onchange   = e => this._resourceCards   = parseInt(this._resourceCardsInput.value  );
        this._playerInputs.forEach(playerInput => playerInput.onchange = e => this.playerInputChanged(playerInput), this);
    }

    get cityCards() { return this._cityCards; }
    set cityCards(value) {
        if (this._cityCards === value) {
            return;
        }
        this._cityCardsInput.value = value;
        this._cityCards = value;
    }

    get escalationCards() { return this._escalationCards; }
    set escalationCards(value) {
        if (this._escalationCards === value) {
            return;
        }
        this._escalationCardsInput.value = value;
        this._escalationCards = value;
    }

    get fundingCards() { return this._fundingCards }
    set fundingCards(value) {
        if (this._fundingCards === value) {
            return;
        }
        this._fundingCardsInput.value = value;
        this._fundingCards = value;
    }

    get isValid() { return this.startingCards && this.totalCards > 0; }

    get objectiveCards() { return this._objectiveCards }
    set objectiveCards(value) {
        if (this._objectiveCards === value) {
            return;
        }
        this._objectiveCardsInput.value = value;
        this._objectiveCards = value;
    }

    get players() { return this._players; }

    get resourceCards() { return this._resourceCards }
    set resourceCards(value) {
        if (this._resourceCards === value) {
            return;
        }
        this._resourceCardsInput.value = value;
        this._resourceCards = value;
    }

    get saveData() {
        return {
            cityCards: this.cityCards,
            objectiveCards: this.objectiveCards,
            resourceCards: this.resourceCards,
            fundingCards: this.fundingCards,
            escalationCards: this.escalationCards,
        };
    }

    get startingCards() {
        switch(this.players.length) {
            case 2:  return 2 * 4;
            case 3:  return 3 * 3;
            case 4:  return 4 * 2;
            default: return undefined;
        }
    }

    get totalCards() {
        return this.cityCards - this.objectiveCards + this.resourceCards + this.fundingCards - this.startingCards + this.escalationCards;
    }

    loadFrom(data) {
        this.cityCards = data.cityCards;
        this.escalationCards = data.escalationCards;
        this.fundingCards = data.fundingCards;
        this.objectiveCards = data.objectiveCards;
        this.resourceCards = data.resourceCards;
        this._players = new Array();
        this._playerInputs.forEach(playerInput => playerInput.checked = false);
        this.updatePlayersInput();
    }

    playerInputChanged(playerInput) {
        if (playerInput.checked) {
            this.addPlayer(playerInput.value);
        } else {
            this.removePlayer(playerInput.value);
        }
    }

    addPlayer(color) {
        this._players.push(color);
        this.updatePlayersInput();
    }

    removePlayer(color) {
        var i = this._players.indexOf(color);
        if (i < 0) {
            return;
        }
        this._players = this._players.splice(i, 1);
        this.updatePlayersInput();
    }

    updatePlayersInput() {
        this._playersInput.value = this._players.join(',');
    }

    startNewGame() {
        this._currentPlayer = 0;
        this.recalculatePawnColor();

        this._cardStacks = new Array(this.escalationCards);
        for(var i = 0; i < this.escalationCards; i++) {
            this._cardStacks[i] = 0;
        }
        for (var playerCards = this.totalCards - 1; playerCards >= 0; playerCards--) {
            this._cardStacks[playerCards % this.escalationCards]++;
        }
        this._cardStacks = this._cardStacks.map(cards => ({ cards: cards, escalation: true }));

        this.recalculatePlayerCards();
        this.recalculateEscalationTurns();
    }

    nextTurn() {
        this._currentPlayer = (this._currentPlayer + 1) % this._players.length;
        this.recalculatePawnColor();
        this.drawPlayerCards(2);
    }
    
    recalculatePawnColor() {
        this._pawn.style.fill = this._players[this._currentPlayer];
    }

    drawPlayerCards(drawCards) {
        while (drawCards > 0) {
            var cardStack = this._cardStacks.find(stack => stack.cards > 0);
            if (!cardStack) { break; }
            if (cardStack.cards > drawCards) {
                cardStack.cards = cardStack.cards - drawCards;
                drawCards = 0;
            } else {
                drawCards = drawCards - cardStack.cards;
                cardStack.cards = 0;
            }
        }
        this.recalculatePlayerCards();
        this.recalculateEscalationTurns();
    }
    
    recalculatePlayerCards() {
        var count = 0;
        this._cardStacks.forEach(stack => count = count + stack.cards);
        this._playerCardsSpan.innerHTML = `${count} cards`;
    }
    
    nextEscalation() {
        var cardStack = this._cardStacks.find(stack => stack.escalation);
        if (!cardStack) {
            return;
        }
        cardStack.escalation = false;
        this.recalculateEscalationTurns();
    }
    
    recalculateEscalationTurns() {
        var safeCards = 0;
        var escalationStack = this._cardStacks.find(stack => {
            if (stack.escalation) {
                return true;
            }
            safeCards = safeCards + stack.cards;
            return false;
        });
    
        var firstTurn = Math.ceil((safeCards + 1) / 2);
        var lastTurn = Math.ceil((safeCards + escalationStack.cards) / 2);
        this._escalationTurnsOutput.innerHTML
            = !escalationStack ? 'never again!'
            : lastTurn < 1     ? 'is late'
            : lastTurn === 1   ? 'on this turn'
            : firstTurn === 1  ? 'within ' + lastTurn + ' turns'
                               : 'in ' + firstTurn + ' to ' + lastTurn + ' turns';
    }
}

class Game {
    addCardPanel = new AddCardPanel();
    cards;
    _drawPlayerCardButton;
    _escalationButton;
    _escalationDiv;
    _newGameButton;
    _newPhaseButton;
    _nextTurnButton;
    playerDeck = new PlayerDeck();
    _playerDeckDiv;
    _skipEscalationButton;
    threatDecks = new ThreatDecks();
    _threatLevel;
    _threatLevels;
    _threatLevelOutput;

    constructor() {
        this._drawPlayerCardButton = document.getElementById('drawPlayerCardButton')
        this._escalationButton = document.getElementById('escalationButton');
        this._escalationDiv = document.getElementById('escalationDiv');
        this._newGameButton = document.getElementById('newGameButton');
        this._newPhaseButton = document.getElementById('newPhaseButton');
        this._nextTurnButton = document.getElementById('nextTurnButton');
        this._playerDeckDiv = document.getElementById('playerDeckDiv');
        this._skipEscalationButton = document.getElementById('skipEscalationButton');
        this._threatLevelOutput = document.getElementById('threatLevelOutput');

        this.addCardPanel.onAddCard = card => this.addCard(card);
        this._drawPlayerCardButton.onclick = e => this.playerDeck.drawPlayerCards(1);
        this._escalationButton.onclick = e => this.nextPhase(true, true); //TODO
        this._newGameButton.onclick = e => this.newGameButtonClicked();
        this._newPhaseButton.onclick = e => this.nextPhase(false, false); //TODO
        this._nextTurnButton.onclick = e => this.playerDeck.nextTurn();
        this._skipEscalationButton.onclick = e => this.playerDeck.nextEscalation();

        this._escalationDiv.remove();
    }

    get saveData() {
        return {
            cards: this.cards.map(card => card.saveData),
            playerDeck: this.playerDeck.saveData,
        };
    }

    get threatLevel() { return this._threatLevel; }
    set threatLevel(value) {
        if (this._threatLevel === value) {
            return;
        }
        this._threatLevel = value;
        this._threatLevelOutput.innerHTML = this.threatLevel;
    }

    loadFrom(data) {
        this.playerDeck.loadFrom(data.playerDeck);
        this.cards = data.cards.map(cardData => {
            const card = new ThreatCard(cardData);
            card.onclick = e => this.cardClicked(card)
            return card;
        }, this);
    }

    addCard(card) {
        card.onclick = e => this.cardClicked(card)
        this.cards.push(card);
        card.deck = this.threatDecks.selectedDeck;
        saveGame();
    }

    newGameButtonClicked() {
        this.startNewGame();
    }

    startNewGame() {
        if (!this.playerDeck.isValid) {
            alert('Game settings are invalid.');
        }

        this.playerDeck.startNewGame();
        this.threatDecks.startNewGame(this.cards);
        this._threatLevels = [2, 2, 2, 3, 3, 4];
        this.threatLevel = this._threatLevels.shift();

        const body = document.getElementById('body');
        body.appendChild(this._escalationDiv);
        body.appendChild(this.threatDecks.node);
        body.appendChild(this.addCardPanel.node);
        this._playerDeckDiv.remove();
    }

    nextPhase(escalation, accelerate) {
        if (this.threatDecks.discardPile.count === 0) {
            return;
        }
        if (escalation) {
            this.playerDeck.nextEscalation();
        }
        if (accelerate) {
            if (this._threatLevels.length > 0) {
                this.threatLevel = this._threatLevels.shift();
            }
        }
    
        this.threatDecks.discardPile.purpose = PURPOSE.THREAT;
        const discardPile = new ThreatDeck(PURPOSE.DISCARD);
        this.threatDecks.addToTop(discardPile);
        this.threatDecks.selectedDeck = discardPile;

        this.recalculateCountdowns();
    }
    
    cardClicked(card) {
        if (card.deck.purpose === PURPOSE.THREAT && card.deck.count === 1) {
            this.threatDecks.remove(card.deck);
        }
        card.deck = this.threatDecks.selectedDeck;
        saveGame();
        this.recalculateCountdowns();
    }

    recalculateCountdowns() {
        var totalThreats = 0;
        this.threatDecks.drawPile.forEach(deck => {
            totalThreats = totalThreats + 1;
            var firstTurn = Math.ceil(totalThreats / this.threatLevel);
            totalThreats = totalThreats + deck.count - 1;
            var lastTurn = Math.ceil(totalThreats / this.threatLevel);
            if (lastTurn < firstTurn) {
                lastTurn = firstTurn;
            }

            deck.heading
                = lastTurn === 1 ? lastTurn + ' turn'
                : firstTurn === lastTurn ? firstTurn + ' turns'
                : firstTurn + ' - ' + lastTurn + ' turns';
        });

        var zeroCardsInDiscardPile = this.threatDecks.discardPile.count === 0;
        escalationButton.disabled = zeroCardsInDiscardPile;
        newPhaseButton.disabled = zeroCardsInDiscardPile;
    }
}

var game;

window.addEventListener("beforeunload", function(e) {
    e.preventDefault();
    e.returnValue = '';
});

window.onload = () => {
    window.onload = undefined;
    game = new Game();
    loadGame();
};

function loadGame() {
    var json = window.localStorage.getItem(LOCAL_STORAGE_KEY);
    if (json) {
        try {
            game.loadFrom(JSON.parse(json));
            return;
        }
        catch { }
    }

    game.loadFrom({
        cards: [
            { deck:'Threat', name:'Algiers'      , affiliation:'Allied' , region:'Africa'       , incident:'Sleeper Cells Activate: North America' },
            { deck:'Threat', name:'Atlanta'      , affiliation:'Allied' , region:'North America', incident:'Sleeper Cells Activate: Asia' },
            { deck:'Threat', name:'Baghdad'      , affiliation:'Soviet' , region:'Asia'         , incident:'Sleeper Cells Activate: Pacific Rim' },
            { deck:'Threat', name:'Bangkok'      , affiliation:'Neutral', region:'Asia'         , incident:'Teams Compromised' },
            { deck:'Threat', name:'Bogotá'       , affiliation:'Neutral', region:'South America', incident:'Sleeper Cells Activate: North America' },
            { deck:'Threat', name:'Bombay'       , affiliation:'Neutral', region:'Asia'         , incident:'Sleeper Cells Activate: Pacific Rim' },
            { deck:'Threat', name:'Buenos Aires' , affiliation:'Neutral', region:'South America', incident:'Recognized: South America, Europe' },
            { deck:'Threat', name:'Cairo'        , affiliation:'Soviet' , region:'Africa'       , incident:'Sleeper Cells Activate: Europe' },
            { deck:'Threat', name:'Calcutta'     , affiliation:'Neutral', region:'Asia'         , incident:'Sleeper Cells Activate: Africa' },
            { deck:'Threat', name:'Delhi'        , affiliation:'Neutral', region:'Asia'         , incident:'Sleeper Cells Activate: Africa' },
            { deck:'Threat', name:'East Berlin'  , affiliation:'Soviet' , region:'Europe'       , incident:'Sleeper Cells Activate: South America' },
            { deck:'Threat', name:'Hanoi'        , affiliation:'Soviet' , region:'Asia'         , incident:'Sleeper Cells Activate: South America' },
            { deck:'Threat', name:'Havana'       , affiliation:'Soviet' , region:'North America', incident:'Safehouse Compromised: Asia' },
            { deck:'Threat', name:'Istanbul'     , affiliation:'Allied' , region:'Europe'       , incident:'Safehouse Compromised: Africa' },
            { deck:'Threat', name:'Jakarta'      , affiliation:'Neutral', region:'Pacific Rim'  , incident:'Sleeper Cells Activate: Europe' },
            { deck:'Threat', name:'Johannesburg' , affiliation:'Allied' , region:'Africa'       , incident:'Safehouse Compromised: South America' },
            { deck:'Threat', name:'Karachi'      , affiliation:'Neutral', region:'Asia'         , incident:'Sleeper Cells Activate: South America' },
            { deck:'Threat', name:'Khartoum'     , affiliation:'Neutral', region:'Africa'       , incident:'Sleeper Cells Activate: Asia' },
            { deck:'Threat', name:'Kiev'         , affiliation:'Neutral', region:'Europe'       , incident:'Safehouse Compromised: Pacific Rim' },
            { deck:'Threat', name:'Lagos'        , affiliation:'Neutral', region:'Africa'       , incident:'Sleeper Cells Activate: South America' },
            { deck:'Threat', name:'Leningrad'    , affiliation:'Soviet' , region:'Europe'       , incident:'Teams Compromised' },
            { deck:'Threat', name:'Leopoldville' , affiliation:'Neutral', region:'Africa'       , incident:'Recognized: Africa, Asia' },
            { deck:'Threat', name:'Lima'         , affiliation:'Neutral', region:'South America', incident:'Sleeper Cells Activate: Europe' },
            { deck:'Threat', name:'London'       , affiliation:'Allied' , region:'Europe'       , incident:'Sleeper Cells Activate: Asia' },
            { deck:'Threat', name:'Los Angeles'  , affiliation:'Allied' , region:'North America', incident:'Sleeper Cells Activate: Europe' },
            { deck:'Threat', name:'Madrid'       , affiliation:'Neutral', region:'Europe'       , incident:'Sleeper Cells Activate: Pacific Rim' },
            { deck:'Threat', name:'Manila'       , affiliation:'Neutral', region:'Pacific Rim'  , incident:'Sleeper Cells Activate: Asia' },
            { deck:'Threat', name:'Mexico City'  , affiliation:'Neutral', region:'North America', incident:'Sleeper Cells Activate: Pacific Rim' },
            { deck:'Threat', name:'Moscow'       , affiliation:'Soviet' , region:'Europe'       , incident:'Recognized: Europe, Asia' },
            { deck:'Threat', name:'New York'     , affiliation:'Allied' , region:'North America', incident:'Recognized: North America, South America' },
            { deck:'Threat', name:'Novosibirisk' , affiliation:'Soviet' , region:'Asia'         , incident:'Sleeper Cells Activate: Europe' },
            { deck:'Threat', name:'Osaka'        , affiliation:'Neutral', region:'Pacific Rim'  , incident:'Recognized: Pacific Rim, North America' },
            { deck:'Threat', name:'Paris'        , affiliation:'Allied' , region:'Europe'       , incident:'Sleeper Cells Activate: North America' },
            { deck:'Threat', name:'Peking'       , affiliation:'Soviet' , region:'Asia'         , incident:'Safehouse Compromised: North America' },
            { deck:'Threat', name:'Prague'       , affiliation:'Soviet' , region:'Europe'       , incident:'Sleeper Cells Activate: Asia' },
            { deck:'Threat', name:'Pyongyang'    , affiliation:'Soviet' , region:'Asia'         , incident:'Safehouse Compromised: Europe' },
            { deck:'Threat', name:'Riyadh'       , affiliation:'Neutral', region:'Asia'         , incident:'Sleeper Cells Activate: North America' },
            { deck:'Threat', name:'Rome'         , affiliation:'Allied' , region:'Europe'       , incident:'Sleeper Cells Activate: Africa' },
            { deck:'Threat', name:'Saigon'       , affiliation:'Allied' , region:'Asia'         , incident:'Recognized: Asia, Pacific Rim' },
            { deck:'Threat', name:'San Francisco', affiliation:'Allied' , region:'North America', incident:'Sleeper Cells Activate: South America' },
            { deck:'Threat', name:'Santiago'     , affiliation:'Neutral', region:'South America', incident:'Sleeper Cells Activate: Africa' },
            { deck:'Threat', name:'São Paulo'    , affiliation:'Neutral', region:'South America', incident:'Teams Compromised' },
            { deck:'Threat', name:'Shanghai'     , affiliation:'Soviet' , region:'Asia'         , incident:'Teams Compromised' },
            { deck:'Threat', name:'Sydney'       , affiliation:'Allied' , region:'Pacific Rim'  , incident:'Teams Compromised' },
            { deck:'Threat', name:'Tokyo'        , affiliation:'Neutral', region:'Pacific Rim'  , incident:'Sleeper Cells Activate: North America' },
            { deck:'Threat', name:'Toronto'      , affiliation:'Allied' , region:'North America', incident:'Sleeper Cells Activate: Africa' },
            { deck:'Threat', name:'Warsaw'       , affiliation:'Soviet' , region:'Europe'       , incident:'Sleeper Cells Activate: Pacific Rim' },
            { deck:'Threat', name:'Washington'   , affiliation:'Allied' , region:'North America', incident:'Teams Compromised' },
        ],
        playerDeck: {
            cityCards: 48,
            escalationCards: 5,
            fundingCards: 5,
            objectiveCards: 1,
            resourceCards: 0,
        },
    });
    saveGame();
}

function resetGame() {
    window.localStorage.removeItem(LOCAL_STORAGE_KEY);
    location.reload();
}

function saveGame() {
    window.localStorage.setItem(LOCAL_STORAGE_KEY, JSON.stringify(game.saveData));
}
