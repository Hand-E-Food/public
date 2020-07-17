var addCardButton;
var addCardDiv;
var body;
var cardColorInput;
var cardInfectionInput;
var cardNameInput;
var cardStacks;
var cardUpgradeInput;
var cityCardsInput;
var currentPawn;
var discardPileDiv;
var epidemicButton;
var epidemicDiv;
var epidemicsInput;
var epidemicTurnsOutput;
var fundingCardsInput;
var infectionRateOutput;
var infectionRates;
var newGameButton;
var newGameDiv;
var newPhaseButton;
var nextTurnButton;
var pawn;
var pawnColors;
var pawnColorsInput;
var pawnInputs;
var resourceCardsInput;
var selectedCard;
var selectedDiv;

const deck = {
    destroyed: 'Destroyed',
    gameEnd: 'Game End',
    infection: 'Infection',
    innoculated: 'Innoculated',
    reserve: 'Reserve',
};

window.onload = () => {
    window.onload = undefined;

    addCardButton = document.getElementById('addCardButton');
    addCardDiv = document.getElementById('addCardDiv');
    body = document.getElementById('body');
    cardColorInput = document.getElementById('cardColorInput');
    cardInfectionInput = document.getElementById('cardInfectionInput');
    cardNameInput = document.getElementById('cardNameInput');
    cardUpgradeInput = document.getElementById('cardUpgradeInput');
    cityCardsInput = document.getElementById('cityCardsInput');
    epidemicButton = document.getElementById('epidemicButton');
    epidemicDiv = document.getElementById('epidemicDiv');
    epidemicsInput = document.getElementById('epidemicsInput');
    epidemicTurnsOutput = document.getElementById('epidemicTurnsOutput');
    fundingCardsInput = document.getElementById('fundingCardsInput');
    infectionRateOutput = document.getElementById('infectionRateOutput');
    newGameButton = document.getElementById('newGameButton');
    newGameDiv = document.getElementById('newGameDiv');
    newPhaseButton = document.getElementById('newPhaseButton');
    nextTurnButton = document.getElementById('nextTurnButton');
    pawn = document.getElementById('pawn');
    pawnColorsInput = document.getElementById('pawnColorsInput');
    pawnInputs = [...document.getElementsByName('pawnInput')];
    resourceCardsInput = document.getElementById('resourceCardsInput');

    addCardButton.onclick = e => addCard();
    cityCardsInput.onchange = e => refreshEpidemicsInput();
    epidemicButton.onclick = e => nextPhase(true, true);
    newGameButton.onclick = e => newGameButtonOnClick();
    newPhaseButton.onclick = e => nextPhase(false, false);
    nextTurnButton.onclick = e => nextTurn();
    pawnInputs.forEach(pawnInput => pawnInput.onchange = e => setPawn(pawnInput));

    cityCardsInput.value =  window.localStorage.getItem('cityCards');
    fundingCardsInput.value =  window.localStorage.getItem('fundingCards');
    resourceCardsInput.value =  window.localStorage.getItem('resourceCards');
    
    refreshEpidemicsInput();
}

function refreshEpidemicsInput() {
    var cityCards = parseInt(cityCardsInput.value);
    epidemicsInput.value = cityCards
        ? calculateEpidemicCards(cityCards)
        : null;
}

function setPawn(pawnInput) {
    var pawnColors = pawnColorsInput.value.split(',');
    if (pawnInput.checked) {
        pawnColors.push(pawnInput.value);
    } else {
        pawnColors = pawnColors.filter(pawnColor => pawnColor != pawnInput.value);
    }
    pawnColorsInput.value = pawnColors.join(',');
}

function newGameButtonOnClick() {
    if (epidemicDiv.classList.contains('hidden') || confirm('Start a new game?')) {
        startNewGame();
    }
}

function startNewGame() {
    newGameDiv.classList.add('hidden');
    getAllInfectionPhases().forEach(phase => body.removeChild(phase));

    pawnColors = pawnColorsInput.value.split(',');
    currentPawn = 0;
    recalculatePawnColor();

    var cityCards = parseInt(cityCardsInput.value);
    var resourceCards = parseInt(resourceCardsInput.value);
    var fundingCards = parseInt(fundingCardsInput.value);
    window.localStorage.setItem('cityCards', cityCards);
    window.localStorage.setItem('resourceCards', resourceCards);
    window.localStorage.setItem('fundingCards', fundingCards);

    var epidemicCards = calculateEpidemicCards(cityCards);
    var startingCards = calculateStartingCards(pawnColors.length);
    var totalCards = cityCards + resourceCards + fundingCards + epidemicCards - startingCards;
    cardStacks = new Array(epidemicCards);
    for(var i = 0; i < epidemicCards; i++) {
        cardStacks[i] = 0;
    }
    for (var playerCards = totalCards - 1; playerCards >= 0; playerCards--) {
        cardStacks[playerCards % epidemicCards]++;
    }
    cardStacks = [0, ...cardStacks].map(cards => ({ cards: cards, epidemic: true }));
    
    infectionRates = [2, 2, 2, 3, 3, 4, 4, 5];

    createPhase(deck.destroyed);
    createPhase(deck.reserve);
    createPhase(deck.gameEnd);
    createPhase(deck.innoculated);
    discardPileDiv = createPhase(deck.infection);

    var cards = loadCards();
    console.log(cards);

    cards
        .filter(city => city.infection)
        .sort((a, b) => a.name < b.name ? -1 : 1)
        .forEach(createCard);

    nextPhase(true, true);

    cards
        .filter(city => !city.infection)
        .sort((a, b) => a.name < b.name ? -1 : 1)
        .forEach(createCard);

    epidemicDiv.classList.remove('hidden');
    addCardDiv.classList.remove('hidden');
}

function calculateEpidemicCards(cityCards) {
    if (cityCards <= 36) {
        return 5;
    } else if (cityCards <= 44) {
        return 6;
    } else if (cityCards <= 51) {
        return 7;
    } else if (cityCards <= 57) {
        return 8;
    } else if (cityCards <= 62) {
        return 9;
    } else {
        return 10;
    }
}

function calculateStartingCards(playerCount) {
    if (playerCount === 2) {
        return 2 * 4;
    } else if (playerCount === 3) {
        return 3 * 3;
    } else if (playerCount === 4) {
        return 4 * 2;
    } else {
        throw new Error('Invalid number of players.');
    }
}

function loadCards() {
    var cards = window.localStorage.getItem('cards');
    if (!cards) {
        return defaultCards;
    }
    try {
        return JSON.parse(cards);
    }
    catch {
        return defaultCards;
    }
}

function nextPhase(epidemic, accelerate) {
    if (discardPileDiv.querySelector('.button').length === 0) {
        return;
    }

    if (epidemic) {
        var cardStack = cardStacks.find(stack => stack.epidemic);
        if (!cardStack) {
            return;
        }
        cardStack.epidemic = false;
    }
    if (accelerate) {
        if (infectionRates.length > 0)
            infectionRateOutput.innerHTML = infectionRates.shift();
        recalculateEpidemicTurns();
    }

    discardPileDiv = createPhase(deck.infection);
    selectPhase(discardPileDiv);
    
    recalculateCountdowns();
}

function createPhase(deck) {
    var countdownSpan = document.createElement('span');
    countdownSpan.id = 'countdown';
    countdownSpan.innerHTML = deck;

    var countdownDiv = document.createElement('div');
    countdownDiv.classList.add('cell');
    countdownDiv.appendChild(countdownSpan);

    var phaseDiv = document.createElement('div');
    phaseDiv.classList.add('phase');
    phaseDiv.classList.add('section');
    phaseDiv.dataset.deck = deck;
    phaseDiv.appendChild(countdownDiv);

    var onclick = e => selectPhase(phaseDiv);
    countdownSpan.onclick = onclick;
    countdownDiv.onclick = onclick;
    
    body.insertBefore(phaseDiv, epidemicDiv.nextSibling);

    return phaseDiv;
}

function selectPhase(phase) {
    if (selectedDiv) {
        selectedDiv.classList.remove('selected');
    }

    selectedDiv = phase;
    
    selectedDiv.classList.add('selected');
}

function addCard() {
    var card = {
        color: cardColorInput.value,
        deck: selectedDiv.dataset.deck,
        infection: cardInfectionInput.checked,
        name: cardNameInput.value,
        upgrade: cardUpgradeInput.value,
    };
    createCard(card);
    saveCards();
}

function createCard(card) {
    var cardSpan = document.createElement('span');
    cardSpan.appendChild(document.createTextNode(card.name));
    if (card.upgrade) {
        cardSpan.appendChild(document.createElement('br'));
        var cardUpgrade = document.createElement('span');
        cardUpgrade.classList.add('upgrade');
        cardUpgrade.appendChild(document.createTextNode(card.upgrade));
        cardSpan.appendChild(cardUpgrade);
    }

    var cardDiv = document.createElement('div');
    cardDiv.dataset.color = card.color;
    cardDiv.dataset.deck = card.deck;
    cardDiv.dataset.infection = card.infection;
    cardDiv.dataset.name = card.name;
    if (card.upgrade) { cardDiv.dataset.upgrade = card.upgrade; }
    cardDiv.classList.add(card.color);
    cardDiv.classList.add('button');
    cardDiv.classList.add('cell');
    cardDiv.appendChild(cardSpan);

    var pileDiv = getAllPhases().find(phase => phase.dataset.deck === card.deck);
    if (!pileDiv) { pileDiv = discardPileDiv; }
    pileDiv.appendChild(cardDiv);

    var onclick = e => moveCard(cardDiv);
    cardSpan.onclick = onclick;
    cardDiv.onclick = onclick;
}

function moveCard(cityDiv) {
    var phaseDiv = cityDiv.parentNode
    if (phaseDiv === selectedDiv) {
        return;
    }

    phaseDiv.removeChild(cityDiv);
    if (phaseDiv.dataset.deck === deck.infection
        && phaseDiv !== discardPileDiv
        && phaseDiv.childNodes.length === 1
    ) {
        phaseDiv.parentNode.removeChild(phaseDiv);
    }
    
    var existingCityDiv;
    for(var i = 1; i < selectedDiv.childNodes.length; i++) {
        if (selectedDiv.childNodes[i].firstChild.innerHTML > cityDiv.firstChild.innerHTML) {
            existingCityDiv = selectedDiv.childNodes[i];
            break;
        }
    }
    selectedDiv.insertBefore(cityDiv, existingCityDiv);

    if (cityDiv.dataset.deck !== selectedDiv.dataset.deck) {
        cityDiv.dataset.deck = selectedDiv.dataset.deck;
        saveCards();
    }

    if (getAllInfectionPhases().length === 1) {
        nextPhase(false, true);
    }

    recalculateCountdowns();
}

function saveCards() {
    var cards = getAllPhases()
        .map(phase => [...phase.childNodes].slice(1))
        .flat()
        .map(card => card.dataset)
        .map(card => {
            return {
                color: card.color,
                deck: getSaveDeck(card.deck),
                infection: card.infection === 'true',
                name: card.name,
                upgrade: card.upgrade,
            };
        });
    
    window.localStorage.setItem('cards', JSON.stringify(cards));
}

function getSaveDeck(deckName) {
    if (deckName === deck.gameEnd) {
        return deck.infection;
    } else {
        return deckName;
    }
}

function nextTurn() {
    currentPawn = (currentPawn + 1) % pawnColors.length;
    recalculatePawnColor();
 
    var drawCards = 2;
    while (drawCards > 0) {
        var cardStack = cardStacks.find(stack => stack.cards > 0);
        if (!cardStack) { break; }
        if (cardStack.cards > drawCards) {
            cardStack.cards = cardStack.cards - drawCards;
            drawCards = 0;
        } else {
            drawCards = drawCards - cardStack.cards;
            cardStack.cards = 0;
        }
    }
    recalculateEpidemicTurns();
}

function recalculatePawnColor() {
    pawn.style.fill = pawnColors[currentPawn];
}

function recalculateEpidemicTurns() {
    var safeCards = 0;
    var epidemicStack = cardStacks.find(stack => {
        if (stack.epidemic) {
            return true;
        }
        safeCards = safeCards + stack.cards;
        return false;
    });

    var firstTurn = Math.ceil((safeCards + 1) / 2);
    var lastTurn = Math.ceil((safeCards + epidemicStack.cards) / 2);
    epidemicTurnsOutput.innerHTML
        = !epidemicStack  ? 'never again!'
        : lastTurn < 1    ? 'is late'
        : lastTurn === 1  ? 'on this turn'
        : firstTurn === 1 ? 'within ' + lastTurn + ' turns'
                          : 'in ' + firstTurn + ' to ' + lastTurn + ' turns';
}

function recalculateCountdowns() {
    var totalInfections = 0;
    var rate = parseInt(infectionRateOutput.innerHTML);
    getAllInfectionPhases().forEach(phase => {
        if (phase === discardPileDiv) {
            getCountdown(phase).innerHTML = 'Dicard<br/>Pile';
        } else {
            totalInfections = totalInfections + 1;
            var firstTurn = Math.ceil(totalInfections / rate);
            totalInfections = totalInfections + [...phase.childNodes].filter(node => node.dataset.infection == 'true').length - 1;
            var lastTurn = Math.ceil(totalInfections / rate);
            if (lastTurn < firstTurn) {
                lastTurn = firstTurn;
            }

            getCountdown(phase).innerHTML =
                  lastTurn === 1 ? lastTurn + '<br/>turn'
                : firstTurn === lastTurn ? firstTurn + '<br/>turns'
                : firstTurn + ' - ' + lastTurn + '<br/>turns'
        }
    });


    var noCardsInDiscardPile = discardPileDiv.childNodes.length === 1;
    epidemicButton.disabled = noCardsInDiscardPile;
    newPhaseButton.disabled = noCardsInDiscardPile;
}

function getAllInfectionPhases() {
    return getAllPhases()
        .filter(phase => phase.dataset.deck === deck.infection);
}

function getAllPhases() {
    return [...document.getElementsByClassName('phase')];
}

function getCountdown(phase) {
    return phase.firstChild.firstChild;
}
