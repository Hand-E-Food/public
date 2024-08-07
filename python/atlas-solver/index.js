const Color = {
    darkGray: '#666',
    default: null,
    excluded: 'red',
    highlight: 'gold',
    included: 'lightgreen',
    player: 'purple',
    station: 'plum',
    transparent: 'transparent',
    white: 'white',
};

const NS = {
    SVG: 'http://www.w3.org/2000/svg',
};

const TrackBuilt = {
    no: 0,
    player: 1,
    station: 2,
}

const ScoreMode = {
    none: 0,
    included: 1,
    excluded: 2,
}

const TrackPoints = {
    1: 1,
    2: 2,
    3: 4,
    4: 7,
    5: 11,
    6: 15,
    7: 18,
    8: 21,
};

Array.prototype.remove = function(item) {
    var index = this.indexOf(item);
    if (index !== -1) {
        this.splice(index, 1);
        return true;
    } else {
        return false;
    }
}

Array.prototype.removeFirst = function(predicate, thisArg) {
    var index = this.findIndex(predicate, thisArg);
    if (index !== -1) {
        return this.splice(index, 1)[0];
    } else {
        return undefined;
    }
}

function blockEvent(e) {
    e.preventDefault();
    e.stopPropagation();
    return false;
}

function pluralise(count, singular, plural) {
    if (!plural) plural = singular + 's';
    return `${count} ${count === 1 ? singular : plural}`;
}

class EventSource {
    _handlers = new Set();

    constructor() { }

    addHandler(callback) {
        this._handlers.add(callback);
    }

    removeHandler(callback) {
        this._handlers.remove(callback);
    }

    invoke(...params) {
        this._handlers.forEach(callback => callback(...params))
    }
}

/**
 * A city junction from which any number of routes extend.
 */
class JunctionCity {
    /**
     * The city in which this junction exists.
     * @type {MapCity}
     * @private
     */
    _city;

    /**
     * The routes extending from this junction.
     * @type {JunctionRoute[]}
     * @private
     */
    _routes;

    /**
     * The city in which this junction exists.
     * @type {MapCity}
     */
    get city() { return this._city; }

    /**
     * The routes extending from this junction.
     * @type {JunctionRoute[]}
     */
    get routes() { return this._routes; }

    /**
     * Creates a junction in a city.
     * @param {MapCity} city The city in which this junction exists.
     * @param {JunctionRoute[]} routes The routes extending from this junction.
     */
    constructor(city, routes) {
        this._city = city;
        this._routes = routes;
    }

    /**
     * Adds the specified route to this junction.
     * @param {JunctionRoute} route The route to add.
     */
    addRoute(route) {
        this._routes.push(route);
    }

    /**
     * Removes the specified route from this junction.
     * @param {JunctionRoute} route The route to remove.
     */
    removeRoute(route) {
        this._routes.remove(route);
    }
}

/**
 * A route between two cities that can be traversed without making a significant decision.
 */
class JunctionRoute {
    /**
     * The cities at either end of this route.
     * @type {MapCity[]}
     * @private
     */
    _cities;

    /**
     * This route's distance.
     * @type {number}
     * @private
     */
    _distance;

    /**
     * The tracks traversed by this route.
     * @type {MMapTrack[]}
     * @private
     */
    _tracks;

    /**
     * The cities at either end of this route.
     * @type {MapCity[]}
     */
    get cities() { return this._cities; }

    /**
     * This route's distance.
     * @type {number}
     */
    get distance() { return this._distance; }

    /**
     * true if this route is a loop.
     * @type {boolean}
     */
    get isLoop() { return this._cities[0] === this._cities[1]; }

    /**
     * The tracks traversed by this route.
     * @type {MMapTrack[]}
     */
    get tracks() { return this._tracks; }

    /**
     * @private
     * Do not use. Use a static method instead.
     */
    constructor() { }

    /**
     * Creates a route along a single piece of track.
     * @param {MapTrack} track The track forming this route.
     * @returns {JunctionRoute} The route.
     */
    static fromTrack(track) {
        const route = new JunctionRoute();
        route._cities = track.cities;
        route._distance = track.distance;
        route._tracks = [ track ];
        return route;
    }

    /**
     * Gets the city at the other end of this route from the specified city.
     * @param {MapCity} city The origin city.
     * @returns {MapCity} The city at the other end of this route.
     */
    getOtherCity(city) {
        // If this route is a loop, both cities are the same, so return that one city.
        return this._cities[0] !== city
            ? this._cities[0]
            : this._cities[1];
    }

    /**
     * Creates a new route by joining two routes.
     * @param {MapCity} commonCity The city in which to perform the join.
     * @param {JunctionRoute} routes The routes to join.
     * @returns {JunctionRoute} The joined route.
     */
    static joinRoutes(commonCity, routes) {
        if (routes.length !== 2) throw new Error("Can only join two routes.");
        const joinedRoute = new JunctionRoute();
        joinedRoute._cities = routes.map(route => route.getOtherCity(commonCity));
        joinedRoute._distance = routes[0].distance + routes[1].distance;
        joinedRoute._tracks = [ ...routes[0].tracks, ...routes[1].tracks ];
        return joinedRoute;
    }
}

/**
 * A collection of junctions.
 */
class JunctionMap {
    /**
     * All junction cities in this collection.
     * @type {JunctionCity[]}
     * @private
     */
    _cities;

    /**
     * The routes joining the junctions in this collection.
     * @type {JunctionRoute[]}
     * @private
     */
    _routes;

    /**
     * All junction cities in this collection.
     * @type {JunctionCity[]}
     */
    get cities() { return this._cities; }

    /**
     * The routes joining the junctions in this collection.
     * @type {JunctionRoute[]}
     */
    get routes() { return this._routes; }

    /**
     * Creates a collection of junctions.
     * @param {MapTrack[]} mapTracks The built tracks that will form routes.
     */
    constructor(mapTracks) {
        this._routes = mapTracks.map(mapTrack => JunctionRoute.fromTrack(mapTrack));

        const mapCities = new Set();
        mapTracks.forEach(mapTrack => mapTrack.cities.forEach(mapCity => mapCities.add(mapCity)));
        this._cities = [...mapCities]
            .map(mapCity => new JunctionCity(
                mapCity,
                this._routes.filter(junctionRoute => junctionRoute.cities.some(routeCity => routeCity === mapCity)),
            ));
    }

    /**
     * Finds the junction for the specified city.
     * @param {MapCity} mapCity The city to find.
     * @returns {JunctionCity} The junction for the specified city.
     */
    getByCity(mapCity) {
        return this._cities.find(junctionCity => junctionCity.city === mapCity);
    }

    /**
     * Gets the junction at the other end of the route.
     * @param {JunctionCity} junctionCity The junction to start from.
     * @param {JunctionRoute} junctionRoute The route to follow.
     * @returns {JunctionCity} The junction at the other end of the route.
     */
    getOtherCity(junctionCity, junctionRoute) {
        return this.getByCity(junctionRoute.getOtherCity(junctionCity.city));
    }

    /**
     * Joins two routes into a single route. Fixes junctions.
     * @param {JunctionCity} junctionCity The junction at which the join occurs.
     * @param {JunctionRoute[]} junctionRoutes Two routes to join.
     * @returns {JunctionRoute} The joined route.
     */
    joinRoutes(junctionCity, junctionRoutes) {
        junctionRoutes = [...junctionRoutes];
        const joinedRoute = JunctionRoute.joinRoutes(junctionCity.city, junctionRoutes);
        junctionRoutes.forEach(junctionRoute => this.removeRoute(junctionRoute), this);
        this._routes.push(joinedRoute);
        this.getByCity(joinedRoute.cities[0]).addRoute(joinedRoute);
        if (!joinedRoute.isLoop)
            this.getByCity(joinedRoute.cities[1]).addRoute(joinedRoute);

        return joinedRoute;
    }

    /**
     * Removes the junction for the specified city from this collecdtion.
     * @param {MapCity} mapCity The city to remove.
     */
    removeCity(mapCity) {
        this._cities.removeFirst(junctionCity => junctionCity.city === mapCity);
    }

    /**
     * Removes the specified junction from this collection.
     * @param {JunctionCity} junctionCity The junction to remove.
     */
    removeJunction(junctionCity) {
        this._cities.remove(junctionCity);
    }

    /**
     * Removes the specified route from its junctions.
     * @param {JunctionRoute} junctionRoute The route to remove.
     */
    removeRoute(junctionRoute) {
        junctionRoute.cities
            .map(city => this.getByCity(city), this)
            .forEach(junctionCity => junctionCity.removeRoute(junctionRoute));
        this._routes.remove(junctionRoute);
    }
}

/**
 * A result of finding the longest track.
 */
class LongestTrackResult {
    /**
     * The total distance of this result.
     */
    distance = 0;

    /**
     * The tracks in this reuslt.
     * @type {MapTrack[]}
     */
    tracks = [];
}

class Map {
    _cities;
    _divNode;
    _onChange = new EventSource();
    _routes;
    _networks = [];
    _tracks;

    get cities() { return this._cities; }

    get networks() { return this._networks; }
    
    get node() { return this._divNode; }

    get onChange() { return this._onChange; }

    get routes() { return this._routes; }

    get tracks() { return this._tracks; }

    constructor(model) {
        if (!isFinite(model.width ) || model.width  <= 0) {
            console.log(model.width);
            throw new Error('Missing or invalid "width"');
        }
        if (!isFinite(model.height) || model.height <= 0) {
            console.log(model.height);
            throw new Error('Missing or invalid "height"');
        }

        const scale = Math.min(model.width, model.height) / 100;

        this._cities = model.cities.map(city => new MapCity(
            city.name,
            city.x,
            city.y,
            scale,
        ));

        this._routes = model.routes.map(route => new MapRoute(
            route.cities.map(name => this.getCity(name)),
            route.long,
            route.points,
        ), this);

        this._tracks = model.tracks.map(track => new MapTrack(
            track.cities.map(name => this.getCity(name)),
            track.distance,
            track.colors,
            track.engines,
            track.tunnel,
            scale,
        ), this);

        const divNode = document.createElement('div');
        divNode.classList.add('map');
        this._divNode = divNode;

        const svgNode = document.createElementNS(NS.SVG, 'svg');
        svgNode.setAttribute('viewBox', `0 0 ${model.width} ${model.height}`);
        svgNode.onclick = blockEvent;
        svgNode.oncontextmenu = blockEvent;
        divNode.appendChild(svgNode);

        if (model.image) {
            const imageNode = document.createElementNS(NS.SVG, 'image');
            imageNode.setAttribute('href', model.image);
            imageNode.setAttribute('height', model.height);
            imageNode.setAttribute('width', model.width);
            svgNode.appendChild(imageNode);
        }

        this._tracks.forEach(mapTrack => {
            mapTrack.onChange.addHandler(e => this._mapTrack_onChange(e));
            svgNode.appendChild(mapTrack.node)
        });

        this._cities.forEach(mapCity => {
            svgNode.appendChild(mapCity.node)
        });
    }

    getCity(name) {
        const city = this._cities.find(city => city.name === name);
        if (!city) throw new Error(`Unknown city "${name}"`);
        return city;
    }

    _getTrackNetworks() {
        let networks = this._tracks
            .filter(track => track.isBuilt !== TrackBuilt.no)
            .map(track => {
                const cities = new Set();
                const tracks = [ track ];
                track.cities.forEach(city => cities.add(city));
                return { cities: cities, tracks: tracks };
            });
        
        for (let a = networks.length - 2; a >= 0; a--) {
            const networkA = networks[a];
            const citiesA = networkA.cities;
            for (let b = networks.length - 1; b > a; b--) {
                const networkB = networks[b];
                const citiesB = [...networkB.cities];
                if (citiesB.some(city => citiesA.has(city))) {
                    citiesB.forEach(city => citiesA.add(city));
                    networkA.tracks.push(...networkB.tracks);
                    networks.splice(b, 1);
                }
            }
        }

        networks.forEach(network => {
            network.tracks.sort((a, b) => a.distance - b.distance);
        });

        return networks;
    }

    isRouteCompleted(mapRoute) {
        return this._networks
            .map(network => network.cities)
            .some(networkCities =>
                mapRoute.cities.every(routeCity =>
                    networkCities.has(routeCity)
                )
            );
    }

    _mapTrack_onChange(e) {
        this._networks = this._getTrackNetworks();
        this._routes.forEach(route => route.isCompleted = this.isRouteCompleted(route));
        this.onChange.invoke({ map: this, mapTrack: e.mapTrack });
    }
}

class MapCity {
    _gNode;
    _highlight = false;
    _name;
    _onChange = new EventSource();
    _routes = [];
    _score = 0;
    _scoreNode;
    _tracks = [];
    _x;
    _y;
    
    get highlight() { return this._highlight; }
    set highlight(value) {
        if (this._highlight === value) return;
        this._highlight = value;
        this._gNode.setAttribute('fill', value ? Color.highlight : Color.darkGray);
    }

    get name() { return this._name; }

    get node() { return this._gNode; }

    get onChange() { return this._onChange; }

    get routes() { return this._routes; }

    get score() { return this._score; }
    set score(value) {
        if (this._score === value) return;
        this._score = value;
        this._scoreNode.nodeValue = value;
    }

    get tracks() { return this._tracks; }

    get x() { return this._x; }

    get y() { return this._y; }

    constructor(name, x, y, scale) {
        if (!name) throw new Error('Missing "name"');
        if (!isFinite(x) || x <= 0) throw new Error('Missing or invalid "x"');
        if (!isFinite(y) || y <= 0) throw new Error('Missing or invalid "y"');

        this._name = name;
        this._x = x;
        this._y = y;

        const fontSize = scale * 1.5;
        const textHeight = fontSize; 
        const textWidth = fontSize * name.length * 0.6;

        const gNode = document.createElementNS(NS.SVG, 'g');
        gNode.setAttribute('fill', Color.darkGray);
        gNode.setAttribute('stroke', Color.transparent)
        this._gNode = gNode;

        const circleNode = document.createElementNS(NS.SVG, 'circle');
        circleNode.setAttribute('cx', x);
        circleNode.setAttribute('cy', y);
        circleNode.setAttribute('r', scale * 2);
        circleNode.onclick = blockEvent;
        circleNode.oncontextmenu = blockEvent;
        circleNode.onmouseenter = e => this._handleHover(true);
        circleNode.onmouseleave = e => this._handleHover(false);
        gNode.appendChild(circleNode);

        const rectNode = document.createElementNS(NS.SVG, 'rect');
        rectNode.setAttribute('x', x - textWidth / 2);
        rectNode.setAttribute('y', y - textHeight);
        rectNode.setAttribute('width', textWidth);
        rectNode.setAttribute('height', textHeight);
        rectNode.setAttribute('rx', fontSize / 4);
        rectNode.setAttribute('ry', fontSize / 4);
        rectNode.onclick = blockEvent;
        rectNode.oncontextmenu = blockEvent;
        rectNode.onmouseenter = e => this._handleHover(true);
        rectNode.onmouseleave = e => this._handleHover(false);
        gNode.appendChild(rectNode);

        const nameTextNode = document.createElementNS(NS.SVG, 'text');
        nameTextNode.setAttribute('x', x);
        nameTextNode.setAttribute('y', y - textHeight / 2);
        nameTextNode.setAttribute('text-anchor', 'middle');
        nameTextNode.setAttribute('dominant-baseline', 'central');
        nameTextNode.setAttribute('fill', Color.white);
        nameTextNode.setAttribute('font-family', 'sans-serif');
        nameTextNode.setAttribute('font-size', fontSize);
        nameTextNode.appendChild(document.createTextNode(name));
        nameTextNode.onclick = blockEvent;
        nameTextNode.oncontextmenu = blockEvent;
        nameTextNode.onmouseenter = e => this._handleHover(true);
        nameTextNode.onmouseleave = e => this._handleHover(false);
        gNode.appendChild(nameTextNode);

        const scoreTextNode = document.createElementNS(NS.SVG, 'text');
        scoreTextNode.setAttribute('x', x);
        scoreTextNode.setAttribute('y', y + textHeight / 2);
        scoreTextNode.setAttribute('text-anchor', 'middle');
        scoreTextNode.setAttribute('dominant-baseline', 'central');
        scoreTextNode.setAttribute('fill', Color.white);
        scoreTextNode.setAttribute('font-family', 'sans-serif');
        scoreTextNode.setAttribute('font-size', fontSize);
        scoreTextNode.setAttribute('font-weight', 'bold');
        scoreTextNode.onclick = blockEvent;
        scoreTextNode.oncontextmenu = blockEvent;
        scoreTextNode.onmouseenter = e => this._handleHover(true);
        scoreTextNode.onmouseleave = e => this._handleHover(false);
        gNode.appendChild(scoreTextNode);

        this._scoreNode = document.createTextNode('0');
        scoreTextNode.appendChild(this._scoreNode);
    }

    addRoute(route) {
        this._routes.push(route);
        route.onChange.addHandler(e => this._refreshScore());
        this._refreshScore();
    }

    addTrack(track) {
        this._tracks.push(track);
    }

    _handleHover(value) {
        this.highlight = value;
        this._onChange.invoke({ mapCity: this });
        return true;
    }

    toDebug() {
        return `MapCity {name: "${this._name}", score: ${this._score}}`;
    }

    _refreshScore() {
        this.score = this._routes
            .filter(route => route.isCompleted && !route.isLong)
            .map(route => route.points)
            .reduce((a, b) => a + b, 0);
    }
}

class MapRoute {
    _cities;
    _isCompleted = false;
    _isLong;
    _onChange = new EventSource();
    _points;

    get cities() { return this._cities; }

    get isCompleted() { return this._isCompleted; }
    set isCompleted(value) {
        if (this._isCompleted === value) return;
        this._isCompleted = value;
        this._onChange.invoke({ mapRoute: this });
    }

    get isLong() { return this._isLong; }

    get onChange() { return this._onChange; }

    get points() { return this._points; }

    constructor(cities, isLong, points) {
        if (!cities || cities.length !== 2) {
            console.log(cities);
            throw new Error('Missing or invalid "cities"');
        }
        if (!isFinite(points) || points <= 0) {
            console.log(points);
            throw new Error('Missing or invalid "points"');
        }

        this._cities = cities;
        this._isLong = isLong === true;
        this._points = points;

        cities.forEach(city => city.addRoute(this));
    }

    toDebug() {
        return `MapRoute {cities: ["${this._cities.map(city => city.name).join('", "')}"], points: ${this._points}, isLong: ${this._isLong}, isCompleted: ${this._isCompleted}}`;
    }
}

class MapTrack {
    _builtLineNode;
    _cities;
    _distance;
    _gNode;
    _highlight = false;
    _isBuilt = TrackBuilt.no;
    _onChange = new EventSource();
    
    get cities() { return this._cities; }

    get distance() { return this._distance; }

    get highlight() { return this._highlight; }
    set highlight(value) {
        if (this._highlight === value) return;
        this._highlight = value;
        this._refreshColor();
    }

    get isBuilt() { return this._isBuilt; }
    set isBuilt(value) {
        if (this._isBuilt === value) return;
        this._isBuilt = value
        this._onChange.invoke({ mapTrack: this });
        this._refreshColor();
    }

    get node() { return this._gNode; }

    get onChange() { return this._onChange; }

    constructor(cities, distance, colors, engines, tunnel, scale) {
        if (!cities || cities.length !== 2) {
            console.log(cities);
            throw new Error('Missing or invalid "cities"');
        }
        if (!isFinite(distance) || distance <= 0) {
            console.log(distance);
            throw new Error('Missing or invalid "distance"');
        }
        if (!colors) {
            console.log(colors);
            throw new Error('Missing or invalid "colors"');
        }

        this._cities = cities;
        this._distance = distance;

        cities.forEach(city => city.addTrack(this));

        const TRACK_SPACING = 1.25;
        const lineScale = scale * 0.5;
        const x1 = cities[0].x;
        const x2 = cities[1].x;
        const y1 = cities[0].y;
        const y2 = cities[1].y;
        const xMid = (x1 + x2) / 2;
        const yMid = (y1 + y2) / 2;
        const fontSize = scale * 2;
        const text = engines ? `${engines}+${distance - engines}` : `${distance}`;

        const gNode = document.createElementNS(NS.SVG, 'g');
        this._gNode = gNode;

        if (tunnel) {
            const tunnelLineNode = document.createElementNS(NS.SVG, 'line');
            tunnelLineNode.setAttribute('x1', x1);
            tunnelLineNode.setAttribute('y1', y1);
            tunnelLineNode.setAttribute('x2', x2);
            tunnelLineNode.setAttribute('y2', y2);
            tunnelLineNode.setAttribute('stroke', Color.darkGray);
            tunnelLineNode.setAttribute('stroke-width', lineScale * (TRACK_SPACING * (colors.length - 1) + 2))
            gNode.appendChild(tunnelLineNode);
        }

        const dxCity = x1 - x2;
        const dyCity = y1 - y2;
        const degTrack = Math.atan(dyCity / dxCity) + Math.PI / 2;
        const dxTrack = Math.cos(degTrack) * lineScale * TRACK_SPACING;
        const dyTrack = Math.sin(degTrack) * lineScale * TRACK_SPACING;
        const colorNames = {
            'a': 'gray',
            'r': 'red',
            'o': 'orange',
            'y': 'yellow',
            'g': 'green',
            'b': 'blue',
            'p': 'pink',
            'w': 'white',
            'k': 'black',
            '?': 'brown',
        };
        for (let i = 0; i < colors.length; i++) {
            const drTrack = (1 - colors.length) / 2 + i;
            const trackLineNode = document.createElementNS(NS.SVG, 'line');
            trackLineNode.setAttribute('x1', x1 + dxTrack * drTrack);
            trackLineNode.setAttribute('y1', y1 + dyTrack * drTrack);
            trackLineNode.setAttribute('x2', x2 + dxTrack * drTrack);
            trackLineNode.setAttribute('y2', y2 + dyTrack * drTrack);
            trackLineNode.setAttribute('stroke', colorNames[colors[i]]);
            trackLineNode.setAttribute('stroke-width', lineScale)
            gNode.appendChild(trackLineNode);
        }

        const fullWidth = lineScale * (TRACK_SPACING * (colors.length - 1) + 2);
        const builtLineNode = document.createElementNS(NS.SVG, 'line');
        builtLineNode.setAttribute('x1', x1);
        builtLineNode.setAttribute('y1', y1);
        builtLineNode.setAttribute('x2', x2);
        builtLineNode.setAttribute('y2', y2);
        builtLineNode.setAttribute('stroke', Color.transparent);
        builtLineNode.setAttribute('stroke-width', fullWidth);
        builtLineNode.onclick = e => this._mouseLineNode_onclick(e);
        gNode.appendChild(builtLineNode);
        this._builtLineNode = builtLineNode;

        const textWidth = fontSize * Math.max(1, text.length * 0.8);
        const textHeight = fontSize;
        const rectNode = document.createElementNS(NS.SVG, 'rect');
        rectNode.setAttribute('x', xMid - textWidth / 2);
        rectNode.setAttribute('y', yMid - textHeight / 2);
        rectNode.setAttribute('width', textWidth);
        rectNode.setAttribute('height', textHeight);
        rectNode.setAttribute('rx', fontSize / 2);
        rectNode.setAttribute('ry', fontSize / 2);
        rectNode.setAttribute('stroke', Color.transparent)
        rectNode.setAttribute('fill', Color.darkGray);
        gNode.appendChild(rectNode);

        const textNode = document.createElementNS(NS.SVG, 'text');
        textNode.setAttribute('x', xMid);
        textNode.setAttribute('y', yMid);
        textNode.setAttribute('text-anchor', 'middle');
        textNode.setAttribute('dominant-baseline', 'central');
        textNode.setAttribute('fill', Color.white);
        textNode.setAttribute('font-family', 'sans-serif');
        textNode.setAttribute('font-size', fontSize);
        textNode.appendChild(document.createTextNode(text));
        gNode.appendChild(textNode);

        const mouseLineNode = document.createElementNS(NS.SVG, 'line');
        mouseLineNode.setAttribute('x1', x1);
        mouseLineNode.setAttribute('y1', y1);
        mouseLineNode.setAttribute('x2', x2);
        mouseLineNode.setAttribute('y2', y2);
        mouseLineNode.setAttribute('stroke', Color.transparent);
        mouseLineNode.setAttribute('stroke-width', fullWidth)
        mouseLineNode.onclick = e => this._mouseLineNode_onclick(e);
        mouseLineNode.oncontextmenu = e => this._mouseLineNode_onclick(e);
        gNode.appendChild(mouseLineNode);
    }

    toDebug() {
        return `MapTrack {cities: ["${this._cities.map(city => city.name).join('", "')}"], distance: ${this._distance}, isBuilt: ${['no', 'player', 'station'][this._isBuilt]}}`;
    }

    _mouseLineNode_onclick(e) {
        const mode = e.which === 3 || e.button === 2 ? TrackBuilt.station : TrackBuilt.player;
        this.isBuilt = this.isBuilt === mode ? TrackBuilt.no : mode;
        e.preventDefault();
        e.stopPropagation();
        return false;
    }

    _refreshColor() {
        let stroke;
        if (this.highlight) {
            stroke = Color.highlight;
        } else {
            switch (this.isBuilt) {
                case TrackBuilt.no:
                    stroke = Color.transparent;
                    break;
                case TrackBuilt.player:
                    stroke = Color.player;
                    break;
                case TrackBuilt.station:
                    stroke = Color.station;
                    break;
                default:
                    throw new Error(`Invalid isBuilt: ${value}`);
            }
        }
        this._builtLineNode.setAttribute('stroke', stroke);
    }
}

class ScoreCard {
    _divNode;
    _map;
    _scoreItems = [];
    _scoreTotal;

    get node() { return this._divNode; }

    constructor(map) {
        this._map = map;

        const sortedRoutes = [...map.routes].sort((a, b) => {
            if (b.points !== a.points) return b.points - a.points;
            if (a.name === b.name) return 0;
            return a.name < b.name ? -1 : 1;
        })

        const divNode = document.createElement('div');
        divNode.classList.add('score');
        this._divNode = divNode;

        const tableNode = document.createElement('table');
        divNode.appendChild(tableNode);

        const scoreLongRoutes = sortedRoutes
            .filter(route => route.isLong)
            .map(route => new ScoreLongRoute(route));
        scoreLongRoutes.forEach(thisRoute => {
            thisRoute.longRoutes = scoreLongRoutes;
        });

        const scoreShortRoutes = sortedRoutes
            .filter(route => !route.isLong)
            .map(route => new ScoreRoute(route));

        this._scoreItems = [
            new ScoreTracks(map),
            new ScoreRemainingStations(map),
            new ScoreLongestTrack(map),
            ...scoreLongRoutes,
            ...scoreShortRoutes,
        ];

        this._scoreTotal = new ScoreTotal();
        tableNode.appendChild(this._scoreTotal.node);

        this._scoreItems.forEach(scoreItem => {
            scoreItem.onChange.addHandler(e => this.refresh());
            tableNode.appendChild(scoreItem.node)
        });

        this.refresh();
    }

    refresh() {
        this.isValid = this._scoreItems.every(item => item.isValid);

        this._scoreTotal.mode = this.isValid ? ScoreMode.none : ScoreMode.excluded;

        this._scoreTotal.points = this._scoreItems
            .map(item => item.score)
            .reduce((a, b) => a + b, 0);
    }
}

class ScoreItem {
    _highlight = false;
    isValid = true;
    _mode = ScoreMode.none;
    _onChange = new EventSource();
    _pointsTdNode;
    _points;
    _textTdNode;
    _trNode;

    get highlight() { return this._highlight; }
    set highlight(value) {
        if (this._highlight === value) return;
        this._highlight = value;
        this._refreshColor();
    }

    get mode() { return this._mode; }
    set mode(value) {
        if (this._mode === value) return;
        this._mode = value;
        this._refreshColor();
        this._raiseOnChange();
    }

    get node() { return this._trNode; }

    get onChange() { return this._onChange; }

    get points() { return parseInt(this._pointsTdNode.innerText); }
    set points(value) {
        if (this._points === value) return;
        this._points = value;
        this._pointsTdNode.innerText = value;
        this._raiseOnChange();
    }

    get score() { return this._mode === ScoreMode.included ? this._points : 0; }

    get text() { return this._textTdNode.innerText; }
    set text(value) { this._textTdNode.innerText = value; }

    get textColor() { return this._textTdNode.style.color; }
    set textColor(value) { this._textTdNode.style.color = value; }

    constructor() {
        this._trNode = document.createElement('tr');
        this._trNode.onmouseenter = e => this._handleHover(true);
        this._trNode.onmouseleave = e => this._handleHover(false);
        
        this._textTdNode = document.createElement('td');
        this._trNode.appendChild(this._textTdNode);
        
        this._pointsTdNode = document.createElement('td');
        this._pointsTdNode.style.textAlign = 'right';
        this._trNode.appendChild(this._pointsTdNode);
    }

    _handleHover(value) { }

    toDebug() {
        return `ScoreItem {text: "${this._text}", points: ${this._points}, mode: ${['none', 'included', 'excluded'][this._mode]}}`;
    }

    _raiseOnChange() {
        this._onChange.invoke({ scoreItem: this });
    }

    refresh() {
        throw new ReferenceError('Function "refresh" is not overridden.');
    }

    _refreshColor() {
        const color = this._highlight
            ? Color.highlight
            : [Color.default, Color.included, Color.excluded][this._mode];

        this._pointsTdNode.style.color = color;
        this._textTdNode.style.color = color;
    }
}

/** A score item for the longest track. */
class ScoreLongestTrack extends ScoreItem {
    /**
     * All junctions in the latest refresh.
     * @type {JunctionMap}
     * @private
     */
    _junctions;

    /**
     * The longest track calculated in the latest refresh.
     * @type {LongestTrackResult}
     * @private
     */
    _longestTrack = new LongestTrackResult();

    /** 
     * Every track on the map.
     * @type {MapTrack[]}
     * @private
     */
    _tracks;

    /**
     * Creates a score item for the longest track.
     * @param {Map} map The map to score.
     */
    constructor(map) {
        super();
        this.points = 10;

        this._tracks = map.tracks;
        map.onChange.addHandler(e => this.refresh());
        this.refresh();
    }

    /**
     * Finds the longest track.
     * @returns {LongestTrackResult} The tracks and distance of the longest track.
     * @private
     */
    _findLongestTrack() {
        const tracks = this._tracks
            .filter(track => track.isBuilt === TrackBuilt.player);
        if (tracks.length === 0) return [];

        const junctions = new JunctionMap(tracks);
        this._junctions = junctions;
        this._simplifyJunctionMap();
        return this._junctions.cities
            .sort((a, b) => a.routes.length - b.routes.length)
            .reduce((bestResult, junctionCity) => {
                const result = this._findLongestTrackFrom(junctionCity, [...junctions.routes]);
                return result.distance > bestResult.distance ? result : bestResult;
            }, new LongestTrackResult());
    }
    
    /**
     * Finds the longest track starting from the specified city using the specified routes.
     * @param {JunctionCity} junctionCity The junction from which to find the longest track.
     * @param {JunctionRoute[]} unusedRoutes The routes to consider using.
     * @returns {LongestTrackResult} The tracks and distance of the longest track.
     * @private
     */
    _findLongestTrackFrom(junctionCity, unusedRoutes) {
        const junctions = this._junctions;
        return junctionCity.routes
            .filter(junctionRoute => unusedRoutes.indexOf(junctionRoute) !== -1)
            .reduce((bestResult, junctionRoute) => {
                let result = this._findLongestTrackFrom(
                    junctions.getOtherCity(junctionCity, junctionRoute),
                    unusedRoutes.filter(route => route !== junctionRoute),
                );
                result.tracks.splice(0, 0, ...junctionRoute.tracks);
                result.distance += junctionRoute.distance;
                return result.distance > bestResult.distance ? result : bestResult;
            }, new LongestTrackResult());
    }

    /**
     * Highlights the longest track on the UI.
     * @param {boolean} value true to highlight the track.
     * @override
     * @private
     */
    _handleHover(value) {
        this._longestTrack.tracks.forEach(track => {
            track.highlight = value;
        });
    }

    /**
     * Recalculates the player's longest track and updates the UI.
     * @override
     */
    refresh() {
        this._longestTrack = this._findLongestTrack();
        this.mode = this._longestTrack.distance > 0 ? ScoreMode.included : ScoreMode.none;
        this.text = `Longest track (${this._longestTrack.distance})`;
    }

    /**
     * Simplifies junctions and routes.
     * @returns {boolean} true if one or more junctions or routes were simplified.
     * @private
     */
    _simplifyJunctionMap() {
        console.log('----------');
        let dirty = [ true ];
        while(dirty.length > 0) {
            const junctions = [...this._junctions.cities];
            console.log(junctions.map(junction => junction.city.name + ': ' + junction.routes.map(route => `${route.getOtherCity(junction.city).name} (${route.tracks.length}${route.isLoop ? ' loop' : ''})`).join(', ')).join('\n'));
            dirty = [];
            if (dirty.length === 0) dirty = junctions.map(junction => this._simplifyPassthrough(junction), this).filter(x => x);
            if (dirty.length === 0) dirty = junctions.map(junction => this._simplifyEmpty(junction), this).filter(x => x);
            if (dirty.length === 0) dirty = junctions.map(junction => this._simplifyLoop(junction), this).filter(x => x);
            if (dirty.length === 0) dirty = junctions.map(junction => this._simplifyDeadEnd(junction), this).filter(x => x);
            if (dirty.length === 0) dirty = junctions.map(junction => this._simplifyTriplets(junction), this).filter(x => x);
            console.log(dirty.join('\n'));
        }
    }

    /**
     * Simplifies a junction by removing shorter dead end routes.
     * @param {JunctionCity} junction The junction to simplify.
     * @returns {boolean} true if the junction was simplified.
     * @private
     */
    _simplifyDeadEnd(junction) {
        const junctions = this._junctions;

        const endpoints = junction.routes
            .map(route => ({ route, branches: junctions.getOtherCity(junction, route).routes.length }))
            .sort((a, b) => {
                let result = b.route.distance - a.route.distance;
                if (result !== 0) return result;
                result = b.branches - a.branches;
                return result;
            });
        
        let dirty = false;
        while(endpoints.length > 2) {
            const endpoint = endpoints.pop();
            if (endpoint.branches === 1) {
                junctions.removeRoute(endpoint.route);
                dirty = true;
            }
        }

        return dirty ? `Removed dead ends from ${junction.city.name}.` : null;
    }

    /**
     * Simplifies a junction that has exactly 0 routes by removing it.
     * @param {JunctionCity} junction The junction to simplify.
     * @returns {boolean} true if the junction was simplified.
     * @private
     */
    _simplifyEmpty(junction) {
        if (junction.routes.length !== 0) return null;

        this._junctions.removeJunction(junction);
        return `Removed empty junction in ${junction.city.name}.`;
    }

    /**
     * Simplifies a junction by joining looped routes with other routes.
     * @param {JunctionCity} junction The junction to simplify.
     * @returns {boolean} true if the junction was simplified.
     * @private
     */
     _simplifyLoop(junction) {
        const loopRoutes = junction.routes.filter(route => !route.isLoop).length <= 2
            ? [...junction.routes]
            : junction.routes.filter(route => route.isLoop);

        if (loopRoutes.length <= 1) return null;

        console.log(loopRoutes);

        const junctions = this._junctions;
        let route = loopRoutes.pop();
        while (loopRoutes.length > 0) {
            route = junctions.joinRoutes(junction, [route, loopRoutes.pop()]);
        }
        return `Express loops ${junction.city.name} from ${route.cities[0].name} to ${route.cities[1].name}.`;
    }

    /**
     * Simplifies a junction that has exactly 2 routes by combining the routes and removing this junction.
     * @param {JunctionCity} junction The junction to simplify.
     * @returns {boolean} true if the junction was simplified.
     * @private
     */
    _simplifyPassthrough(junction) {
        if (junction.routes.length !== 2) return null;

        const junctions = this._junctions;
        const route = junctions.joinRoutes(junction, junction.routes);
        return `Express through ${junction.city.name} from ${route.cities[0].name} to ${route.cities[1].name}.`;
    }

    /**
     * Simplifies a junction that has 3 or more routes to the same city by combining the longest pair of routes.
     * @param {JunctionCity} junction The junction to simplify.
     * @returns {boolean} true if the junction was simplified.
     * @private
     */
    _simplifyTriplets(junction) {
        const junctions = this._junctions;
        let dirty = [];
        junction.routes
            .reduce((result, route) => {
                const city = route.getOtherCity(junction.city);
                let entry = result.find(x => x.city === city);
                if (!entry) {
                    entry = { city: city, routes: [] };
                    result.push(entry);
                }
                entry.routes.push(route);
                return result;
            }, [])
            .filter(group => group.routes.length >= 3)
            .forEach(group => {
                const city = group.city;
                const routes = group.routes;
                routes.sort((a, b) => b.distance - a.distance);
                while(routes.length >= 3) {
                    junctions.joinRoutes(city, routes.splice(0, 2));
                    dirty.push(`Express braids ${junction.city.name} with ${city.name}.`);
                };
            });

        return dirty.length === 0 ? null : dirty.join('\n');
    }
}

class ScoreRemainingStations extends ScoreItem {
    _tracks;

    constructor(map) {
        super();
        this.mode = ScoreMode.included;

        this._tracks = map.tracks;
        map.onChange.addHandler(e => this.refresh());
        this.refresh();
    }

    getRemainingStations() {
        return 3 - this._tracks
            .filter(track => track.isBuilt === TrackBuilt.station)
            .length;
    }

    refresh() {
        const remainingStations = this.getRemainingStations();
        this.isValid = remainingStations >= 0;
        this.mode = this.isValid ? ScoreMode.included : ScoreMode.excluded;
        this.points = remainingStations * 4;
        this.text = pluralise(remainingStations, 'remaining station');
    }
}

class ScoreRoute extends ScoreItem {
    _route;

    get route() { return this._route; }

    constructor(route) {
        super();
        this.points = route.points;
        this.text = route.cities.map(city => city.name).join(' - ');
        this._route = route;
        route.onChange.addHandler(e => this.refresh());

        route.cities.forEach(city => city.onChange.addHandler(e => this._refreshHighlight()))

        this.refresh();
    }

    _handleHover(value) {
        this.highlight = value;
        this._route.cities.forEach(city => city.highlight = value);
        return true;
    }

    refresh() {
        this.mode = this._route.isCompleted ? ScoreMode.included : ScoreMode.none;
    }

    _refreshHighlight() {
        this.highlight = this._route.cities.some(city => city.highlight);
    }
}

class ScoreLongRoute extends ScoreRoute {
    _longRoutes;

    get longRoutes() { return this._longRoutes; }
    set longRoutes(value) {
        this._longRoutes = value;
        this.refresh();
    }

    constructor(route) {
        super(route);
    }

    refresh() {
        if (!this._longRoutes) return;
        super.refresh();
        let foundBest = false;
        this._longRoutes.forEach(route => {
            if (route.mode === ScoreMode.none) return;
            route.mode = foundBest ? ScoreMode.excluded : ScoreMode.included;
            foundBest = true;
        });
    }
}

class ScoreTotal extends ScoreItem {
    constructor() {
        super();
        this.points = 0;
        this.text = 'Total score';
        this._trNode.classList.add('total');
    }
}

class ScoreTracks extends ScoreItem {
    _map;
    _tracks;

    constructor(map) {
        super();
        this._map = map;
        this._tracks = [...map._tracks].sort((a, b) => a.distance - b.distance);
        map.onChange.addHandler(e => this.refresh());
        this.refresh();
    }

    getRemainingTracks() {
        return this._tracks
            .filter(track => track.isBuilt === TrackBuilt.player)
            .map(track => track.distance)
            .reduce((a, b) => a - b, 45);
    }

    getSurplusTracks() {
        return this._map.networks
            .map(network => this._getSurplusTracksInNetwork(network))
            .reduce((a, b) => a + b, 0);
    }

    _getSurplusTracksInNetwork(network) {
        const cities = new Set();
        const tracks = [...network.tracks];
        let surplus = 0;

        let track = tracks.shift();
        track.cities.forEach(city => cities.add(city));

        while (tracks.length > 0) {
            track = tracks.removeFirst(track => track.cities.some(city => cities.has(city)));
            if (track.cities.every(city => cities.has(city))) {
                surplus += track.distance;
            } else {
                track.cities.forEach(city => cities.add(city));
            }
        }

        return surplus;
    }

    getTrackScore() {
        return this._tracks
            .filter(track => track.isBuilt === TrackBuilt.player)
            .map(track => TrackPoints[track.distance])
            .reduce((a, b) => a + b, 0);
    }

    refresh() {
        const remainingTracks = this.getRemainingTracks();
        const surplusTracks = this.getSurplusTracks();
        this.isValid = remainingTracks >= 0;
        this.mode = this.isValid ? ScoreMode.included : ScoreMode.excluded;
        this.points = this.getTrackScore();
        this.text = `Built tracks (${remainingTracks} remaining, ${surplusTracks} surplus)`;
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

    constructor(model) {
        if (!model.title) throw new Error('Missing "title"');

        this._title = model.title;
        this._map = new Map(model);
        this._summary = new ScoreCard(this._map);
        this._map.onchange = e => this._summary.refresh();

        this._divNode = document.createElement('div');
        this._divNode.classList.add('atlas');
        this._divNode.appendChild(this._summary.node);
        this._divNode.appendChild(this._map.node);
    }
}

let atlas;

async function loadAtlas() {
    const urlParams = new URLSearchParams(window.location.search);
    const filename = urlParams.get('atlas');
    if (!filename) return;

    const response = await fetch(`./${filename}.json`);
    if (!response.ok) throw Error(`Failed to get '${filename}.json'. Response status ${response.status} ${response.statusText}`);

    const json = await response.json();
    atlas = new Atlas(json);
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
