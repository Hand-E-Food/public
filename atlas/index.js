const NS = {
    SVG: 'http://www.w3.org/2000/svg',
}; 

class LocationList {
    _location = null;
    _locations = [];
    _selectNode;

    get location() { return this._location; }
    onLocationChanged;

    get locations() { return this._locations; }
    set locations(locations) {
        if (!locations) {
            locations = [];
        }
        if (this._locations == locations) {
            return;
        }

        const selectNode = this._selectNode;
        selectNode.value = null;
        while (selectNode.firstChild) {
            selectNode.lastChild.remove();
        }

        this._locations = locations;

        const optionNode = document.createElement('option');
        optionNode.selected = true;
        optionNode.value = -1;
        selectNode.appendChild(optionNode);

        const textNode = document.createTextNode('- - -');
        optionNode.appendChild(textNode);

        locations.forEach((location, i) => {
            let optionNode = document.createElement('option');
            optionNode.classList.add('location');
            optionNode.value = i;
            selectNode.appendChild(optionNode);

            let textNode = document.createTextNode(location.name);
            optionNode.appendChild(textNode);
        });
    }

    get node() { return this._selectNode; }

    constructor(locations) {
        const selectNode = document.createElement('select');
        selectNode.classList.add('locations');
        selectNode.size = 2;
        selectNode.onchange = e => this._selectNode_onchange(e);
        this._selectNode = selectNode;

        this.locations = locations;
    }

    _selectNode_onchange(e) {
        const index = this._selectNode.value ? parseInt(this._selectNode.value) : -1;
        
        this._location = index >= 0 ? this._locations[index] : null;

        return this.onLocationChanged
            ? this.onLocationChanged({
                location: this._location,
                target: this,
            })
            : true;
    }
}

class Map {
    _divNode;
    _lineHNode;
    _lineVNode;
    _svgNode;

    get node() { return this._divNode; }

    constructor(image, width, height) {
        const divNode = document.createElement('div');
        divNode.classList.add('map');
        this._divNode = divNode;

        const svgNode = document.createElementNS(NS.SVG, 'svg');
        svgNode.setAttribute('stroke-width', Math.min(width, height) / 100)
        svgNode.setAttribute('viewBox', `0 0 ${width} ${height}`);
        divNode.appendChild(svgNode);
        this._svgNode = svgNode;

        const imageNode = document.createElementNS(NS.SVG, 'image');
        imageNode.setAttribute('href', image);
        imageNode.setAttribute('height', height);
        imageNode.setAttribute('width', width);
        svgNode.appendChild(imageNode);

        const lineHNode = document.createElementNS(NS.SVG, 'line');
        lineHNode.setAttribute('x1', 0);
        lineHNode.setAttribute('x2', width);
        svgNode.appendChild(lineHNode);
        this._lineHNode = lineHNode;

        const lineVNode = document.createElementNS(NS.SVG, 'line');
        lineVNode.setAttribute('y1', 0);
        lineVNode.setAttribute('y2', height);
        svgNode.appendChild(lineVNode);
        this._lineVNode = lineVNode;

        this.clearTarget();
    }

    clearTarget() {
        this._svgNode.setAttribute('stroke', 'transparent');
        this._lineVNode.setAttribute('x1', 0);
        this._lineVNode.setAttribute('x2', 0);
        this._lineHNode.setAttribute('y1', 0);
        this._lineHNode.setAttribute('y2', 0);
    }

    setTarget(x, y) {
        this._lineVNode.setAttribute('x1', x);
        this._lineVNode.setAttribute('x2', x);
        this._lineHNode.setAttribute('y1', y);
        this._lineHNode.setAttribute('y2', y);
        this._svgNode.setAttribute('stroke', 'red');
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
        
        const locationList = new LocationList(data.locations);
        locationList.onLocationChanged = e => this._locationList_onLocationChanged(e);
        divNode.appendChild(locationList.node);
        this._locationList = locationList;

        const map = new Map(data.image, data.width, data.height);
        divNode.appendChild(map.node);
        this._map = map;
    }

    _locationList_onLocationChanged(e) {
        if (e.location) {
            this._map.setTarget(e.location.x, e.location.y);
        } else {
            this._map.clearTarget();
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
