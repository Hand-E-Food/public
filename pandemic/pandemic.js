window.onload = () => {
    window.onload = undefined;
    body = document.getElementById('body');
    infectionRate = document.getElementById("infectionRate");
    epidemicButton = document.getElementById("epidemicButton");
    epidemicButton.onclick = ev => nextPhase(true);
    newPhaseButton = document.getElementById("newPhaseButton");
    newPhaseButton.onclick = ev => nextPhase(false);

    nextPhase(false);
    cities.sort((a, b) => a.name < b.name ? -1 : 1).forEach(createCity);
    nextPhase(true);
}

var body;
var epidemicButton;
var infectionRate;
var infectionRates = [2, 2, 2, 3, 3, 4, 4];
var latestPhaseDiv;
var newPhaseButton;

function nextPhase(epidemic) {
    if (latestPhaseDiv && latestPhaseDiv.childNodes.length === 0) {
        return;
    }

    if (epidemic) {
        infectionRate.innerHTML = infectionRates.shift();
    }

    var countdownSpan = document.createElement('span');
    countdownSpan.id = 'countdown';

    var countdownDiv = document.createElement('div');
    countdownDiv.appendChild(countdownSpan);

    latestPhaseDiv = document.createElement('div');
    latestPhaseDiv.classList.add('phase');
    latestPhaseDiv.appendChild(countdownDiv);
    
    body.insertBefore(latestPhaseDiv, body.childNodes[2]);

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

function recalculateCountdowns() {
    var cities = 0;
    var rate = parseInt(infectionRate.innerHTML);
    [...document.getElementsByClassName('phase')].forEach((phase, index) => {
        var countdown = phase.firstChild.firstChild;
        if (index === 0) {
            countdown.innerHTML = 'Dicard<br/>Pile';
        } else {
            cities = cities + 1;
            var firstTurn = Math.ceil(cities / rate);
            cities = cities + phase.childNodes.length - 2;
            var lastTurn = Math.ceil(cities / rate);

            countdown.innerHTML = firstTurn === lastTurn
                ? firstTurn + '<br/>turn'
                : firstTurn + ' - ' + lastTurn + '<br/>turns'
        }
    });
}
