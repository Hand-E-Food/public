class LocationList {
    _location = null;
    _locations;
    _selectNode;

    onLocationChanged;

    get location() { return this._location; }

    get node() { return this._selectNode; }

    constructor(locations) {
        let selectNode = document.createElement('select');
        selectNode.classList.add('locations');
        selectNode.size = 2;
        selectNode.onchange = e => this._selectNode_onchange(e);
        this._selectNode = selectNode;

        this._locations = locations;
        locations.forEach((location, i) => {
            let optionNode = document.createElement('option');
            optionNode.classList.add('location');
            optionNode.value = i;
            this._node = optionNode;
            selectNode.appendChild(optionNode);

            let textNode = document.createTextNode(location.name);
            optionNode.appendChild(textNode);
        });
    }

    _selectNode_onchange(e) {
        this._location = this._selectNode.value
            ? this._locations[parseInt(this._selectNode.value)]
            : null;

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
    _height;
    _lineHNode;
    _lineVNode;
    _scale;
    _width;

    get node() { return this._divNode; }

    constructor(image, width, height) {
        this._width = width;
        this._height = height;
        const strokeWidth = Math.min(height, width) / 100;

        let divNode = document.createElement('div');
        divNode.classList.add('map');
        this._divNode = divNode;

        let svgNode = document.createElementNS('http://www.w3.org/2000/svg', 'svg');
        svgNode.classList.add('map');
        svgNode.setAttribute('viewBox', `0 0 ${width} ${height}`);
        divNode.appendChild(svgNode);

        let imageNode = document.createElementNS('http://www.w3.org/2000/svg', 'image');
        imageNode.setAttribute('href', image);
        imageNode.setAttribute('height', height);
        imageNode.setAttribute('width', width);
        svgNode.appendChild(imageNode);

        let lineHNode = document.createElementNS('http://www.w3.org/2000/svg', 'line');
        lineHNode.setAttribute('stroke', 'red');
        lineHNode.setAttribute('stroke-width', strokeWidth)
        lineHNode.setAttribute('x1', 0);
        lineHNode.setAttribute('x2', width);
        svgNode.appendChild(lineHNode);
        this._lineHNode = lineHNode;

        let lineVNode = document.createElementNS('http://www.w3.org/2000/svg', 'line');
        lineVNode.setAttribute('stroke', 'red');
        lineVNode.setAttribute('stroke-width', strokeWidth)
        lineVNode.setAttribute('y1', 0);
        lineVNode.setAttribute('y2', height);
        svgNode.appendChild(lineVNode);
        this._lineVNode = lineVNode;

        this.clearTarget();
    }

    clearTarget() {
        this.setTarget(this._width / 2, this._height / 2);
    }

    setTarget(x, y) {
        this._lineVNode.setAttribute('x1', x);
        this._lineVNode.setAttribute('x2', x);
        this._lineHNode.setAttribute('y1', y);
        this._lineHNode.setAttribute('y2', y);
    }
}

class Atlas {
    _divNode;
    _locationList;
    _map;

    get node() { return this._divNode; }

    constructor(data) {
        let divNode = document.createElement('div');
        divNode.classList.add('atlas');
        this._divNode = divNode;
        
        let locationList = new LocationList(data.locations);
        locationList.onLocationChanged = e => this._locationList_onLocationChanged(e);
        divNode.appendChild(locationList.node);
        this._locationList = locationList;

        let map = new Map(data.image, data.width, data.height);
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

window.onload = () => {
    window.onload = undefined;
    const urlParams = new URLSearchParams(window.location.search);
    const filename = urlParams.get('map');

    fetch(`./${filename}.json`)
        .then(response => {
            if (response.ok) {
                response.json().then(json => {
                    atlas = new Atlas(json);
                    document.body.replaceChildren(atlas.node);
                });
            } else {
                console.error(response);
                document.body.replaceChildren(response.statusText);
            }
        });
}