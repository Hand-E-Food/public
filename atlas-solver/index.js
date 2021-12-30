const COLOR = {
    BUILT: 'purple',
    DARK_GRAY: '#666',
    STATION: 'plum',
    TRANSPARENT: 'transparent',
    WHITE: 'white',
};

const NS = {
    SVG: 'http://www.w3.org/2000/svg',
};

const TRACK_POINTS = {
    1: 1,
    2: 2,
    3: 4,
    4: 7,
    6: 15,
    8: 21,
};

const TRACK_SELECTION_MODE = {
    NONE: 0,
    BUILT: 1,
    STATION: 2,
}

class Summary {
    _data;
    _divNode;
    _networks;

    get node() { return this._divNode; }

    get remainingStations() {
        return 3 - this._data.tracks
            .filter(track => track.selectionMode === TRACK_SELECTION_MODE.STATION)
            .length;
    }

    get remainingTracks() {
        return this._data.tracks
            .filter(track => track.selectionMode === TRACK_SELECTION_MODE.BUILT)
            .map(track => track.distance)
            .reduce((a, b) => a - b, 45);
    }

    constructor(data) {
        this._data = data;

        const divNode = document.createElement('div');
        divNode.classList.add('summary');
        this._divNode = divNode;

        this.refresh();
    }

    refresh() {
        const divNode = this._divNode;
        while (divNode.firstChild) {
            divNode.lastChild.remove();
        }

        try {
            this._validate();
        } catch (ex) {
            const spanNode = document.createElement('span');
            spanNode.innerText = ex;
            spanNode.classList.add('error');
            divNode.appendChild(spanNode);
            return;
        }

        this._networks = this._getTrackNetworks();

        const score = this._getScore();
        const totalPoints = Object.values(score).reduce((a, b) => isFinite(b) ? Number(a) + Number(b) : Number(a), 0);

        const tableNode = document.createElement('table');
        divNode.appendChild(tableNode);

        const thNode = document.createElement('tr');
        thNode.classList.add('total');
        tableNode.appendChild(thNode);

        const totalNameTdNode = document.createElement('td');
        totalNameTdNode.innerText = 'Total';
        thNode.appendChild(totalNameTdNode);

        const totalPointsTdNode = document.createElement('td');
        totalPointsTdNode.innerText = totalPoints;
        totalPointsTdNode.style.textAlign = 'right';
        thNode.appendChild(totalPointsTdNode);

        for(const [text, points] of Object.entries(score)) {
            const trNode = document.createElement('tr');
            tableNode.appendChild(trNode);

            const nameTdNode = document.createElement('td');
            nameTdNode.innerText = text;
            trNode.appendChild(nameTdNode);

            const pointsTdNode = document.createElement('td');
            let pointsText;
            if (!points) {
                pointsText = '';
            } else if (isFinite(points)) {
                pointsText = points;
                pointsTdNode.classList.add('include');
            } else {
                pointsText = points.substring(1);
                pointsTdNode.classList.add('exclude');
            }
            pointsTdNode.innerText = pointsText;
            pointsTdNode.style.textAlign = 'right';
            trNode.appendChild(pointsTdNode);
        }
    }

    _validate() {
        const remainingStations = this.remainingStations;
        if (remainingStations < 0) {
            throw new Error(`Built ${-remainingStations} too many stations.`);
        }

        const remainingTracks = this.remainingTracks;
        if (remainingTracks < 0) {
            throw new Error(`Built ${-remainingTracks} too many tracks.`);
        }
    }

    _getScore() {
        let longRouteKey = null;
        
        const score = {};

        score['Tracks'] = this._getScoreFromBuiltTracks();
        score['Longest Track'] = this._hasLongestTrack() ? 10 : '~10';
        score['Remaining Stations'] = this.remainingStations * 4;

        this._data.routes.forEach(route => {
            const key = route.cities.map(city => city.name).join(' - ');
            const isCompleted = this._routeIsCompleted(route);
            let points;
            if (isCompleted) {
                if (route.long) {
                    if (longRouteKey) {
                        if (score[longRouteKey] < route.points) {
                            score[longRouteKey] = `~${score[longRouteKey]}`;
                            longRouteKey = key;
                            points = route.points;
                        } else {
                            points = `~${route.points}`;
                        }
                    } else {
                        longRouteKey = key;
                        points = route.points;
                    }
                } else {
                    points = route.points;
                }
            } else {
                points = '';
            }
            score[key] = points;
        });

        return score;
    }

    _routeIsCompleted(route) {
        return this._networks.some(network =>
            route.cities.every(city =>
                network.has(city)
            )
        );
    }

    _getTrackNetworks() {
        let networks = this._data.tracks
            .filter(track => track.selectionMode !== TRACK_SELECTION_MODE.NONE)
            .map(track => {
                const network = new Set();
                track.cities.forEach(city => network.add(city));
                return network;
            });
        
        for (let a = networks.length - 2; a >= 0; a--) {
            const networkA = networks[a];
            for (let b = networks.length - 1; b > a; b--) {
                const networkB = [...networks[b].values()];
                if (networkB.some(city => networkA.has(city))) {
                    networkB.forEach(city => networkA.add(city));
                    networks.splice(b, 1);
                }
            }
        }

        return networks;
    }

    _hasLongestTrack() {
        return this._data.tracks.some(track => track.selectionMode === TRACK_SELECTION_MODE.BUILT);
    }

    _getScoreFromBuiltTracks() {
        return this._data.tracks
            .filter(track => track.selectionMode === TRACK_SELECTION_MODE.BUILT)
            .map(track => TRACK_POINTS[track.distance])
            .reduce((a, b) => a + b, 0);
    }
}

class MapCity {
    _gNode;
    
    get node() { return this._gNode; }

    constructor(data, scale) {
        const fontSize = scale * 1.5;
        const textHeight = fontSize; 
        const textWidth = fontSize * data.name.length * 0.6;

        const gNode = document.createElementNS(NS.SVG, 'g');
        this._gNode = gNode;

        const circleNode = document.createElementNS(NS.SVG, 'circle');
        circleNode.setAttribute('cx', data.x);
        circleNode.setAttribute('cy', data.y);
        circleNode.setAttribute('fill', COLOR.DARK_GRAY);
        circleNode.setAttribute('r', scale * 2);
        circleNode.setAttribute('stroke', COLOR.TRANSPARENT)
        gNode.appendChild(circleNode);

        const rectNode = document.createElementNS(NS.SVG, 'rect');
        rectNode.setAttribute('x', data.x - textWidth / 2);
        rectNode.setAttribute('y', data.y - fontSize / 2);
        rectNode.setAttribute('width', textWidth);
        rectNode.setAttribute('height', textHeight);
        rectNode.setAttribute('rx', fontSize / 4);
        rectNode.setAttribute('ry', fontSize / 4);
        rectNode.setAttribute('fill', COLOR.DARK_GRAY);
        rectNode.setAttribute('stroke', COLOR.TRANSPARENT);
        gNode.appendChild(rectNode);

        const textNode = document.createElementNS(NS.SVG, 'text');
        textNode.setAttribute('x', data.x);
        textNode.setAttribute('y', data.y);
        textNode.setAttribute('text-anchor', 'middle');
        textNode.setAttribute('dominant-baseline', 'central');
        textNode.setAttribute('fill', COLOR.WHITE);
        textNode.setAttribute('font-family', 'sans-serif');
        textNode.setAttribute('font-size', fontSize);
        textNode.appendChild(document.createTextNode(data.name));
        gNode.appendChild(textNode);
    }
}

class MapTrack {
    _gNode;
    _selectionLineNode;
    _track;

    onclick;
    
    get node() { return this._gNode; }

    get selectionMode() { return this._track.selectionMode; }
    set selectionMode(value) {
        let stroke;
        switch (value) {
            case TRACK_SELECTION_MODE.NONE:
                stroke = COLOR.TRANSPARENT;
                break;
            case TRACK_SELECTION_MODE.BUILT:
                stroke = COLOR.BUILT;
                break;
            case TRACK_SELECTION_MODE.STATION:
                stroke = COLOR.STATION;
                break;
            default:
                throw new Error(`Invalid selectionMode: ${value}`);
        }
        this._selectionLineNode.setAttribute('stroke', stroke);
        this._track.selectionMode = value;
    }

    get track() { return this._track; }

    constructor(data, scale) {
        const TRACK_SPACING = 1.25;
        const x1 = data.cities[0].x;
        const x2 = data.cities[1].x;
        const y1 = data.cities[0].y;
        const y2 = data.cities[1].y;
        const xMid = (x1 + x2) / 2;
        const yMid = (y1 + y2) / 2;
        const fontSize = scale * 2;
        const text = data.engines > 0
            ? `${data.engines}+${data.distance - data.engines}`
            : `${data.distance}`;

        this._track = data;

        const gNode = document.createElementNS(NS.SVG, 'g');
        this._gNode = gNode;

        if (data.tunnel) {
            const tunnelLineNode = document.createElementNS(NS.SVG, 'line');
            tunnelLineNode.setAttribute('x1', x1);
            tunnelLineNode.setAttribute('y1', y1);
            tunnelLineNode.setAttribute('x2', x2);
            tunnelLineNode.setAttribute('y2', y2);
            tunnelLineNode.setAttribute('stroke', COLOR.DARK_GRAY);
            tunnelLineNode.setAttribute('stroke-width', scale * (TRACK_SPACING * (data.colors.length - 1) + 2))
            gNode.appendChild(tunnelLineNode);
        }

        const dxCity = x1 - x2;
        const dyCity = y1 - y2;
        const degTrack = Math.atan(dyCity / dxCity) + Math.PI / 2;
        const dxTrack = Math.cos(degTrack) * scale * TRACK_SPACING;
        const dyTrack = Math.sin(degTrack) * scale * TRACK_SPACING;
        for (let i = 0; i < data.colors.length; i++) {
            const drTrack = (1 - data.colors.length) / 2 + i;
            const trackLineNode = document.createElementNS(NS.SVG, 'line');
            trackLineNode.setAttribute('x1', x1 + dxTrack * drTrack);
            trackLineNode.setAttribute('y1', y1 + dyTrack * drTrack);
            trackLineNode.setAttribute('x2', x2 + dxTrack * drTrack);
            trackLineNode.setAttribute('y2', y2 + dyTrack * drTrack);
            trackLineNode.setAttribute('stroke', data.colors[i]);
            trackLineNode.setAttribute('stroke-width', scale)
            gNode.appendChild(trackLineNode);
        }

        {
            const selectionLineNode = document.createElementNS(NS.SVG, 'line');
            selectionLineNode.setAttribute('x1', x1);
            selectionLineNode.setAttribute('y1', y1);
            selectionLineNode.setAttribute('x2', x2);
            selectionLineNode.setAttribute('y2', y2);
            selectionLineNode.setAttribute('stroke', COLOR.TRANSPARENT);
            selectionLineNode.setAttribute('stroke-width', scale * (TRACK_SPACING * (data.colors.length - 1) + 2))
            selectionLineNode.onclick = e => this.onclick ? this.onclick(e) : true;
            gNode.appendChild(selectionLineNode);
            this._selectionLineNode = selectionLineNode;
        }

        const textWidth = fontSize * Math.max(1, text.length * 0.8);
        const textHeight = fontSize;
        const rectNode = document.createElementNS(NS.SVG, 'rect');
        rectNode.setAttribute('x', xMid - textWidth / 2);
        rectNode.setAttribute('y', yMid - textHeight / 2);
        rectNode.setAttribute('width', textWidth);
        rectNode.setAttribute('height', textHeight);
        rectNode.setAttribute('rx', fontSize / 2);
        rectNode.setAttribute('ry', fontSize / 2);
        rectNode.setAttribute('stroke', COLOR.TRANSPARENT)
        rectNode.setAttribute('fill', COLOR.DARK_GRAY);
        rectNode.onclick = e => this.onclick ? this.onclick(e) : true;
        gNode.appendChild(rectNode);

        const textNode = document.createElementNS(NS.SVG, 'text');
        textNode.setAttribute('x', xMid);
        textNode.setAttribute('y', yMid);
        textNode.setAttribute('text-anchor', 'middle');
        textNode.setAttribute('dominant-baseline', 'central');
        textNode.setAttribute('fill', COLOR.WHITE);
        textNode.setAttribute('font-family', 'sans-serif');
        textNode.setAttribute('font-size', fontSize);
        textNode.appendChild(document.createTextNode(text));
        textNode.onclick = e => this.onclick ? this.onclick(e) : true;
        gNode.appendChild(textNode);
    }
}

class Map {
    _divNode;

    onchange;

    get node() { return this._divNode; }

    constructor(data) {
        const scale = Math.min(data.width, data.height) / 100;

        const divNode = document.createElement('div');
        divNode.classList.add('map');
        this._divNode = divNode;

        const svgNode = document.createElementNS(NS.SVG, 'svg');
        svgNode.setAttribute('viewBox', `0 0 ${data.width} ${data.height}`);
        divNode.appendChild(svgNode);

        if (data.image) {
            const imageNode = document.createElementNS(NS.SVG, 'image');
            imageNode.setAttribute('href', data.image);
            imageNode.setAttribute('height', data.height);
            imageNode.setAttribute('width', data.width);
            svgNode.appendChild(imageNode);
        }

        data.tracks.forEach(track => {
            const mapTrack = new MapTrack(track, scale);
            mapTrack.onclick = e => {
                if (mapTrack.selectionMode !== TRACK_SELECTION_MODE.NONE) {
                    mapTrack.selectionMode = TRACK_SELECTION_MODE.NONE;
                } else if (e.ctrlKey) {
                    mapTrack.selectionMode = TRACK_SELECTION_MODE.STATION;
                } else {
                    mapTrack.selectionMode = TRACK_SELECTION_MODE.BUILT;
                }
                e.stopPropagation();
                if (this.onchange) {
                    this.onchange({ target: this });
                }
                return true;
            };
            svgNode.appendChild(mapTrack.node)
        });

        data.cities.forEach(city => svgNode.appendChild(new MapCity(city, scale).node));
    }
}

class Atlas {
    _data;
    _divNode;
    _map;
    _summary;
    _title;

    get node() { return this._divNode; }

    get title() { return this._title; }

    constructor(data) {
        this._data = data;
        this._title = data.title;

        const divNode = document.createElement('div');
        divNode.classList.add('atlas');
        this._divNode = divNode;

        const summary = new Summary(data);
        divNode.appendChild(summary.node);
        this._summary = summary;

        const map = new Map(data);
        map.onchange = e => this._summary.refresh();
        divNode.appendChild(map.node);
        this._map = map;
    }
}

let atlas;

function convertToDomain(data) {
    const colors = {
        'a': 'gray',
        'r': 'red',
        'o': 'orange',
        'y': 'yellow',
        'g': 'green',
        'b': 'blue',
        'p': 'pink',
        'w': 'white',
        'k': 'black',
    };

    if (!data.title) {
        throw new Error('Missing "title"');
    }
    if (data.width <= 0) {
        throw new Error('Missing or invalid "width"');
    }
    if (data.height <= 0) {
        throw new Error('Missing or invalid "height"');
    }

    data.cities.forEach(city => {
        city.tracks = [];
    });
    data.tracks.forEach(track => {
        track.cities = track.cities.map(name => {
            const city = data.cities.find(city => city.name === name);
            if (!city) {
                throw new Error(`Unknown city "${name}"`);
            }
            city.tracks += track;
            return city;
        });
        track.colors = track.colors.map(color => colors[color])
        track.engines = track.engines || 0;
        track.selectionMode = TRACK_SELECTION_MODE.NONE;
        track.tunnel = track.tunnel === true;
    });
    data.routes.forEach(route => {
        route.cities = route.cities.map(name => {
            const city = data.cities.find(city => city.name === name);
            if (!city) {
                throw new Error(`Unknown city "${name}"`);
            }
            return city;
        });
        route.long = route.long === true;
    });

    return data;
}

async function loadAtlas() {
    const urlParams = new URLSearchParams(window.location.search);
    const filename = urlParams.get('atlas');
    if (!filename) {
        return;
    }

    const response = await fetch(`./${filename}.json`);
    if (!response.ok) {
        throw Error(`Failed to get '${filename}.json'. Response status ${response.status} ${response.statusText}`);
    }

    const json = await response.json();
    atlas = new Atlas(convertToDomain(json));
    document.head.querySelector('title').innerText = atlas.title;
    document.body.replaceChildren(atlas.node);
}

window.onload = () => {
    window.onload = undefined;

    loadAtlas().catch(reason => {
        const errorNode = document.createElement('p');
        errorNode.classList.add('error');
        errorNode.appendChild(document.createTextNode(reason));
        document.body.appendChild(errorNode);
        console.error(reason);
    });
}
