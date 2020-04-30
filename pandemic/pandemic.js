var body;
var cardStacks;
var epidemicButton;
var epidemicCount;
var epidemicDiv;
var epidemicTurns;
var fundingCount;
var infectionRate;
var infectionRates;
var latestPhaseDiv;
var newGameButton;
var newPhaseButton;
var nextTurnButton;

window.onload = () => {
    window.onload = undefined;
    body = document.getElementById('body');
    epidemicButton = document.getElementById("epidemicButton");
    epidemicCount = document.getElementById("epidemicCount");
    epidemicDiv = document.getElementById("epidemicDiv");
    epidemicTurns = document.getElementById("epidemicTurns");
    fundingCount = document.getElementById("fundingCount");
    infectionRate = document.getElementById("infectionRate");
    newGameButton = document.getElementById("newGameButton");
    newPhaseButton = document.getElementById("newPhaseButton");
    nextTurnButton = document.getElementById("nextTurnButton");

    epidemicButton.onclick = e => nextPhase(true);
    newGameButton.onclick = e => newGameButtonOnClick();
    newPhaseButton.onclick = e => nextPhase(false);
    nextTurnButton.onclick = e => nextTurn();

    startNewGame();
}

function newGameButtonOnClick() {
    if (confirm("Start a new game?")) {
        startNewGame();
    }
}

function startNewGame() {
    getAllPhases().forEach(phase => body.removeChild(phase));

    var epidemicCards = parseInt(epidemicCount.value);
    var fundingCards = parseInt(fundingCount.value);
    cardStacks = new Array(epidemicCards);
    for(var i = 0; i < epidemicCards; i++) {
        cardStacks[i] = 0;
    }
    for (var playerCards = fundingCards + cities.length + epidemicCards - 1; playerCards >= 0; playerCards--) {
        cardStacks[playerCards % epidemicCards]++;
    }
    cardStacks = [0, ...cardStacks];
    
    infectionRates = [2, 2, 2, 3, 3, 4, 4].slice(0, epidemicCards + 1);
    nextPhase(false);
    cities.sort((a, b) => a.name < b.name ? -1 : 1).forEach(createCity);
    nextPhase(true);
}

function nextPhase(epidemic) {
    if (latestPhaseDiv && latestPhaseDiv.childNodes.length === 0) {
        return;
    }

    if (epidemic) {
        infectionRate.innerHTML = infectionRates.shift();
        if (cardStacks.length >= 2) {
            cardStacks[1] = cardStacks[0] + cardStacks[1];
        }
        cardStacks.shift();
        recalculateEpidemicTurns();
    }

    var countdownSpan = document.createElement('span');
    countdownSpan.id = 'countdown';

    var countdownDiv = document.createElement('div');
    countdownDiv.appendChild(countdownSpan);

    latestPhaseDiv = document.createElement('div');
    latestPhaseDiv.classList.add('phase');
    latestPhaseDiv.appendChild(countdownDiv);
    
    body.insertBefore(latestPhaseDiv, epidemicDiv.nextSibling);

    recalculateCountdowns();

    epidemicButton.disabled = true;
    newPhaseButton.disabled = true;
}

function createCity(city) {
    var onclick = e => cityOnLeftClick(cityDiv);
    var oncontextmenu = e => { cityOnRightClick(cityDiv); return false; }

    var citySpan = document.createElement('span');
    citySpan.innerHTML = city.name;
    citySpan.onclick = onclick;
    citySpan.oncontextmenu = oncontextmenu;

    var cityDiv = document.createElement('div');
    cityDiv.classList.add('city');
    cityDiv.classList.add(city.color);
    cityDiv.onclick = onclick;
    cityDiv.oncontextmenu = oncontextmenu;
    cityDiv.appendChild(citySpan);

    latestPhaseDiv.appendChild(cityDiv);
}

function cityOnLeftClick(cityDiv) {
    var phaseDiv = cityDiv.parentNode

    if (phaseDiv === body.childNodes[2]) {
        return;
    }

    phaseDiv.removeChild(cityDiv);
    if (phaseDiv.childNodes.length === 1) {
        phaseDiv.parentNode.removeChild(phaseDiv);
    }
    var existingCityDiv;
    for(var i = 1; i < latestPhaseDiv.childNodes.length; i++) {
        if (latestPhaseDiv.childNodes[i].firstChild.innerHTML > cityDiv.firstChild.innerHTML) {
            existingCityDiv = latestPhaseDiv.childNodes[i];
            break;
        }
    }
    latestPhaseDiv.insertBefore(cityDiv, existingCityDiv);

    epidemicButton.disabled = false;
    newPhaseButton.disabled = false;

    recalculateCountdowns();
}

function cityOnRightClick(cityDiv) {
    var phaseDiv = cityDiv.parentNode

    phaseDiv.removeChild(cityDiv);
    if (phaseDiv.childNodes.length === 1) {
        phaseDiv.parentNode.removeChild(phaseDiv);
    }

    recalculateCountdowns();
}

function nextTurn() {
    for (var i = 2; i > 0; i--) {
        if (cardStacks.length > 0) {
            cardStacks[0]--;
            if (cardStacks[0] === 0) {
                cardStacks.shift();
            }
        }
    }
    recalculateEpidemicTurns();
}

function recalculateEpidemicTurns() {
    if (cardStacks.length > 0) {
        var turns = Math.ceil(cardStacks[0] / 2);
        epidemicTurns.innerHTML = turns === 1 ? '1 turn' : '1 - ' + turns + ' turns';
    } else {
        epidemicTurns.innerHTML = 'never';
    }
}

function recalculateCountdowns() {
    var cities = 0;
    var rate = parseInt(infectionRate.innerHTML);
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
