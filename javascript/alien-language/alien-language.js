const fontSize = 60;
const svgNS = "http://www.w3.org/2000/svg";
const xmlnsNS = "http://www.w3.org/2000/xmlns/";
const xlinkNS = "http://www.w3.org/1999/xlink";

var seeds = [];

var AudioContextConstructor = AudioContext || webkitAudioContext;

function initialise() {
    document.getElementById("speakButton").disabled = !AudioContextConstructor;
    selectAll(document.getElementById("input"));
}

function selectAll(element) {
    if (element.select) {
        element.select();
    } else if (element.setSelectionRange) {
        element.setSelectionRange(0, element.value.length)
    }
}

function generateOutput() {
    var inputElement = document.getElementById("input");
    var input = inputElement.value;
    seeds = input.split(" ")
        .map(word => createSeed(word))
        .filter(seed => seed > 0)
        .map(seed => germinateSeed(seed));

    var outputElement = resetOutputElement();

    seeds.forEach(seed => {
        var svgElement = createSvgElement(1, 1);
        createGlyph(seed, svgElement, 0, 0);
        outputElement.appendChild(svgElement);
    }, this);
}

function createSeed(word) {
    word = word.toLowerCase();
    var seed = 0;
    for (var i = 0; i < word.length; i++) {
        var char = word.charCodeAt(i);
        if (char >= 97 && char <= 122)
            seed = seed * 26 - seed + char - 96;
    }
    return seed;
}

function germinateSeed(seed) {
    seed = [ seed ];
    var results = [];
    results.push(popNumber(seed, 5));
    for(var i = 1; i <= 4; i++) {
        if (i === results[0]) {
            results.push(0);
            results.push(0);
        } else {
            var long = popNumber(seed, 5);
            var short = long
                ? popNumber(seed, 7) + 1
                : popNumber(seed, 9);
            results.push(long);
            results.push(short);
        }
    }
    return results;
}

function popNumber(seed, max) {
    var result = seed[0] % max;
    seed[0] = (seed[0] - result) / max;
    return result;
}

function resetOutputElement() {
    var outputDivElement = document.getElementById("output");
    var node;
    while(node = outputDivElement.lastChild) {
        outputDivElement.removeChild(node);
    }
    var outputSpanElement = document.createElement("span");
    outputDivElement.appendChild(outputSpanElement);
    return outputSpanElement;
}

function createSvgElement(width, height) {
    var svgElement = document.createElementNS(svgNS, "svg");
    svgElement.setAttributeNS(xmlnsNS, "xmlns:xlink", xlinkNS);
    svgElement.setAttribute("width", width * fontSize);
    svgElement.setAttribute("height", height * fontSize);
    svgElement.setAttribute("viewBox", "0 0 " + (width * 100) + " " + (height * 100));
    return svgElement;
}

function createGlyph(seed, gElement, originX, originY) {

    const rule = {
        out1: 10,
        mid1: 30,
        cir1: 44.14,
        midX: 50,
        cir2: 55.86,
        mid2: 70,
        out2: 90,
        rad:  20,
    };

    /* 
     * : = circle element of block
     * block 1 = top-left
     * block 2 = top-right
     * block 3 = bottom-left
     * block 4 = bottom-right
     * first letter after circle points to the centre
     * subsequent letters are clockwise
     */
    const renders = [
        // Circles
        { segments: "+........:........:........:........", createElement: (x, y) => createCircle(x+rule.mid1, y+rule.mid1, rule.rad) },
        { segments: ":........+........:........:........", createElement: (x, y) => createCircle(x+rule.mid2, y+rule.mid1, rule.rad) },
        { segments: ":........:........+........:........", createElement: (x, y) => createCircle(x+rule.mid1, y+rule.mid2, rule.rad) },
        { segments: ":........:........:........+........", createElement: (x, y) => createCircle(x+rule.mid2, y+rule.mid2, rule.rad) },
        // Diagonal lines touching circles
        { segments: "+........:........:........:+...+...", createElement: (x, y) => createLine(x+rule.out2, y+rule.out2, x+rule.cir1, y+rule.cir1) },
        { segments: "+........:........:........:+...-...", createElement: (x, y) => createLine(x+rule.mid2, y+rule.mid2, x+rule.cir1, y+rule.cir1) },
        { segments: ":........+........:+...+...:........", createElement: (x, y) => createLine(x+rule.out1, y+rule.out2, x+rule.cir2, y+rule.cir1) },
        { segments: ":........+........:+...-...:........", createElement: (x, y) => createLine(x+rule.mid1, y+rule.mid2, x+rule.cir2, y+rule.cir1) },
        { segments: ":........:+...+...+........:........", createElement: (x, y) => createLine(x+rule.out2, y+rule.out1, x+rule.cir1, y+rule.cir2) },
        { segments: ":........:+...-...+........:........", createElement: (x, y) => createLine(x+rule.mid2, y+rule.mid1, x+rule.cir1, y+rule.cir2) },
        { segments: ":+...+...:........:........+........", createElement: (x, y) => createLine(x+rule.out1, y+rule.out1, x+rule.cir2, y+rule.cir2) },
        { segments: ":+...-...:........:........+........", createElement: (x, y) => createLine(x+rule.mid1, y+rule.mid1, x+rule.cir2, y+rule.cir2) },
        // Long diagonal top-left to bottom-right
        { segments: ":+...+...:........:........:+...+...", createElement: (x, y) => createLine(x+rule.out1, y+rule.out1, x+rule.out2, y+rule.out2) },
        { segments: ":+...+...:........:........:+...-...", createElement: (x, y) => createLine(x+rule.mid2, y+rule.mid2, x+rule.out1, y+rule.out1) },
        { segments: ":-...+...:........:........:+...+...", createElement: (x, y) => createLine(x+rule.mid1, y+rule.mid1, x+rule.out2, y+rule.out2) },
        { segments: ":+...+...:........:........--.......", createElement: (x, y) => createLine(x+rule.out1, y+rule.out1, x+rule.midX, y+rule.midX) },
        { segments: ":+...-...:........:........:+...-...", createElement: (x, y) => createLine(x+rule.mid1, y+rule.mid1, x+rule.mid2, y+rule.mid2) },
        { segments: "--.......:........:........:+...+...", createElement: (x, y) => createLine(x+rule.out2, y+rule.out2, x+rule.midX, y+rule.midX) },
        { segments: ":-...+...:........:........:........", createElement: (x, y) => createLine(x+rule.out1, y+rule.out1, x+rule.mid1, y+rule.mid1) },
        { segments: ":+...-...:........:........--.......", createElement: (x, y) => createLine(x+rule.mid1, y+rule.mid1, x+rule.midX, y+rule.midX) },
        { segments: "--.......:........:........:+...-...", createElement: (x, y) => createLine(x+rule.mid2, y+rule.mid2, x+rule.midX, y+rule.midX) },
        { segments: ":........:........:........:-...+...", createElement: (x, y) => createLine(x+rule.out2, y+rule.out2, x+rule.mid2, y+rule.mid2) },
        // Long diagonal top-right to bottom-left
        { segments: ":........:+...+...:+...+...:........", createElement: (x, y) => createLine(x+rule.out2, y+rule.out1, x+rule.out1, y+rule.out2) },
        { segments: ":........:+...+...:+...-...:........", createElement: (x, y) => createLine(x+rule.mid1, y+rule.mid2, x+rule.out2, y+rule.out1) },
        { segments: ":........:-...+...:+...+...:........", createElement: (x, y) => createLine(x+rule.mid2, y+rule.mid1, x+rule.out1, y+rule.out2) },
        { segments: ":........:+...+...--.......:........", createElement: (x, y) => createLine(x+rule.out2, y+rule.out1, x+rule.midX, y+rule.midX) },
        { segments: ":........:+...-...:+...-...:........", createElement: (x, y) => createLine(x+rule.mid2, y+rule.mid1, x+rule.mid1, y+rule.mid2) },
        { segments: ":........--.......:+...+...:........", createElement: (x, y) => createLine(x+rule.out1, y+rule.out2, x+rule.midX, y+rule.midX) },
        { segments: ":........:-...+...:........:........", createElement: (x, y) => createLine(x+rule.out2, y+rule.out1, x+rule.mid2, y+rule.mid1) },
        { segments: ":........:+...-...--.......:........", createElement: (x, y) => createLine(x+rule.mid2, y+rule.mid1, x+rule.midX, y+rule.midX) },
        { segments: ":........--.......:+...-...:........", createElement: (x, y) => createLine(x+rule.mid1, y+rule.mid2, x+rule.midX, y+rule.midX) },
        { segments: ":........:........:-...+...:........", createElement: (x, y) => createLine(x+rule.out1, y+rule.out2, x+rule.mid1, y+rule.mid2) },
        // Long line on bottom
        { segments: ":........:........:.+...+..:...+...+", createElement: (x, y) => createLine(x+rule.out1, y+rule.mid2, x+rule.out2, y+rule.mid2) },
        { segments: ":........:........:.+...+..:...-...+", createElement: (x, y) => createLine(x+rule.mid2, y+rule.mid2, x+rule.out1, y+rule.mid2) },
        { segments: ":........:........:.-...+..:...+...+", createElement: (x, y) => createLine(x+rule.mid1, y+rule.mid2, x+rule.out2, y+rule.mid2) },
        { segments: ":........:........:.+...+..:.......-", createElement: (x, y) => createLine(x+rule.out1, y+rule.mid2, x+rule.midX, y+rule.mid2) },
        { segments: ":........:........:.+...-..:...-...+", createElement: (x, y) => createLine(x+rule.mid1, y+rule.mid2, x+rule.mid2, y+rule.mid2) },
        { segments: ":........:........:.-......:...+...+", createElement: (x, y) => createLine(x+rule.out2, y+rule.mid2, x+rule.midX, y+rule.mid2) },
        { segments: ":........:........:.-...+..:........", createElement: (x, y) => createLine(x+rule.out1, y+rule.mid2, x+rule.mid1, y+rule.mid2) },
        { segments: ":........:........:.+...-..:.......-", createElement: (x, y) => createLine(x+rule.mid1, y+rule.mid2, x+rule.midX, y+rule.mid2) },
        { segments: ":........:........:.-......:...-...+", createElement: (x, y) => createLine(x+rule.mid2, y+rule.mid2, x+rule.midX, y+rule.mid2) },
        { segments: ":........:........:........:...+...-", createElement: (x, y) => createLine(x+rule.out2, y+rule.mid2, x+rule.mid2, y+rule.mid2) },
        // Long line on left
        { segments: ":.+...+..:........:...+...+:........", createElement: (x, y) => createLine(x+rule.mid1, y+rule.out1, x+rule.mid1, y+rule.out2) },
        { segments: ":.+...+..:........:...-...+:........", createElement: (x, y) => createLine(x+rule.mid1, y+rule.mid2, x+rule.mid1, y+rule.out1) },
        { segments: ":.-...+..:........:...+...+:........", createElement: (x, y) => createLine(x+rule.mid1, y+rule.mid1, x+rule.mid1, y+rule.out2) },
        { segments: ":.+...+..:........:.......-:........", createElement: (x, y) => createLine(x+rule.mid1, y+rule.out1, x+rule.mid1, y+rule.midX) },
        { segments: ":.+...-..:........:...-...+:........", createElement: (x, y) => createLine(x+rule.mid1, y+rule.mid1, x+rule.mid1, y+rule.mid2) },
        { segments: ":.-......:........:...+...+:........", createElement: (x, y) => createLine(x+rule.mid1, y+rule.out2, x+rule.mid1, y+rule.midX) },
        { segments: ":.-...+..:........:........:........", createElement: (x, y) => createLine(x+rule.mid1, y+rule.out1, x+rule.mid1, y+rule.mid1) },
        { segments: ":.+...-..:........:.......-:........", createElement: (x, y) => createLine(x+rule.mid1, y+rule.mid1, x+rule.mid1, y+rule.midX) },
        { segments: ":.-......:........:...-...+:........", createElement: (x, y) => createLine(x+rule.mid1, y+rule.mid2, x+rule.mid1, y+rule.midX) },
        { segments: ":........:........:...+...-:........", createElement: (x, y) => createLine(x+rule.mid1, y+rule.out2, x+rule.mid1, y+rule.mid2) },
        // Long line on right
        { segments: ":........:...+...+:........:.+...+..", createElement: (x, y) => createLine(x+rule.mid2, y+rule.out1, x+rule.mid2, y+rule.out2) },
        { segments: ":........:...+...+:........:.+...-..", createElement: (x, y) => createLine(x+rule.mid2, y+rule.mid2, x+rule.mid2, y+rule.out1) },
        { segments: ":........:...+...-:........:.+...+..", createElement: (x, y) => createLine(x+rule.mid2, y+rule.mid1, x+rule.mid2, y+rule.out2) },
        { segments: ":........:...+...+:........:.-......", createElement: (x, y) => createLine(x+rule.mid2, y+rule.out1, x+rule.mid2, y+rule.midX) },
        { segments: ":........:...-...+:........:.+...-..", createElement: (x, y) => createLine(x+rule.mid2, y+rule.mid1, x+rule.mid2, y+rule.mid2) },
        { segments: ":........:.......-:........:.+...+..", createElement: (x, y) => createLine(x+rule.mid2, y+rule.out2, x+rule.mid2, y+rule.midX) },
        { segments: ":........:...+...-:........:........", createElement: (x, y) => createLine(x+rule.mid2, y+rule.out1, x+rule.mid2, y+rule.mid1) },
        { segments: ":........:...-...+:........:.-......", createElement: (x, y) => createLine(x+rule.mid2, y+rule.mid1, x+rule.mid2, y+rule.midX) },
        { segments: ":........:.......-:........:.+...-..", createElement: (x, y) => createLine(x+rule.mid2, y+rule.mid2, x+rule.mid2, y+rule.midX) },
        { segments: ":........:........:........:.-...+..", createElement: (x, y) => createLine(x+rule.mid2, y+rule.out2, x+rule.mid2, y+rule.mid2) },
        // Short line on top-left
        { segments: ":..+...+.:........:........:........", createElement: (x, y) => createLine(x+rule.out1, y+rule.midX, x+rule.midX, y+rule.out1) },
        { segments: ":..+...-.:........:........:........", createElement: (x, y) => createLine(x+rule.out1, y+rule.midX, x+rule.mid1, y+rule.mid1) },
        { segments: ":..-...+.:........:........:........", createElement: (x, y) => createLine(x+rule.midX, y+rule.out1, x+rule.mid1, y+rule.mid1) },
        // Short line on top-right
        { segments: ":........:..+...+.:........:........", createElement: (x, y) => createLine(x+rule.out2, y+rule.midX, x+rule.midX, y+rule.out1) },
        { segments: ":........:..-...+.:........:........", createElement: (x, y) => createLine(x+rule.out2, y+rule.midX, x+rule.mid2, y+rule.mid1) },
        { segments: ":........:..+...-.:........:........", createElement: (x, y) => createLine(x+rule.midX, y+rule.out1, x+rule.mid2, y+rule.mid1) },
        // Short line on bottom-left
        { segments: ":........:........:..+...+.:........", createElement: (x, y) => createLine(x+rule.out1, y+rule.midX, x+rule.midX, y+rule.out2) },
        { segments: ":........:........:..-...+.:........", createElement: (x, y) => createLine(x+rule.out1, y+rule.midX, x+rule.mid1, y+rule.mid2) },
        { segments: ":........:........:..+...-.:........", createElement: (x, y) => createLine(x+rule.midX, y+rule.out2, x+rule.mid1, y+rule.mid2) },
        // Short line on bottom-right
        { segments: ":........:........:........:..+...+.", createElement: (x, y) => createLine(x+rule.out2, y+rule.midX, x+rule.midX, y+rule.out2) },
        { segments: ":........:........:........:..+...-.", createElement: (x, y) => createLine(x+rule.out2, y+rule.midX, x+rule.mid2, y+rule.mid2) },
        { segments: ":........:........:........:..-...+.", createElement: (x, y) => createLine(x+rule.midX, y+rule.out2, x+rule.mid2, y+rule.mid2) },
        
    ];

    var segments = getSegments(seed);

    renders
        .filter(render => segmentsMatch(render.segments, segments))
        .forEach(render => gElement.appendChild(render.createElement(originX, originY)));
}

function getSegments(seed) {
    var result = "";
    for(var i = 1; i <= 4; i++)
        result += getQuadrantSegments(seed, i);
    return result;    
}

function getQuadrantSegments(seed, quadrant) {
    var i = quadrant * 2 - 1;
    var j = quadrant * 2 - 0;
    var result = "         ".split("");

    if (seed[0] === quadrant)
        result[0] = "O";
    if (seed[i] > 0) {
        result[seed[i] + 0] = "L";
        result[seed[i] + 4] = "L";
        result[(seed[i] + seed[j]) % 8 + 1] = "S";
    } else if (seed[j] > 0) {
        result[seed[j]] = "S";
    }
    return result.join("");
}

function segmentsMatch(pattern, segments) {
    for(var i = 0; i < pattern.length; i++) {
        if (pattern[i] === "-" && segments[i] !== " ")
            return false;
        if (pattern[i] === "+" && segments[i] === " ")
            return false;
    }
    return true;
}

function createCircle(cx, cy, r) {
    var element = document.createElementNS(svgNS, "circle");
    element.setAttribute("cx", cx);
    element.setAttribute("cy", cy);
    element.setAttribute("r" , r);
    return element;
}

function createLine(x1, y1, x2, y2) {
    var element = document.createElementNS(svgNS, "line");
    element.setAttribute("x1", x1);
    element.setAttribute("y1", y1);
    element.setAttribute("x2", x2);
    element.setAttribute("y2", y2);
    return element;
}

function speakOutput() {
    if (!AudioContextConstructor)
        return;

    var audioContext = new AudioContextConstructor();
    audioContext.suspend();
    var startTime = [ 0 ];
    sounds = seeds.map((seed, index) => createSound(audioContext, seed, index, startTime));
    sounds[sounds.length - 1].onended = event => event.target.context.close();
    audioContext.resume();
}

function createSound(audioContext, seed, index, startTimeRef) {
    var frequencyCurve = [];
    [2,4,8,6].forEach(n => {
        if (seed[n] > 0) {
            frequencyCurve.push(seed[n] * 20 + seed[n - 1] * 160 + 200);
        }
    });

    var soundLength = (frequencyCurve.length + 1) * 0.12;
    var startTime   = startTimeRef[0];
    var stopTime    = startTime + soundLength;

    var gainCurve = getGainCurve(frequencyCurve.length - 1);

    var gainNode = audioContext.createGain();
    gainNode.connect(audioContext.destination);
    gainNode.gain.value = gainNode.gain.minValue;
    gainNode.gain.setValueCurveAtTime(
        new Float32Array(gainCurve),
        startTime,
        soundLength
    );

    var periodicWave = createPeriodicWave(audioContext, seed);

    var oscillatorNode = audioContext.createOscillator();
    oscillatorNode.connect(gainNode);
    oscillatorNode.setPeriodicWave(periodicWave);
    if (frequencyCurve.length === 0) {
        oscillatorNode.frequency.value = oscillatorNode.frequency.minValue;
    } else if (frequencyCurve.length === 1) {
        oscillatorNode.frequency.value = frequencyCurve[0];
    } else {
        oscillatorNode.frequency.setValueCurveAtTime(
            new Float32Array(frequencyCurve),
            startTime,
            soundLength
        );
    }
    oscillatorNode.start(startTime);
    oscillatorNode.stop(stopTime);

    startTimeRef[0] = stopTime;

    return oscillatorNode;
}

function getGainCurve(count) {
    var rise = [0.00, 0.05, 0.15, 0.30, 0.50, 0.70, 0.85, 0.95, 1.00];
    var wave = [0.97, 0.91, 0.82, 0.70, 0.58, 0.49, 0.43, 0.40, 0.43, 0.49, 0.58, 0.70, 0.82, 0.91, 0.97, 1.00];
    var fall = [0.95, 0.85, 0.70, 0.50, 0.30, 0.15, 0.05, 0.00];
    var result = rise;
    while (count > 0) {
        count--;
        result = result.concat(wave);
    }
    result = result.concat(fall);
    return result;
}

function createPeriodicWave(audioContext, seed) {
    var complex = [
        getComplex(     0 ,      0 ),
        getComplex(seed[1], seed[2]),
        getComplex(seed[3], seed[4]),
        getComplex(seed[7], seed[8]),
        getComplex(seed[5], seed[6]),
    ];

    return audioContext.createPeriodicWave(
        new Float32Array(complex.map(x => x.real)),
        new Float32Array(complex.map(x => x.imag)),
        { disableNormalization: true }
    );
}

function getComplex(amplitude, phase) {
    amplitude = (amplitude + 1) / 5;
    phase = Math.PI * (phase - 4) / 4;

    return {
        real: amplitude * Math.cos(phase),
        imag: amplitude * Math.sin(phase),
    };
}
