const INITIAL_GAME = {
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
        fundingCards: 5,
        objectiveCards: 1,
        satelliteCards: 0,
    },
};

const LOCAL_STORAGE_KEY = 'pandemic0';

const PURPOSE = {
    DELETED: 'Deleted',
    DESTROYED: 'Destroyed',
    DISCARD: 'Discard Pile',
    GAME_END: 'Game End',
    THREAT: 'Threat',
};

class Cell {
    _node;
    _span;

    onClick;

    get classList() { return this._node.classList; }

    get innerHTML() { return this._span.innerHTML; }
    set innerHTML(value) { this._span.innerHTML = value; }

    get node() { return this._node; }

    constructor(innerHTML, ...classList) {
        this._span = document.createElement('span');
        this._span.innerHTML = innerHTML;

        this._node = document.createElement('div');
        this._node.classList.add('cell', ...classList);
        this._node.appendChild(this._span);
        this._node.onclick = e => this.onClick ? this.onClick(e) : true;
    }

    appendChild(newChild) {
        this._span.appendChild(newChild);
    }
}

class ThreatCard {
    _affiliation;
    _cell;
    _deck;
    _deckName;
    _incident;
    _name;
    _region;

    onClick;

    constructor(data) {
        this._cell = new Cell();
        this._cell.onClick = e => this.onClick ? this.onClick(e) : true;

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
            this._cell.classList.remove(this._affiliation.toLowerCase());
        }
        this._affiliation = value;
        if (this._affiliation) {
            this._cell.classList.add(this._affiliation.toLowerCase());
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
        if (this._deck) {
            this._deck.addCard(this);
        }
    }

    get deckName() {
        if (!this.deck) { return this._deckName; }
        switch(this.deck.purpose) {
            case PURPOSE.DISCARD:
            case PURPOSE.THREAT:
            case PURPOSE.GAME_END:
                return this.affiliation === 'Infection' ? PURPOSE.DISCARD : PURPOSE.THREAT;
            case PURPOSE.DELETED:
                return null;
            default:
                return this.deck.purpose;
        }
    }
    set deckName(value) { this._deckName = value; }

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
        this._cell.innerHTML = this._name;
    }

    get node() { return this._cell.node; }

    get region() { return this._region; }
    set region(value) {
        if (this._region === value) {
            return;
        }
        if (this._region) {
            this._cell.classList.remove(this._region.replace(' ', '-').toLowerCase());
        }
        this._region = value;
        if (this._region) {
            this._cell.classList.add(this._region.replace(' ', '-').toLowerCase());
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
    _isSelected = false;

    onSelected = null;

    constructor(purpose) {
        this._headingNode = document.createElement('span');

        this._countNode = document.createElement('span');
        this._countNode.style.fontSize = 'smaller';

        let titleSpan = document.createElement('span');
        titleSpan.appendChild(this._headingNode);
        titleSpan.appendChild(document.createElement('br'));
        titleSpan.appendChild(this._countNode);

        let titleDiv = document.createElement('div');
        titleDiv.classList.add('cell');
        titleDiv.appendChild(titleSpan);
        titleDiv.onclick = e => this.onSelected ? this.onSelected() : true;
    
        this._div = document.createElement('div');
        this._div.classList.add('phase', 'section');
        this._div.appendChild(titleDiv);

        this.purpose = purpose;
        this.heading = purpose;
        this._updateCount();
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

    get isSelected() { return this._isSelected; }
    set isSelected(value) {
        if (this._isSelected === value) {
            return;
        }
        this._isSelected = value;
        if (this._isSelected) {
            this._div.classList.add('selected');
        } else {
            this._div.classList.remove('selected');
        }
    }

    get node() { return this._div; }

    addCard(card) {
        let sibling = null;
        let i = 0;
        for (; i < this._cards.length; i++) {
            let other = this._cards[i];
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
        this._updateCount();
    }

    removeCard(card) {
        for (let i = this._cards.length - 1; i >= 0; i--) {``
            if (this._cards[i] === card) {
                this._cards.splice(i, 1);
                break;
            }
        }
        card.node.remove();
        card.parent = null;
        this._updateCount();
    }

    _updateCount() {
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
            this._selectedDeck.isSelected = false;
        }
        this._selectedDeck = value;
        if (this._selectedDeck) {
            this._selectedDeck.isSelected = true;
        }
    }

    startNewGame(cards) {
        while(this._decks.length > 0) {
            this.remove(this._decks[0]);
        }

        [ PURPOSE.DISCARD, PURPOSE.THREAT, PURPOSE.GAME_END, PURPOSE.DESTROYED, PURPOSE.DELETED ]
            .forEach(purpose => this._addToBottom(new ThreatDeck(purpose)), this);

        this.selectedDeck = this._decks[0];

        cards.forEach(card => card.deck = this._decks.find(deck => deck.purpose === card.deckName), this);
    }

    _addToBottom(deck) {
        deck.parent = this;
        this._decks.push(deck);
        this._div.appendChild(deck.node);
        deck.onSelected = e => this.selectedDeck = deck;
    }

    addToTop(deck) {
        deck.parent = this;
        this._decks.unshift(deck);
        this._div.prepend(deck.node);
        deck.onSelected = e => this.selectedDeck = deck;
    }

    remove(deck) {
        let i = this._decks.indexOf(deck);
        if (i < 0) {
            return;
        }
        deck.node.remove();
        this._decks.splice(i, 1);
        deck.parent = null;
        delete deck.onSelected;
        if (deck.isSelected) {
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
        addCardDiv.onclick = e => this._addCard();

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

    _addCard() {
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

class SetupPanel {
    _fundingCards;
    _fundingCardsInput;
    _newGameButton;
    _node;
    _objectiveCards;
    _objectiveCardsInput;
    _playerInputs;
    _players = new Array();
    _playersInput;
    _satelliteCards;
    _satelliteCardsInput;

    get cityCards() { return 48; }

    get fundingCards() { return this._fundingCards }
    set fundingCards(value) {
        if (this._fundingCards === value) {
            return;
        }
        this._fundingCardsInput.value = value;
        this._fundingCards = value;
    }

    get isValid() { return this.startingCards && this.playerCards > 0; }

    get node() { return this._node; }

    get objectiveCards() { return this._objectiveCards }
    set objectiveCards(value) {
        if (this._objectiveCards === value) {
            return;
        }
        this._objectiveCardsInput.value = value;
        this._objectiveCards = value;
    }

    get playerCards() { return this.cityCards - this.objectiveCards + this.fundingCards - this.startingCards; }

    get players() { return this._players; }

    get satelliteCards() { return this._satelliteCards }
    set satelliteCards(value) {
        if (this._satelliteCards === value) {
            return;
        }
        this._satelliteCardsInput.value = value;
        this._satelliteCards = value;
    }

    get saveData() {
        return {
            fundingCards: this.fundingCards,
            objectiveCards: this.objectiveCards,
            satelliteCards: this.satelliteCards,
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

    constructor() {
        this._cityCardsInput = document.getElementById('cityCardsInput');
        this._escalationCardsInput = document.getElementById('escalationCardsInput');
        this._fundingCardsInput = document.getElementById('fundingCardsInput');
        this._node = document.getElementById('setupPanel');
        this._objectiveCardsInput = document.getElementById('objectiveCardsInput');
        this._playersInput = document.getElementById('playersInput');
        this._satelliteCardsInput = document.getElementById('satelliteCardsInput');
        this._playerInputs = [...document.getElementsByName('playerInput')];

        this._fundingCardsInput.onchange    = e => this._fundingCards    = parseInt(this._fundingCardsInput.value   );
        this._objectiveCardsInput.onchange  = e => this._objectiveCards  = parseInt(this._objectiveCardsInput.value );
        this._satelliteCardsInput.onchange   = e => this._satelliteCards   = parseInt(this._satelliteCardsInput.value  );
        this._playerInputs.forEach(playerInput => playerInput.onchange = e => this._playerInputChanged(playerInput), this);
        this._newGameButton = document.getElementById('newGameButton');
    }

    loadFrom(data) {
        this.fundingCards = data.fundingCards;
        this.objectiveCards = data.objectiveCards;
        this.satelliteCards = data.satelliteCards;
        this._players = new Array();
        this._playerInputs.forEach(playerInput => playerInput.checked = false);
        this._updatePlayersInput();
    }

    _playerInputChanged(playerInput) {
        if (playerInput.checked) {
            this._addPlayer(playerInput.value);
        } else {
            this._removePlayer(playerInput.value);
        }
    }

    _addPlayer(color) {
        this._players.push(color);
        this._updatePlayersInput();
    }

    _removePlayer(color) {
        let i = this._players.indexOf(color);
        if (i < 0) {
            return;
        }
        this._players.splice(i, 1);
        this._updatePlayersInput();
    }

    _updatePlayersInput() {
        this._playersInput.value = this._players.join(',');
    }
}

class PlayerDeck {
    _cardStacks;
    _currentPlayer;
    _escalationTurnsOutput;
    _node;
    _pawn;
    _playerCardCountCell;
    playerCards;
    players;
    satelliteCards;
    _threatLevel;
    _threatLevelOutput;

    onNextPhase;

    get escalationCards() { return 5; }

    get node() { return this._node; }

    get threatLevel() { return this._threatLevel; }
    set threatLevel(value) {
        if (this._threatLevel === value) {
            return;
        }
        this._threatLevel = value;
        this._threatLevelOutput.innerHTML = value;
    }

    constructor() {

        let createLabel = function(text) {
            let label = document.createElement('span');
            label.classList.add('label');
            label.appendChild(document.createTextNode(text));
            return label;
        };

        let createValue = function() {
            let value = document.createElement('span');
            value.classList.add('value');
            return value;
        };

        let createPawnSvg = function() {
            let head = document.createElementNS('http://www.w3.org/2000/svg', 'circle');
            head.setAttribute('cx', 2);
            head.setAttribute('cy', 1);
            head.setAttribute('r' , 1);

            let body = document.createElementNS('http://www.w3.org/2000/svg', 'path');
            body.setAttribute('d', 'M1 4 L2 0 L3 4 Z');

            let pawn = document.createElementNS('http://www.w3.org/2000/svg', 'svg');
            pawn.classList.add('pawn');
            pawn.setAttribute('viewBox', '0 0 4 4');
            pawn.setAttribute('fill', 'white');
            pawn.setAttribute('stroke', 'transparent');
            pawn.appendChild(body);
            pawn.appendChild(head);
            return pawn;
        };

        let escalationButton = new Cell('Escalation!', 'red', 'escalation');
        escalationButton.onClick = e => this.onNextPhase ? this.onNextPhase(true, true) : true;

        let newPhaseButton = new Cell('New Phase', 'yellow');
        newPhaseButton.onClick = e => this.onNextPhase ? this.onNextPhase(false, false) : true;

        this._pawn = createPawnSvg();

        let nextTurnButton = new Cell('Next Turn', 'green');
        nextTurnButton.appendChild(document.createElement('br'));
        nextTurnButton.appendChild(this._pawn);
        nextTurnButton.onClick = e => this.nextTurn();

        let skipEscalationButton = new Cell('Skip Escalation', 'blue');
        skipEscalationButton.onClick = e => this.nextEscalation();

        this._playerCardCountCell = new Cell('0');

        let drawPlayerCardButton = new Cell('Draw extra player card', 'blue');
        drawPlayerCardButton.onClick = e => this.drawPlayerCards(1);

        this._escalationTurnsOutput = createValue();
        this._threatLevelOutput = createValue();
        this.threatLevel = 2;

        let escalationInfoNode = document.createElement('p');
        escalationInfoNode.classList.add('info');
        escalationInfoNode.appendChild(createLabel('Escalation '));
        escalationInfoNode.appendChild(this._escalationTurnsOutput);
        escalationInfoNode.appendChild(document.createElement('br'));
        escalationInfoNode.appendChild(createLabel('Threat Level: '));
        escalationInfoNode.appendChild(this._threatLevelOutput);

        this._node = document.createElement('div');
        this._node.classList.add('section');
        this._node.appendChild(escalationButton.node);
        this._node.appendChild(newPhaseButton.node);
        this._node.appendChild(nextTurnButton.node);
        this._node.appendChild(skipEscalationButton.node);
        this._node.appendChild(this._playerCardCountCell.node);
        this._node.appendChild(drawPlayerCardButton.node);
        this._node.appendChild(escalationInfoNode);
    }

    startNewGame() {
        this._currentPlayer = 0;
        this._recalculatePawnColor();

        this._cardStacks = new Array(this.escalationCards);
        
        for(let i = this.escalationCards - 1; i >= 0; i--) {
            this._cardStacks[i] = 1;
        }

        for (let cards = this.satelliteCards; cards > 0; cards--) {
            this._cardStacks[this.escalationCards - cards]++;
        }

        for (let cards = this.playerCards - 1; cards >= 0; cards--) {
            this._cardStacks[cards % this.escalationCards]++;
        }

        this._cardStacks = this._cardStacks.map(cards => ({ cards: cards, escalation: true }));

        this._recalculatePlayerCards();
        this._recalculateEscalationTurns();
    }

    nextTurn() {
        this._currentPlayer = (this._currentPlayer + 1) % this.players.length;
        this._recalculatePawnColor();
        this.drawPlayerCards(2);
    }
    
    _recalculatePawnColor() {
        this._pawn.style.fill = this.players[this._currentPlayer];
    }

    drawPlayerCards(drawCards) {
        while (drawCards > 0) {
            let cardStack = this._cardStacks.find(stack => stack.cards > 0);
            if (!cardStack) { break; }
            if (cardStack.cards > drawCards) {
                cardStack.cards = cardStack.cards - drawCards;
                drawCards = 0;
            } else {
                drawCards = drawCards - cardStack.cards;
                cardStack.cards = 0;
            }
        }
        this._recalculatePlayerCards();
        this._recalculateEscalationTurns();
    }
    
    _recalculatePlayerCards() {
        let count = 0;
        this._cardStacks.forEach(stack => count = count + stack.cards);
        this._playerCardCountCell.innerHTML = `${count} cards`;
    }
    
    nextEscalation() {
        let cardStack = this._cardStacks.find(stack => stack.escalation);
        if (!cardStack) {
            return;
        }
        cardStack.escalation = false;
        this._recalculateEscalationTurns();
    }
    
    _recalculateEscalationTurns() {
        let safeCards = 0;
        let escalationStack = this._cardStacks.find(stack => {
            if (stack.escalation) {
                return true;
            }
            safeCards = safeCards + stack.cards;
            return false;
        });
    
        if (escalationStack) {
            let firstTurn = Math.ceil((safeCards + 1) / 2);
            let lastTurn = Math.ceil((safeCards + escalationStack.cards) / 2);
            this._escalationTurnsOutput.innerHTML
                = lastTurn < 1     ? 'is late'
                : lastTurn === 1   ? 'on this turn'
                : firstTurn === 1  ? 'within ' + lastTurn + ' turns'
                                : 'in ' + firstTurn + ' to ' + lastTurn + ' turns';
        } else {
            this._escalationTurnsOutput.innerHTML = 'never again!'
        }
    }
}

class Game {
    _addCardPanel = new AddCardPanel();
    _cards;
    _drawPlayerCardButton;
    _escalationButton;
    _newGameButton;
    _newPhaseButton;
    _nextTurnButton;
    _playerDeck = new PlayerDeck();
    _setupPanel = new SetupPanel();
    _skipEscalationButton;
    _threatDecks = new ThreatDecks();
    _threatLevel;
    _threatLevels;

    constructor() {
        this._drawPlayerCardButton = document.getElementById('drawPlayerCardButton')
        this._escalationButton = document.getElementById('escalationButton');
        this._newGameButton = document.getElementById('newGameButton');
        this._newPhaseButton = document.getElementById('newPhaseButton');
        this._nextTurnButton = document.getElementById('nextTurnButton');
        this._skipEscalationButton = document.getElementById('skipEscalationButton');
        this._threatLevelOutput = document.getElementById('threatLevelOutput');

        this._addCardPanel.onAddCard = card => this._addCard(card);
        this._newGameButton.onclick = e => this._newGameButtonClicked();
        this._playerDeck.onNextPhase = (escalation, accelerate) => this._nextPhase(escalation, accelerate);
    }

    get saveData() {
        return {
            cards: this._cards.filter(card => card.deckName).map(card => card.saveData),
            playerDeck: this._setupPanel.saveData,
        };
    }

    get threatLevel() { return this._threatLevel; }
    set threatLevel(value) {
        if (this._threatLevel === value) {
            return;
        }
        this._threatLevel = value;
        this._playerDeck.threatLevel = this.threatLevel;
    }

    loadFrom(data) {
        this._setupPanel.loadFrom(data.playerDeck);
        this._cards = data.cards.map(cardData => {
            const card = new ThreatCard(cardData);
            card.onClick = e => this._cardClicked(card)
            return card;
        }, this);
    }

    _addCard(card) {
        card.onClick = e => this._cardClicked(card)
        this._cards.push(card);
        card.deck = this._threatDecks.selectedDeck;
        saveGame();
    }

    _newGameButtonClicked() {
        this.startNewGame();
    }

    startNewGame() {
        if (!this._setupPanel.isValid) {
            alert('Game settings are invalid.');
            return;
        }

        this._playerDeck._escalationCards = this._setupPanel.escalationCards;
        this._playerDeck.players = this._setupPanel.players;
        this._playerDeck.playerCards = this._setupPanel.playerCards;
        this._playerDeck.satelliteCards = this._setupPanel.satelliteCards;
        this._playerDeck.startNewGame();
        this._threatDecks.startNewGame(this._cards);
        this._threatLevels = [2, 2, 2, 3, 3, 4];
        this.threatLevel = this._threatLevels.shift();
        this._recalculateCountdowns();

        document.body.appendChild(this._playerDeck.node);
        document.body.appendChild(this._threatDecks.node);
        document.body.appendChild(this._addCardPanel.node);
        this._setupPanel.node.remove();

        saveGame();
    }

    _nextPhase(escalation, accelerate) {
        if (this._threatDecks.discardPile.count === 0) {
            return;
        }
        if (escalation) {
            this._playerDeck.nextEscalation();
        }
        if (accelerate) {
            if (this._threatLevels.length > 0) {
                this.threatLevel = this._threatLevels.shift();
            }
        }
    
        this._threatDecks.discardPile.purpose = PURPOSE.THREAT;
        const discardPile = new ThreatDeck(PURPOSE.DISCARD);
        this._threatDecks.addToTop(discardPile);
        this._threatDecks.selectedDeck = discardPile;

        this._recalculateCountdowns();
    }
    
    _cardClicked(card) {
        if (card.deck.purpose === PURPOSE.THREAT && card.deck.count === 1) {
            this._threatDecks.remove(card.deck);
        }
        let oldDeckName = card.deckName;
        card.deck = this._threatDecks.selectedDeck;
        if (oldDeckName !== card.deckName) {
            saveGame();
        }
        this._recalculateCountdowns();
    }

    _recalculateCountdowns() {
        let totalThreats = 0;
        this._threatDecks.drawPile.forEach(deck => {
            totalThreats = totalThreats + 1;
            let firstTurn = Math.ceil(totalThreats / this.threatLevel);
            totalThreats = totalThreats + deck.count - 1;
            let lastTurn = Math.ceil(totalThreats / this.threatLevel);
            if (lastTurn < firstTurn) {
                lastTurn = firstTurn;
            }

            deck.heading
                = lastTurn === 1 ? lastTurn + ' turn'
                : firstTurn === lastTurn ? firstTurn + ' turns'
                : firstTurn + ' - ' + lastTurn + ' turns';
        });
    }
}

let game;

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
    let json = window.localStorage.getItem(LOCAL_STORAGE_KEY);
    if (json) {
        try {
            game.loadFrom(JSON.parse(json));
            console.log('Game loaded.');
            return;
        }
        catch { }
    }

    game.loadFrom(INITIAL_GAME);
    console.log('New campaign started.');
    saveGame();
}

function resetGame() {
    window.localStorage.removeItem(LOCAL_STORAGE_KEY);
    console.log('Game reset.');
    location.reload();
}

function saveGame() {
    window.localStorage.setItem(LOCAL_STORAGE_KEY, JSON.stringify(game.saveData));
    console.log('Game saved.');
}
