const NS = {
    SVG: 'http://www.w3.org/2000/svg',
}; 

class Location {
    _divNode;
    _location;
    _spanNode;

    get location() { return this._location; }
    set location(value) {
        if (this._location === value) { return; }
        this._location = value;
        this._spanNode.textContent = value.name;
    }

    get node() { return this._divNode; }

    get selected() { return this._divNode.classList.contains('selected'); }
    set selected(value) {
        if (this.selected != value) {
            if (value) {
                this._divNode.classList.add('selected');
            } else {
                this._divNode.classList.remove('selected');
            }
        }
        if (this.onSelectedChanged) {
            this.onSelectedChanged({
                location: this._location,
                selected: value,
                target: this,
            });
        }
    }
    onSelectedChanged;

    constructor() {
        const divNode = document.createElement('div');
        divNode.classList.add('location');
        divNode.onclick = e => this._divNode_onclick(e);
        this._divNode = divNode;

        const spanNode = document.createElement('span');
        divNode.appendChild(spanNode);
        this._spanNode = spanNode;
    }

    _divNode_onclick(e) {
        this.selected = !this.selected;
        return true;
    }
}

class LocationList {
    _divNode;
    _locations = [];

    onSelectedLocationsChanged;

    get locations() { return this._locations.map(node => node.location); }
    set locations(value) {
        this._locations.forEach(location => {
            delete location.onSelectedChanged;
            location.node.remove();
        });
        this._locations = [];

        if (!value) { return; }

        value.forEach(item => {
            const location = new Location();
            location.location = item;
            location.onSelectedChanged = e => this._location_onSelectedChanged(e);
            const locationNode = location.node;
            this._divNode.appendChild(locationNode);
            this._locations.push(locationNode);
        });
    }

    get node() { return this._divNode; }

    constructor() {
        const divNode = document.createElement('div');
        divNode.classList.add('locations');
        this._divNode = divNode;
    }

    _location_onSelectedChanged(e) {
        if (!this.onSelectedLocationsChanged) { return; }
        this.onSelectedLocationsChanged(e);
    }
}

class Map {
    _divNode;
    _scale;
    _svgNode;
    _targets = {};

    get node() { return this._divNode; }

    constructor(image, width, height) {
        this._scale = Math.min(width, height) / 100;

        const divNode = document.createElement('div');
        divNode.classList.add('map');
        this._divNode = divNode;

        const svgNode = document.createElementNS(NS.SVG, 'svg');
        svgNode.setAttribute('fill', 'transparent');
        svgNode.setAttribute('stroke', 'red');
        svgNode.setAttribute('stroke-width', this._scale)
        svgNode.setAttribute('viewBox', `0 0 ${width} ${height}`);
        divNode.appendChild(svgNode);
        this._svgNode = svgNode;

        const imageNode = document.createElementNS(NS.SVG, 'image');
        imageNode.setAttribute('href', image);
        imageNode.setAttribute('height', height);
        imageNode.setAttribute('width', width);
        svgNode.appendChild(imageNode);

        this.clearTargets();
    }

    addTarget(x, y) {
        const key = `${x},${y}`;
        if (this._targets[key]) {
            return;
        }

        const circleNode = document.createElementNS(NS.SVG, 'circle');
        circleNode.setAttribute('cx', x);
        circleNode.setAttribute('cy', y);
        this._svgNode.appendChild(circleNode);

        const animateNode = document.createElementNS(NS.SVG, 'animate');
        animateNode.setAttribute('attributeName', 'r');
        animateNode.setAttribute('calcMode', 'spline');
        animateNode.setAttribute('dur', '2s');
        animateNode.setAttribute('keyTimes', '0;0.5;1');
        animateNode.setAttribute('keySplines', '0.5 0 0.5 1;0.5 0 0.5 1');
        animateNode.setAttribute('repeatCount', 'indefinite');
        animateNode.setAttribute('values', `0;${this._scale * 5};0`);
        circleNode.appendChild(animateNode);

        this._targets[key] = [ circleNode ];
    }

    clearTargets() {
        Object.values(this._targets).forEach(nodes =>
            nodes.forEach(node =>
                node.remove()
            )
        );
        this._targets = {};
    }

    removeTarget(x, y) {
        const key = `${x},${y}`;
        const nodes = this._targets[key];
        if (!nodes) {
            return;
        }

        nodes.forEach(node =>
            node.remove()
        );
        delete this._targets[key];
    }
}

class Atlas {
    _divNode;
    _locationList;
    _map;
    _title;

    get node() { return this._divNode; }

    get title() { return this._title; }

    constructor(data) {
        this._title = data.title;

        const divNode = document.createElement('div');
        divNode.classList.add('atlas');
        this._divNode = divNode;
        
        const locationList = new LocationList();
        locationList.locations = data.locations;
        locationList.onSelectedLocationsChanged = e => this._locationList_onSelectedLocationsChanged(e);
        divNode.appendChild(locationList.node);
        this._locationList = locationList;

        const map = new Map(data.image, data.width, data.height);
        divNode.appendChild(map.node);
        this._map = map;
    }

    _locationList_onSelectedLocationsChanged(e) {
        if (e.location) {
            if (e.selected) {
                this._map.addTarget(e.location.x, e.location.y);
            } else {
                this._map.removeTarget(e.location.x, e.location.y);
            }
        }
    }
}

let atlas;

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

    const json = await response.json()
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
