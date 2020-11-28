var body;
var cardStacks;
var cityCardsInput;
var currentPawn;
var discardPileDiv;
var epidemicButton;
var epidemicDiv;
var epidemicCardsInput;
var epidemicTurnsOutput;
var fundingCardsInput;
var infectionRateOutput;
var infectionRates;
var newPhaseButton;
var pawn;
var pawnColors;
var pawnColorsInput;
var playerCardsSpan;
var resourceCardsInput;
var selectedDiv;

const deck = {
    gameEnd: 'Game End',
    infection: 'Threat',
};

window.addEventListener("beforeunload", function(e) {
    e.preventDefault();
    e.returnValue = '';
});

window.onload = () => {
    window.onload = undefined;

    body = document.getElementById('body');
    cityCardsInput = document.getElementById('cityCardsInput');
    epidemicButton = document.getElementById('epidemicButton');
    epidemicDiv = document.getElementById('epidemicDiv');
    epidemicCardsInput = document.getElementById('epidemicCardsInput');
    epidemicTurnsOutput = document.getElementById('epidemicTurnsOutput');
    fundingCardsInput = document.getElementById('fundingCardsInput');
    infectionRateOutput = document.getElementById('infectionRateOutput');
    newPhaseButton = document.getElementById('newPhaseButton');
    pawn = document.getElementById('pawn');
    pawnColorsInput = document.getElementById('pawnColorsInput');
    playerCardsSpan = document.getElementById('playerCardsSpan');
    resourceCardsInput = document.getElementById('resourceCardsInput');

    document.getElementById('drawPlayerCardButton').onclick = e => drawPlayerCards(1);
    document.getElementById('newGameButton').onclick = e => newGameButtonOnClick();
    document.getElementById('nextTurnButton').onclick = e => nextTurn();
    document.getElementById('skipEpidemicButton').onclick = e => nextEpidemic();
    [...document.getElementsByName('pawnInput')].forEach(pawnInput => pawnInput.onchange = e => setPawn(pawnInput));
    cityCardsInput.onchange = e => savePlayerDeck();
    epidemicButton.onclick = e => nextPhase(true, true);
    epidemicCardsInput.onchange = e => savePlayerDeck();
    fundingCardsInput.onchange = e => savePlayerDeck();
    newPhaseButton.onclick = e => nextPhase(false, false);
    resourceCardsInput.onchange = e => savePlayerDeck();

    cityCardsInput.value = window.localStorage.getItem('pandemic0/cityCards') || defaultCityCards;
    epidemicCardsInput.value = window.localStorage.getItem('pandemic0/epidemicCards') || defaultEpidemicCards;
    fundingCardsInput.value = window.localStorage.getItem('pandemic0/fundingCards') || defaultFundingCards;
    resourceCardsInput.value = window.localStorage.getItem('pandemic0/resourceCards') || defaultResourceCards;
    
    startNewGame();
}

function savePlayerDeck() {
    window.localStorage.setItem('pandemic0/cityCards', cityCardsInput.value);
    window.localStorage.setItem('pandemic0/resourceCards', resourceCardsInput.value);
    window.localStorage.setItem('pandemic0/fundingCards', fundingCardsInput.value);
    window.localStorage.setItem('pandemic0/epidemicCards', epidemicCardsInput.value);
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
    if (confirm('Start a new game?')) {
        startNewGame();
    }
}

function startNewGame() {
    getAllPhases().forEach(phase => body.removeChild(phase));

    pawnColors = pawnColorsInput.value.split(',');
    currentPawn = 0;
    recalculatePawnColor();

    var cityCards = parseInt(cityCardsInput.value);
    var resourceCards = parseInt(resourceCardsInput.value);
    var fundingCards = parseInt(fundingCardsInput.value);
    var epidemicCards = parseInt(epidemicCardsInput.value);

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
    
    infectionRates = [2, 2, 2, 3, 3, 4];

    createPhase(deck.gameEnd);
    discardPileDiv = createPhase(deck.infection);

    var cards = loadCards();

    cards
        .sort((a, b) => a.name < b.name ? -1 : 1)
        .forEach(createCard);

    nextPhase(true, true);
    recalculatePlayerCards();
    recalculateCountdowns();
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
    return defaultCards;
    var cards = window.localStorage.getItem('pandemic0/cards');
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
        nextEpidemic();
    }

    if (accelerate) {
        if (infectionRates.length > 0) {
            infectionRateOutput.innerHTML = infectionRates.shift();
        }
        recalculateEpidemicTurns();
    }

    discardPileDiv = createPhase(deck.infection);
    selectPhase(discardPileDiv);
    
    recalculateCountdowns();
}

function nextEpidemic() {
    var cardStack = cardStacks.find(stack => stack.epidemic);
    if (!cardStack) {
        return;
    }
    cardStack.epidemic = false;
    recalculateEpidemicTurns();
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

    countdownDiv.onclick = e => selectPhase(phaseDiv);
    
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

function createCard(card) {
    var cardSpan = document.createElement('span');
    cardSpan.appendChild(document.createTextNode(card.name));

    var cardDiv = document.createElement('div');
    cardDiv.dataset.affiliation = card.affiliation;
    cardDiv.dataset.deck = card.deck;
    cardDiv.dataset.incident = card.incident;
    cardDiv.dataset.name = card.name;
    cardDiv.dataset.region = card.region;
    cardDiv.classList.add(card.affiliation.toLowerCase());
    cardDiv.classList.add(card.region.toLowerCase().replace(' ', '-'));
    cardDiv.classList.add('button');
    cardDiv.classList.add('cell');
    cardDiv.appendChild(cardSpan);

    var phaseDiv = getAllPhases().find(phase => phase.dataset.deck === card.deck);
    if (!phaseDiv) { phaseDiv = discardPileDiv; }
    putCard(cardDiv, phaseDiv);

    cardDiv.onclick = e => moveCard(cardDiv);
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
    
    putCard(cityDiv, selectedDiv);

    if (getAllInfectionPhases().length === 1) {
        nextPhase(false, true);
    }

    recalculateCountdowns();
}

function putCard(cityDiv, phaseDiv) {
    var existingCityDiv;
    for(var i = 1; i < phaseDiv.childNodes.length; i++) {
        var thisCityDiv = phaseDiv.childNodes[i];
        if (cityDiv.dataset.name < thisCityDiv.dataset.name) {
            existingCityDiv = thisCityDiv;
            break;
        }
    }
    phaseDiv.insertBefore(cityDiv, existingCityDiv);

    if (cityDiv.dataset.deck !== phaseDiv.dataset.deck) {
        cityDiv.dataset.deck = phaseDiv.dataset.deck;
        saveCards();
    }
}

function saveCards() {
    var cards = getAllPhases()
        .map(phase => [...phase.childNodes].slice(1))
        .flat()
        .map(card => card.dataset)
        .map(card => {
            return {
                deck: getSaveDeck(card.deck),
                name: card.name,
                affiliation: card.affiliation,
                region: card.region,
                incident: card.incident,
            };
        });
    
    window.localStorage.setItem('pandemic0/cards', JSON.stringify(cards));
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
 
    drawPlayerCards(2);
}

function recalculatePawnColor() {
    pawn.style.fill = pawnColors[currentPawn];
}

function drawPlayerCards(drawCards) {
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
    recalculatePlayerCards();
    recalculateEpidemicTurns();
}

function recalculatePlayerCards() {
    var count = 0;
    cardStacks.forEach(stack => count = count + stack.cards);
    playerCardsSpan.innerHTML = `${count} cards`;
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
        var cardCount = phase.childNodes.length - 1;
        if (phase === discardPileDiv) {
            getCountdown(phase).innerHTML = `Dicard Pile<br/>(${cardCount})`;
        } else {
            totalInfections = totalInfections + 1;
            var firstTurn = Math.ceil(totalInfections / rate);
            totalInfections = totalInfections + [...phase.childNodes].filter(node => isCountedCard(node.dataset)).length - 1;
            var lastTurn = Math.ceil(totalInfections / rate);
            if (lastTurn < firstTurn) {
                lastTurn = firstTurn;
            }

            getCountdown(phase).innerHTML = (
                  lastTurn === 1 ? lastTurn + ' turn'
                : firstTurn === lastTurn ? firstTurn + ' turns'
                : firstTurn + ' - ' + lastTurn + ' turns'
                ) + `<br/>(${cardCount})`;
        }
    });

    var zeroCardsInDiscardPile = discardPileDiv.childNodes.length === 1;
    epidemicButton.disabled = zeroCardsInDiscardPile;
    newPhaseButton.disabled = zeroCardsInDiscardPile;
}

function isCountedCard(card) {
    return true;
    // return card.infection == 'true'
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
