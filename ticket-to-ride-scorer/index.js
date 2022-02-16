class Player {
    _node;
    _score = 0;
    _scoreNode;
    _selected = false;
    onClick;

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

    constructor(color) {
        this._node = document.createElement('div');
        this._node.classList.add('player');
        this._node.style.backgroundColor = color;
        this._node.style.color = ['limegreen', 'yellow'].indexOf(color) >= 0
            ? 'black'
            : 'white';
        this._node.onclick = e => {
            if (this.onClick) this.onClick(e);
            e.cancelBubble = true;
            return true;
        }
        
        this._scoreNode = document.createTextNode(this._score);
        this._node.appendChild(this._scoreNode);
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

    add(color) {
        let player = new Player(color);
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
            if (this.onClick) this.onClick(e);
            e.cancelBubble = true;
            return true;
        }
    }
}

class TextButton extends Button {
    constructor(text, width) {
        super(text);
        this._node.style.width = `calc(var(--v) * (${width} - 0.20) / 4)`;
    }
}

class Palette {
    _node;
    onApplyPoints;
    onUndo;
    _undoButton;

    get node() { return this._node; }

    constructor() {
        this._node = document.createElement('div');
        this._node.classList.add('palette');

        this._undoButton = new Button('\u293e');
        this._undoButton.onClick = e => this.onUndo ? this.onUndo() : true;
        this._node.appendChild(this._undoButton.node);
    }

    clearButtons() {
        while (this._node.firstChild) {
            this._node.lastChild.remove();
        }
    }

    addPointButton(text, points) {
        let button = new Button(text);
        button.onClick = e => this.onApplyPoints ? this.onApplyPoints(points) : true;
        this._node.appendChild(button.node);
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
        this._node.onclick = e => {
            this.players.selectedPlayer = null;
            e.cancelBubble = true;
            return true;
        }

        this._showTrackPalette();
    }

    _applyPoints(points) {
        let player = this.players.selectedPlayer;
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
        this.palette.addPointButton('1🚃',  1);
        this.palette.addPointButton('2🚃',  2);
        this.palette.addPointButton('3🚃',  4);
        this.palette.addPointButton('4🚃',  7);
        this.palette.addPointButton('5🚃', 11);
        this.palette.addPointButton('6🚃', 15);
        this.palette.addPointButton('7🚃', 18);
        this.palette.addPointButton('8🚃', 21);
        this.palette.addPointButton('🏫' ,  4);
        this.palette.addTextButton('Routes', 2, () => this._showRoutePalette());
        this.palette.addUndoButton();
    }

    _showRoutePalette() {
        this.palette.clearButtons();
        this.palette.addPointButton( '5',  5);
        this.palette.addPointButton( '6',  6);
        this.palette.addPointButton( '7',  7);
        this.palette.addPointButton( '8',  8);
        this.palette.addPointButton( '9',  9);
        this.palette.addPointButton('10', 10);
        this.palette.addPointButton('11', 11);
        this.palette.addPointButton('12', 12);
        this.palette.addPointButton('13', 13);
        this.palette.addPointButton('20', 20);
        this.palette.addPointButton('21', 21);
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
    let players = ['black', 'blue', 'limegreen', 'yellow', 'red'];
    while (players.length > 0) {
        let i = Math.floor(Math.random() * players.length);
        let player = players.splice(i, 1)[0];
        app.players.add(player);
    }
    document.body.replaceChildren(app.node);
}
