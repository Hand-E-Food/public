class Player {
    _name;
    _node;
    _score = 0;
    _scoreNode;
    _selected = false;
    onClick;

    get name() { return this._name; }

    get node() { return this._node; }

    get score() { return this._score; }
    set score(value) {
        this._score = value;
        this._scoreNode.nodeValue = this._score;
    }

    get selected() { return this._selected; }
    set selected(value) {
        if (this._selected)
            this._node.classList.remove('selected');
        this._selected = value;
        if (this._selected)
            this._node.classList.add('selected');
    }

    constructor(name, color) {
        this._name = name;

        this._node = document.createElement('div');
        this._node.classList.add('player');
        this._node.onclick = e => this.onClick ? this.onClick(e) : true;
        
        let nameSpanNode = document.createElement('span');
        nameSpanNode.appendChild(document.createTextNode(name))
        nameSpanNode.classList.add('playerName');
        nameSpanNode.style.color = color;
        this._node.appendChild(nameSpanNode);
        
        let scoreSpanNode = document.createElement('span');
        scoreSpanNode.classList.add('playerScore');
        this._scoreNode = document.createTextNode(this._score);
        scoreSpanNode.appendChild(this._scoreNode);
        this._node.appendChild(scoreSpanNode);
    }
}

class Players {
    _node;
    _players = [];
    _selectedPlayer = null;

    get node() { return this._node; }

    get selectedPlayer() { return this._selectedPlayer; }
    set selectedPlayer(player) {
        if (this._selectedPlayer)
            this._selectedPlayer.selected = false;
        this._selectedPlayer = player;
        if (this._selectedPlayer)
            this._selectedPlayer.selected = true;
    }

    constructor() {
        this._node = document.createElement('div');
        this._node.classList.add('players');
    }

    add(name, color) {
        let player = new Player(name, color);
        player.onClick = e => {
            this.selectedPlayer = player;
            return true;
        }
        this._node.appendChild(player.node);
        this._players.push(player);
        return player;
    }
}

class Button {
    _node;
    onClick;

    get node() { return this._node; }

    constructor(text) {
        this._node = document.createElement('div');
        this._node.classList.add('button');
        this._node.appendChild(document.createTextNode(text));
        this._node.onclick = e => {
            console.log(`Clicked ${text}`)
            this.onClick ? this.onClick(e) : true;
        }
    }
}

class PointsButton extends Button {
    _points;

    get points() { return this._points; }

    constructor(points) {
        let text = points >= 0
            ? '+' + points.toString()
            : points.toString();

        super(text);

        this._points = points;
    }
}

class UndoButton extends Button {
    constructor() {
        super('\u293e');
        this._node.classList.add('undo');
    }
}

class TextButton extends Button {
    constructor(text, width) {
        super(text);
        this._node.style.width = `${25 * width - 5}vw`;
    }
}

class Palette {
    onApplyPoints;
    _node;
    onUndo;
    _undoButton;

    get node() { return this._node; }

    constructor() {
        this._node = document.createElement('div');
        this._node.classList.add('palette');

        this._undoButton = new UndoButton();
        this._undoButton.onClick = e => this.onUndo ? this.onUndo() : true;
        this._node.appendChild(this._undoButton.node);
    }

    clearButtons() {
        while (this._node.firstChild) {
            this._node.lastChild.remove();
        }
    }

    addPointButton(points) {
        let pointsButton = new PointsButton(points);
        pointsButton.onClick = e => this.onApplyPoints ? this.onApplyPoints(points) : true;
        this._node.appendChild(pointsButton.node);
    }

    addTextButton(text, width, callback) {
        let textButton = new TextButton(text, width);
        textButton.onClick = e => callback();
        this._node.appendChild(textButton.node);
    }

    addUndoButton() {
        if (this._undoButton.node.parentNode)
            this._undoButton.node.remove();
        this._node.appendChild(this._undoButton.node);
    }
}

class UndoApplyPointsAction {
    _player;
    _points;

    constructor(player, points) {
        this._player = player;
        this._points = points;
    }

    undo(app) {
        this._player.score -= this._points;
        app.players.selectedPlayer = this._player;
    }
}

class UndoChangePaletteAction {
    _showPaletteCallback;

    constructor(showPaletteCallback) {
        this._showPaletteCallback = showPaletteCallback;
    }

    undo(app) {
        this._showPaletteCallback();
    }
}

class App {
    _node;
    palette;
    players;
    _undoQueue = [];
    
    get node() { return this._node; }

    constructor() {
        this.players = new Players();
        this.palette = new Palette();
        this.palette.onApplyPoints = points => this._applyPoints(points);
        this.palette.onUndo = () => this._undo();

        this._node = document.createElement('div');
        this._node.classList.add('app');
        this._node.appendChild(this.players.node);
        this._node.appendChild(this.palette.node);

        this._showTrackPalette();
    }

    _applyPoints(points) {
        let player = this.players.selectedPlayer;
        console.log(`Giving ${player.name} ${points} points.`);
        if (!player) return;
        player.score += points;
        this._did(new UndoApplyPointsAction(player, points));
        return true;
    }

    _did(undoAction) {
        this._undoQueue.push(undoAction);
    }

    _showTrackPalette() {
        this.palette.clearButtons();
        this.palette.addPointButton(1);
        this.palette.addPointButton(2);
        this.palette.addPointButton(4);
        this.palette.addPointButton(7);
        this.palette.addPointButton(11);
        this.palette.addPointButton(15);
        this.palette.addPointButton(18);
        this.palette.addPointButton(21);
        this.palette.addTextButton('Routes', 2, () => this._showRoutePalette());
        this.palette.addUndoButton();
    }

    _showRoutePalette() {
        this.palette.clearButtons();
        this.palette.addPointButton(5);
        this.palette.addPointButton(6);
        this.palette.addPointButton(7);
        this.palette.addPointButton(8);
        this.palette.addPointButton(9);
        this.palette.addPointButton(10);
        this.palette.addPointButton(11);
        this.palette.addPointButton(12);
        this.palette.addPointButton(13);
        this.palette.addPointButton(20);
        this.palette.addPointButton(21);
        this.palette.addUndoButton();
        this._did(new UndoChangePaletteAction(() => this._showTrackPalette()));
    }

    _undo() {
        let action = this._undoQueue.pop();
        if (action)
            action.undo(this);
        return true;
    }
}

let app;

window.onload = () => {
    window.onload = undefined;
    app = new App();
    app.players.add('Black', 'gray');
    app.players.add('Blue', 'blue');
    app.players.add('Green', 'limegreen');
    app.players.add('Yellow', 'yellow');
    app.players.add('Red', 'red');
    document.body.replaceChildren(app.node);
}
