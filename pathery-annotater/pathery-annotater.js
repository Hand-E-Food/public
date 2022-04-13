/* eslint no-multi-spaces: off */
/* global clearwalls: true,
          grid_click: true,
          mapdata: false,
          solution: false,
*/

/** Indicates how to treat a tile.
 * @readonly
 * @enum {number}
 */
const PathType = {
  Inaccessible: 0,
  Terminus: 1,
  Start: 2,
  Finish: 3,
  Pathable: 4,
  Ice: 5,
};

/** Used to populate `Rules`.
 * @returns {{PathType: Array<Array<{x: number, y: number, type: PathType}>>}} The rules for each
 * PathType.
 */
function createRules() {

  /** Generate a simple object describing the PathType expected at a tile offset.
   * @param x {number} The X offset.
   * @param y {number} The Y offset.
   * @param type {PathType} The expected PathType. Default: `PathType.Inaccessible`
   * @returns {{x: number, y: number, type: PathType}}
   */
  function d(x, y, type = PathType.Inaccessible) { return {x, y, type}; }

  /** Keep this rule in one orientation. */
  function x1(rule) {
    return [ rule ];
  }

  /** Mirror this rule into two orientations. */
  function x2(rule) {
    return [
      rule.map(cell => d(cell.x, cell.y, cell.type)),
      rule.map(cell => d(cell.y, cell.x, cell.type)),
    ];
  }

  /** Rotate this rule into four orientations. */
  function x4(rule) {
    return [
      rule.map(cell => d( cell.x,  cell.y, cell.type)),
      rule.map(cell => d(-cell.y,  cell.x, cell.type)),
      rule.map(cell => d(-cell.x, -cell.y, cell.type)),
      rule.map(cell => d( cell.y, -cell.x, cell.type)),
    ];
  }

  /** Mirror and rotate this rule into eight orientations. */
  function x8(rule) {
    return x2(rule).map(x4).flat();
  }

  return {
    [PathType.Pathable]: [ // Block if a dead end, one or two tiles wide.
      x4([ d(-1,  0), d( 0, -1), d(+1,  0) ]), // Blocked on three sides
      x8([ d(-1,  0), d( 0, -1), d(+1, -1), d(+2,  0), d(+1,  0, PathType.Pathable) ]), // two adjacent pathables blocked on three sides
      x8([ d(-1,  0), d( 0, -1), d(+1, -1), d(+2,  0), d(+1,  0, PathType.Start   ), d(+1, +1, PathType.Start ) ]), // two adjacent pathables blocked on three sides
      x8([ d(-1,  0), d( 0, -1), d(+1, -1), d(+2,  0), d(+1,  0, PathType.Finish  ), d(+1, +1, PathType.Finish) ]), // two adjacent pathables blocked on three sides
    ].flat(),

    [PathType.Ice]: [
      x4([ d(-1,  0), d( 0, -1) ]) // Blocked on adjacent sides
    ].flat(),

    [PathType.Start]: [
      x1([ d(-1,  0), d( 0, -1), d(+1,  0), d( 0, +1) ]), // Blocked on four sides
      x4([ d(-1,  0), d( 0, -1), d(+1,  0), d( 0, +1, PathType.Start) ]), // Blocked on three sides and open side is also start
    ].flat(),

    [PathType.Finish]: [
      x1([ d(-1,  0), d( 0, -1), d(+1,  0), d( 0, +1) ]), // Blocked on four sides
      x4([ d(-1,  0), d( 0, -1), d(+1,  0), d( 0, +1, PathType.Finish) ]), // Blocked on three sides and open side is also finish
    ].flat(),
  };
}

/** The rules that can cause a particular tile to be inaccessible.
 * @readonly
 * @type {{PathType: Array<Array<{x: number, y: number, type: PathType}>>}}
 */
const Rules = createRules();

/** Annotates a map. */
class Annotater {

  /** Initialises an Annotater for the specified map ID.
   * @param id {number} The map ID.
   */
  constructor(id) {
    this.id = id;
    const mapdata = this.mapdata;
    this.tiles = mapdata.tiles;
    this.width = mapdata.width;
    this.height = mapdata.height;

    this.path = new Array(this.height);
    for(let y = 0; y < this.height; y++) {
      this.path[y] = new Array(this.width);
    }
  }

  /** Gets this annotater's map data.
   * @returns {{
   *   ID: number,
   *   checkpoints: number,
   *   code: string,
   *   containsNormalStart: boolean,
   *   containsReverseStart: boolean,
   *   dateCreated: string,
   *   dateExpires: number,
   *   flags: string,
   *   height: number,
   *   isBlind: boolean,
   *   loaded: boolean,
   *   mapid: number,
   *   name: string,
   *   normalStartLocations: Array<string>,
   *   oldCode: null,
   *   reverseStartLocations: Array<string>,
   *   savedSolution: string,
   *   teleports: number,
   *   tiles: Array<Array<Array<string, number>>>,
   *   usedWallCount: string,
   *   walls: string,
   *   width: number,
   * }}
   */
  get mapdata() { return mapdata[this.id]; }

  /** Replaces the map annotation. */
  annotate() {
    this._initialisePath();
    this._annotatePath();
  }

  /** Resets the analysis of the map before finding details to annotate. */
  _initialisePath() {
    for(let y = 0; y < this.height; y++) {
      const pathRow = this.path[y];
      for(let x = 0; x < this.width; x++) {
        pathRow[x] = this._getPathType(this.tiles[y][x][0]);
        this._annotateCell(x, y, false);
      }
    }

    this.solution
      .split(".")
      .filter(cell => cell.length > 0)
      .map(cell => cell.split(","))
      .forEach(cell => this._setPath(cell[1], cell[0], PathType.Inaccessible), this);
  }

  /** Determines how a given map cell should be treated.
   * @param value {string} The map cell's type as a single letter.
   * @returns {PathType} A PathType indicating how to treat this cell.
  */
  _getPathType(value) {
    switch (value) {
      case "c": return PathType.Terminus;     // checkpoint
      case "f": return PathType.Finish;       // finish
      case "o": return PathType.Pathable;     // open
      case "p": return PathType.Pathable;     // pathable only
      case "r": return PathType.Inaccessible; // rock
      case "s": return PathType.Start;        // green/red start
      case "t": return PathType.Pathable;     // teleport in
      case "u": return PathType.Terminus;     // teleport out
      case "x": return PathType.Pathable;     // green/red pathable/rock
      case "z": return PathType.Ice;          // ice
      default:  return PathType.Pathable;
    }
  }

  /** Gets the current solution drawn on this annotater's map.
   * @returns {string} The current solution encoded as `{x},{y}` coordinate pairs separated with periods (`.`).
   * @example ".1,2.3,4.5,6."
   */
  get solution() { return solution[this.id]; }

  /** Finds and annotates map cells that cannot be accessed. */
  _annotatePath() {
    this.xyToAssess = [];
    for (let y = 0; y < this.height; y++) {
      for (let x = 0; x < this.width; x++) {
        this.xyToAssess.push({x, y});
      }
    }

    while (this.xyToAssess.length > 0) {
      const cell = this.xyToAssess.pop();
      const x = cell.x;
      const y = cell.y;

      if (this._shouldBlockTile(x, y)) {
        this.xyToAssess.push(
          {x: x-1, y: y-1},
          {x: x  , y: y-1},
          {x: x+1, y: y-1},
          {x: x-1, y: y  },
          {x: x+1, y: y  },
          {x: x-1, y: y+1},
          {x: x  , y: y+1},
          {x: x+1, y: y+1},
        );
        this._setPath(x, y, PathType.Inaccessible);
        this._annotateCell(x, y, true);
      }
    }
  }

  /** Checks if a map cell should be inaccessible.
   * @param x {number} The cell's X coordinate.
   * @param y {number} The cell's Y coordinate.
   */
  _shouldBlockTile(x, y) {
    const rules = Rules[this._getPath(x, y)];
    if (!rules) return false;

    return rules
      .some(rule =>
        rule.every(cell =>
          this._getPath(x + cell.x, y + cell.y) === cell.type
        ,this)
      ,this);
  }

  /** Annotates the specified cell.
   * @param x {number} The cell's X coordinate.
   * @param y {number} The cell's Y coordinate.
   * @param value {boolean} `true` to annotate the cell as inaccessible. `false` to clear the annotation.
   */
  _annotateCell(x, y, value) {
    this.getHTMLElement(x, y).firstElementChild.style.backgroundColor = value ? "#ff000088" : null;
  }

  /** Gets the HTML element for the specified cell.
   * @param x {number} The cell's X coordinate.
   * @param y {number} The cell's Y coordinate.
   * @returns {HTMLElement} The HTML element for the specified cell.
   */
  getHTMLElement(x, y) {
    return document.getElementById(`${this.id},${y},${x}`);
  }

  /** Gets the path type applied to the specified cell.
   * @param x {number} The cell's X coordinate.
   * @param y {number} The cell's Y coordinate.
   * @returns {PathType} The PathType for the specified cell.
   */
  _getPath(x, y) { return (x >= 0 && x < this.width && y >= 0 && y < this.height) ? this.path[y][x] : PathType.Inaccessible; }

  /** Sets the path type applied to the specified cell.
   * @param x {number} The cell's X coordinate.
   * @param y {number} The cell's Y coordinate.
   * @param value {PathType} The PathType for the specified cell.
   */
  _setPath(x, y, value) { this.path[y][x] = value; }
}

var annotaters = {};
mapdata
  .filter(map => map)
  .forEach(map => {
    const annotater = new Annotater(map.ID);
    annotaters[map.ID] = annotater;
    annotater.annotate();
  }
);

var super_clearwalls = clearwalls;
clearwalls = function(mapid) {
  super_clearwalls(mapid);
  annotaters[mapid].annotate();
}

var super_grid_click = grid_click;
grid_click = function(obj) {
  super_grid_click(obj);
  const tmp = obj.id.split(',');
  const mapid = tmp[0] - 0;
  annotaters[mapid].annotate();
}
