var body;
var cardStacks;
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
var innoculatedDiv;
var newGameButton;
var newGameDiv;
var newPhaseButton;
var nextTurnButton;
var pawn;
var pawnColors;
var pawnColorsInput;
var resourceCardsInput;
var selectedDiv;

window.onload = () => {
    window.onload = undefined;

    body = document.getElementById('body');
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
    resourceCardsInput = document.getElementById('resourceCardsInput');

    cityCardsInput.onchange = e => refreshEpidemicsInput();
    epidemicButton.onclick = e => nextPhase(true, true);
    newGameButton.onclick = e => newGameButtonOnClick();
    newPhaseButton.onclick = e => nextPhase(false, false);
    nextTurnButton.onclick = e => nextTurn();

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

function newGameButtonOnClick() {
    if (epidemicDiv.classList.contains('hidden') || confirm('Start a new game?')) {
        startNewGame();
    }
}

function startNewGame() {
    newGameDiv.classList.add('hidden');
    getAllPhases().forEach(phase => body.removeChild(phase));

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

    innoculatedDiv = createPhase();
    discardPileDiv = createPhase();

    cities
        .filter(city => city.infection)
        .sort((a, b) => a.name < b.name ? -1 : 1)
        .forEach(createCity);

    nextPhase(true, true);

    cities
        .filter(city => !city.infection)
        .sort((a, b) => a.name < b.name ? -1 : 1)
        .forEach(createCity);

    epidemicDiv.classList.remove('hidden');
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

    discardPileDiv = createPhase();
    selectPhase(discardPileDiv);
    
    recalculateCountdowns();

    epidemicButton.disabled = true;
    newPhaseButton.disabled = true;
}

function createPhase() {
    var countdownSpan = document.createElement('span');
    countdownSpan.id = 'countdown';

    var countdownDiv = document.createElement('div');
    countdownDiv.classList.add('cell');
    countdownDiv.appendChild(countdownSpan);

    var phaseDiv = document.createElement('div');
    phaseDiv.classList.add('phase');
    phaseDiv.classList.add('section');
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

function createCity(city) {
    var citySpan = document.createElement('span');
    citySpan.innerHTML = city.name + (city.modifier ? `<br/><span class="effect">${city.modifier}</span>` : '');

    var cityDiv = document.createElement('div');
    if (city.infection) {
        cityDiv.dataset.infection = true;
    }
    cityDiv.classList.add(city.color);
    cityDiv.classList.add('button');
    cityDiv.classList.add('cell');
    cityDiv.appendChild(citySpan);

    discardPileDiv.appendChild(cityDiv);

    var onclick = e => infectCity(cityDiv);
    var oncontextmenu = e => { destroyInfection(cityDiv); return false; }
    citySpan.onclick = onclick;
    citySpan.oncontextmenu = oncontextmenu;
    cityDiv.onclick = onclick;
    cityDiv.oncontextmenu = oncontextmenu;
}

function infectCity(cityDiv) {
    var phaseDiv = cityDiv.parentNode

    if (phaseDiv === selectedDiv) {
        return;
    }

    phaseDiv.removeChild(cityDiv);
    if (phaseDiv !== discardPileDiv && phaseDiv !== innoculatedDiv && phaseDiv.childNodes.length === 1) {
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

    if (getAllPhases().length === 2) {
        nextPhase(false, true);
    }

    epidemicButton.disabled = false;
    newPhaseButton.disabled = false;

    recalculateCountdowns();
}

function destroyInfection(cityDiv) {
    var phaseDiv = cityDiv.parentNode

    phaseDiv.removeChild(cityDiv);
    if (phaseDiv !== discardPileDiv && phaseDiv.childNodes.length === 1) {
        phaseDiv.parentNode.removeChild(phaseDiv);
    }

    recalculateCountdowns();
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
    var allPhases = getAllPhases();
    allPhases.forEach((phase, index) => {
        if (phase === discardPileDiv) {
            getCountdown(phase).innerHTML = 'Dicard<br/>Pile';
        } else if (phase === innoculatedDiv) {
            getCountdown(phase).innerHTML = 'Innoculated';
        } else {
            totalInfections = totalInfections + 1;
            var firstTurn = Math.ceil(totalInfections / rate);
            totalInfections = totalInfections + [...phase.childNodes].filter(node => node.dataset.infection).length - 1;
            var lastTurn = Math.ceil(totalInfections / rate);

            getCountdown(phase).innerHTML =
                  lastTurn === 1 ? lastTurn + '<br/>turn'
                : firstTurn === lastTurn ? firstTurn + '<br/>turns'
                : firstTurn + ' - ' + lastTurn + '<br/>turns'
        }
    });
}

function getAllPhases() {
    return [...document.getElementsByClassName('phase')];
}

function getCountdown(phase) {
    return phase.firstChild.firstChild;
}