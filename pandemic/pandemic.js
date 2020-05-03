var body;
var cardStacks;
var currentPawn;
var discardPileDiv;
var epidemicButton;
var epidemicDiv;
var epidemicTurns;
var epidemicsInput;
var fundingInput;
var infectionRateOutput;
var infectionRates;
var newGameButton;
var newGameDiv;
var newPhaseButton;
var nextTurnButton;
var pawn;
var pawnColors;
var pawnColorsInput;

window.onload = () => {
    window.onload = undefined;
    body = document.getElementById('body');
    epidemicButton = document.getElementById('epidemicButton');
    epidemicsInput = document.getElementById('epidemicsInput');
    epidemicDiv = document.getElementById('epidemicDiv');
    epidemicTurns = document.getElementById('epidemicTurns');
    fundingInput = document.getElementById('fundingInput');
    infectionRateOutput = document.getElementById('infectionRateOutput');
    newGameButton = document.getElementById('newGameButton');
    newGameDiv = document.getElementById('newGameDiv');
    newPhaseButton = document.getElementById('newPhaseButton');
    nextTurnButton = document.getElementById('nextTurnButton');
    pawn = document.getElementById('pawn');
    pawnColorsInput = document.getElementById('pawnColorsInput');

    epidemicButton.onclick = e => nextPhase(true);
    newGameButton.onclick = e => newGameButtonOnClick();
    newPhaseButton.onclick = e => nextPhase(false);
    nextTurnButton.onclick = e => nextTurn();
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

    var epidemicCards = parseInt(epidemicsInput.value);
    var fundingCards = parseInt(fundingInput.value);
    cardStacks = new Array(epidemicCards);
    for(var i = 0; i < epidemicCards; i++) {
        cardStacks[i] = 0;
    }
    for (var playerCards = fundingCards + cities.length + epidemicCards - 1; playerCards >= 0; playerCards--) {
        cardStacks[playerCards % epidemicCards]++;
    }
    cardStacks = [0, ...cardStacks].map(cards => ({ cards: cards, epidemic: true }));
    
    infectionRates = [2, 2, 2, 3, 3, 4, 4].slice(0, epidemicCards + 1);
    nextPhase(false);
    cities.sort((a, b) => a.name < b.name ? -1 : 1).forEach(createCity);
    nextPhase(true);

    epidemicDiv.classList.remove('hidden');
}

function nextPhase(epidemic) {
    if (discardPileDiv && discardPileDiv.querySelector('.button').length === 0) {
        return;
    }

    if (epidemic) {
        var cardStack = cardStacks.find(stack => stack.epidemic);
        if (!cardStack) {
            return;
        }
        cardStack.epidemic = false;
        infectionRateOutput.innerHTML = infectionRates.shift();
        recalculateEpidemicTurns();
    }

    var countdownSpan = document.createElement('span');
    countdownSpan.id = 'countdown';

    var countdownDiv = document.createElement('div');
    countdownDiv.appendChild(countdownSpan);

    discardPileDiv = document.createElement('div');
    discardPileDiv.classList.add('phase');
    discardPileDiv.appendChild(countdownDiv);
    
    body.insertBefore(discardPileDiv, epidemicDiv.nextSibling);

    recalculateCountdowns();

    epidemicButton.disabled = true;
    newPhaseButton.disabled = true;
}

function createCity(city) {
    var citySpan = document.createElement('span');
    citySpan.innerHTML = city.name;

    var cityDiv = document.createElement('div');
    cityDiv.classList.add(city.color);
    cityDiv.classList.add('button');
    cityDiv.appendChild(citySpan);

    discardPileDiv.appendChild(cityDiv);

    var onclick = e => infectCity(cityDiv);
    var oncontextmenu = e => { immunizeCity(cityDiv); return false; }
    citySpan.onclick = onclick;
    citySpan.oncontextmenu = oncontextmenu;
    cityDiv.onclick = onclick;
    cityDiv.oncontextmenu = oncontextmenu;
}

function infectCity(cityDiv) {
    var phaseDiv = cityDiv.parentNode

    if (phaseDiv === discardPileDiv) {
        return;
    }

    phaseDiv.removeChild(cityDiv);
    if (phaseDiv.childNodes.length === 1) {
        phaseDiv.parentNode.removeChild(phaseDiv);
    }
    var existingCityDiv;
    for(var i = 1; i < discardPileDiv.childNodes.length; i++) {
        if (discardPileDiv.childNodes[i].firstChild.innerHTML > cityDiv.firstChild.innerHTML) {
            existingCityDiv = discardPileDiv.childNodes[i];
            break;
        }
    }
    discardPileDiv.insertBefore(cityDiv, existingCityDiv);

    epidemicButton.disabled = false;
    newPhaseButton.disabled = false;

    recalculateCountdowns();
}

function immunizeCity(cityDiv) {
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
    if (epidemicStack) {
        var firstTurn = Math.ceil((safeCards + 1) / 2);
        var lastTurn = Math.ceil((safeCards + epidemicStack.cards) / 2);
        epidemicTurns.innerHTML
            = lastTurn < 1    ? 'is late'
            : lastTurn === 1  ? 'on this turn'
            : firstTurn === 1 ? 'within ' + lastTurn + ' turns'
                              : 'in ' + firstTurn + ' to ' + lastTurn + ' turns';
    } else {
        epidemicTurns.innerHTML = 'never again!';
    }
}

function recalculateCountdowns() {
    var cities = 0;
    var rate = parseInt(infectionRateOutput.innerHTML);
    getAllPhases().forEach((phase, index) => {
        var countdown = phase.firstChild.firstChild;
        if (index === 0) {
            countdown.innerHTML = 'Dicard<br/>Pile';
        } else {
            cities = cities + 1;
            var firstTurn = Math.ceil(cities / rate);
            cities = cities + phase.childNodes.length - 2;
            var lastTurn = Math.ceil(cities / rate);

            countdown.innerHTML =
                  lastTurn === 1 ? lastTurn + '<br/>turn'
                : firstTurn === lastTurn ? firstTurn + '<br/>turns'
                : firstTurn + ' - ' + lastTurn + '<br/>turns'
        }
    });
}

function getAllPhases() {
    return [...document.getElementsByClassName('phase')];
}
