window.onload = () => {
    window.onload = undefined;
    body = document.getElementById('body');
    epidemicButton = document.getElementById("epidemicButton");
    epidemicButton.onclick = ev => nextEpidemic();

    nextEpidemic();
    cities.sort().forEach(createCity);
    nextEpidemic();
}

var body;
var epidemicButton;
var infectionRates = [0, 2, 2, 2, 3, 3, 4, 4];
var latestPhaseDiv;

function nextEpidemic() {
    if (latestPhaseDiv && latestPhaseDiv.childNodes.length === 0) {
        return;
    }

    document.getElementById("infectionRate").innerHTML = infectionRates.shift();

    latestPhaseDiv = document.createElement('div');
    latestPhaseDiv.classList.add('phase');
    
    if (body.childNodes.length > 1) {
        body.insertBefore(latestPhaseDiv, body.childNodes[2]);
    } else {
        body.appendChild(latestPhaseDiv);
    }

    epidemicButton.disabled = true;
}

function createCity(name) {
    var cityDiv = document.createElement('div');
    cityDiv.classList.add('city');
    cityDiv.innerHTML = name;
    cityDiv.onclick = ev => cityOnClick(cityDiv);

    latestPhaseDiv.appendChild(cityDiv);
}

function cityOnClick(cityDiv) {
    var phaseDiv = cityDiv.parentNode
    if (phaseDiv !== body.childNodes[3]) {
        return;
    }

    phaseDiv.removeChild(cityDiv);
    if (phaseDiv.childNodes.length === 0) {
        phaseDiv.parentNode.removeChild(phaseDiv);
    }
    latestPhaseDiv.appendChild(cityDiv);
    epidemicButton.disabled = false;
}
