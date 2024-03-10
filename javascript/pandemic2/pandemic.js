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
var inoculatedDiv;
var manageCardDiv;
var newPhaseButton;
var pawn;
var pawnColors;
var pawnColorsInput;
var playerCardsSpan;
var resourceCardsInput;
var selectedDiv;

const deck = {
    destroyed: 'Destroyed',
    gameEnd: 'Game End',
    infection: 'Infection',
    inoculated: 'Inoculated',
    reserve: 'Reserve',
};

window.addEventListener("beforeunload", function(e) {
    e.preventDefault();
    e.returnValue = '';
});

window.onload = () => {
    window.onload = undefined;

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
    manageCardDiv = document.getElementById('manageCardDiv');
    newPhaseButton = document.getElementById('newPhaseButton');
    pawn = document.getElementById('pawn');
    pawnColorsInput = document.getElementById('pawnColorsInput');
    playerCardsSpan = document.getElementById('playerCardsSpan');
    resourceCardsInput = document.getElementById('resourceCardsInput');

    document.getElementById('addCardButton').onclick = e => addCard();
    document.getElementById('drawPlayerCardButton').onclick = e => drawPlayerCards(1);
    document.getElementById('manageCardHeaderDiv').onclick = e => selectPhase(manageCardDiv);
    document.getElementById('newGameButton').onclick = e => newGameButtonOnClick();
    document.getElementById('nextTurnButton').onclick = e => nextTurn();
    document.getElementById('skipEpidemicButton').onclick = e => nextEpidemic();
    [...document.getElementsByName('pawnInput')].forEach(pawnInput => pawnInput.onchange = e => setPawn(pawnInput));
    cityCardsInput.onchange = e => savePlayerDeck();
    epidemicButton.onclick = e => nextPhase(true, true);
    fundingCardsInput.onchange = e => savePlayerDeck();
    newPhaseButton.onclick = e => nextPhase(false, false);
    resourceCardsInput.onchange = e => savePlayerDeck();

    cityCardsInput.value =  window.localStorage.getItem('cityCards');
    fundingCardsInput.value =  window.localStorage.getItem('fundingCards');
    resourceCardsInput.value =  window.localStorage.getItem('resourceCards');
    
    refreshEpidemicsInput();
    startNewGame();
}

function savePlayerDeck() {
    window.localStorage.setItem('cityCards', cityCardsInput.value);
    window.localStorage.setItem('resourceCards', resourceCardsInput.value);
    window.localStorage.setItem('fundingCards', fundingCardsInput.value);
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
    inoculatedDiv = createPhase(deck.inoculated);
    discardPileDiv = createPhase(deck.infection);

    var cards = loadCards();

    cards
        .filter(city => city.infection)
        .sort((a, b) => a.name < b.name ? -1 : 1)
        .forEach(createCard);

    nextPhase(true, true);

    cards
        .filter(city => !city.infection)
        .sort((a, b) => a.name < b.name ? -1 : 1)
        .forEach(createCard);
    
    recalculatePlayerCards();
    recalculateCountdowns();
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
    
    if (selectedDiv === manageCardDiv) {
        cardNameInput.value = cityDiv.dataset.name;
        cardColorInput.value = cityDiv.dataset.color;
        cardInfectionInput.checked = cityDiv.dataset.infection == 'true';
        cardUpgradeInput.value = cityDiv.dataset.upgrade ? cityDiv.dataset.upgrade : '';
        saveCards();
        selectPhase(phaseDiv);
    } else {
        putCard(cityDiv, selectedDiv);
    }

    if (getAllInfectionPhases().length === 1) {
        nextPhase(false, true);
    }

    recalculateCountdowns();
}

function putCard(cityDiv, phaseDiv) {
    var existingCityDiv;
    for(var i = 1; i < phaseDiv.childNodes.length; i++) {
        var thisCityDiv = phaseDiv.childNodes[i];
        if (cityDiv.dataset.infection < thisCityDiv.dataset.infection) {
            existingCityDiv = thisCityDiv;
            break;
        } else if (cityDiv.dataset.infection > thisCityDiv.dataset.infection) {
        } else if (cityDiv.dataset.name < thisCityDiv.dataset.name) {
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
            totalInfections = totalInfections + [...phase.childNodes].filter(node => node.dataset.infection == 'true').length - 1;
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

    var inoculatedCards = inoculatedDiv.childNodes.length - 1;
    getCountdown(inoculatedDiv).innerHTML = `Inoculated<br/>(${inoculatedCards})`

    var zeroCardsInDiscardPile = discardPileDiv.childNodes.length === 1;
    epidemicButton.disabled = zeroCardsInDiscardPile;
    newPhaseButton.disabled = zeroCardsInDiscardPile;
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
