// ==UserScript==
// @name         Pathery Annotater
// @namespace    http://tampermonkey.net/
// @version      1.0
// @description  Annotates Pathery maps.
// @author       Mark Richardson
// @match        https://www.pathery.com/
// @icon         https://www.google.com/s2/favicons?sz=64&domain=pathery.com
// @grant        none
// ==/UserScript==

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
const CellType = {
  /** Indicate that the tile is inaccessible. */
  Inaccessible: 0,
  /** Indicate that the tile is pathable. */
  Pathable: 1,
  /** Indicate that the tile is a terminus and the path might extend in only one direction.
   * A terminus can include a checkpoint or teleport out.
   */
  Terminus: 2,
  /** Indicate that the tile is a pathable start tile. */
  Start: 3,
  /** Indicate that the tile is a pathable finish tile. */
  Finish: 4,
  /** Indicate that the tile is pathable ice. */
  Ice: 5,
};

/** Indicates how to annotate a tile.
 * @readonly
 * @enum {string}
 */
const AnnotationType = {
  /** Remove the annotation.
   * @type string
   */
  Clear: null,
  /** Annotate that the cell is inaccessible. */
  Inaccessible: "#ff000088",
  /** Annotate that the cell is a bottleneck. */
  Bottleneck: "#00ffff88",
}

/** Manages the state of the map being analysed. */
class PatheryMap {

  /** This map source data.
   * @type {{
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
  _mapdata;

  /** This map's width.
   * @type {number}
   */
  width;

  /** This map's height.
   * @type {number}
   */
  height;

  /** The information derived for this map.
   * @type {Array<Array<CellType>>}
  */
  _cells;

  /** The locations each path can start at. Always contains two elements: a list of the normal
   * start locations; and a list of the reverse start locations.
   * @type {Array<Array<{x: number, y: number}>>}
   */
  starts = [[],[]];

  /** The finish locations.
   * @type {Array<{x: number, y: number}>}
   */
  finishes = [];

  /** The locations of each checkpoint. The final list represents the finish.
   * @type {Array<Array<{x: number, y: number}>>}
   */
  checkpoints;

  /** The in and out location of each teleport.
   * @type {Array<{in: {x: number, y: number}, out: {x: number, y: number}}>}
   */
  teleports;

  /** Initialises a Map for the specified map ID.
   * @param {number} id The map ID.
   */
  constructor(id) {
    this._mapdata = mapdata[id];
    this.width = this._mapdata.width;
    this.height = this._mapdata.height;

    this._cells = new Array(this.height);
    for(let y = 0; y < this.height; y++) {
      this._cells[y] = new Array(this.width);
    }

    this.checkpoints = new Array(this._mapdata.checkpoints);
    for(let i = 0; i < this.checkpoints.length; i++) {
      this.checkpoints[i] = [];
    }

    this.teleports = new Array(this._mapdata.teleports);
    for(let i = 0; i < this.teleports.length; i++) {
      this.teleports[i] = { in: null, out: null };
    }

    for(let y = 0; y < this.height; y++) {
      const tilesRow = this._mapdata.tiles[y];
      for(let x = 0; x < this.width; x++) {
        const tilesCell = tilesRow[x];
        const n = tilesCell[1] - 1;
        switch (tilesCell[0]) {
          case "c": this.checkpoints[n].push({ x, y }); break; // checkpoint
          case "f": this.finishes.push({ x, y });       break; // finish
          case "s": this.starts[n].push({ x, y });      break; // start
          case "t": this.teleports[n].in = { x, y };    break; // teleport in
          case "u": this.teleports[n].out = { x, y };   break; // teleport out
        }
      }
    }
  }

  /** Resets the path type of every cell. */
  reset() {
    const tiles = this._mapdata.tiles;
    for(let y = 0; y < this.height; y++) {
      const pathRow = this._cells[y];
      const tilesRow = tiles[y];
      for(let x = 0; x < this.width; x++) {
        pathRow[x] = this._getCellType(tilesRow[x][0]);
      }
    }
  }

  /** Determines how a given map cell should be treated.
   * @param {string} value The map cell's type as a single letter.
   * @returns {CellType} A CellType indicating how to treat this cell.
  */
  _getCellType(value) {
    switch (value) {
      case "c": return CellType.Terminus;     // checkpoint
      case "f": return CellType.Finish;       // finish
      case "o": return CellType.Pathable;     // open
      case "p": return CellType.Pathable;     // pathable only
      case "r": return CellType.Inaccessible; // rock
      case "s": return CellType.Start;        // green/red start
      case "t": return CellType.Pathable;     // teleport in
      case "u": return CellType.Terminus;     // teleport out
      case "x": return CellType.Pathable;     // green/red pathable/rock
      case "z": return CellType.Ice;          // ice
      default:  return CellType.Pathable;
    }
  }

  /** Gets the path type applied to the specified cell.
   * @param {number} x The cell's X coordinate.
   * @param {number} y The cell's Y coordinate.
   * @returns {CellType} The CellType for the specified cell.
   */
  get(x, y) { return (x >= 0 && x < this.width && y >= 0 && y < this.height) ? this._cells[y][x] : CellType.Inaccessible; }

  /** Sets the path type applied to the specified cell.
   * @param {number} x The cell's X coordinate.
   * @param {number} y The cell's Y coordinate.
   * @param {CellType} value The CellType for the specified cell.
   */
  set(x, y, value) { this._cells[y][x] = value; }
}

/** Annotates a map. */
class PatheryAnnotater {

  /** The rules that can cause a particular tile to be inaccessible.
   * @static @readonly
   * @type {{CellType: Array<Array<{x: number, y: number, type: CellType}>>}}
   */
  static InaccessibleRules = function() {

    /** Generate a simple object describing the CellType expected at a tile offset.
     * @param {number} x The X offset.
     * @param {number} y The Y offset.
     * @param {CellType} type The expected CellType. Default: `CellType.Inaccessible`
     * @returns {{x: number, y: number, type: CellType}}
     */
    function d(x, y, type = CellType.Inaccessible) { return {x, y, type}; }

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
      [CellType.Pathable]: [ // Block if a dead end, one or two tiles wide.
        x4([ d(-1,  0), d( 0, -1), d(+1,  0) ]), // Blocked on three sides
        x8([ d(-1,  0), d( 0, -1), d(+1, -1), d(+2,  0), d(+1,  0, CellType.Pathable) ]), // two adjacent pathables blocked on three sides
        x8([ d(-1,  0), d( 0, -1), d(+1, -1), d(+2,  0), d(+1,  0, CellType.Start   ), d(+1, +1, CellType.Start ) ]), // two adjacent pathables blocked on three sides
        x8([ d(-1,  0), d( 0, -1), d(+1, -1), d(+2,  0), d(+1,  0, CellType.Finish  ), d(+1, +1, CellType.Finish) ]), // two adjacent pathables blocked on three sides
      ].flat(),

      [CellType.Ice]: [
        x4([ d(-1,  0), d( 0, -1) ]) // Blocked on adjacent sides
      ].flat(),

      [CellType.Start]: [
        x1([ d(-1,  0), d( 0, -1), d(+1,  0), d( 0, +1) ]), // Blocked on four sides
        x4([ d(-1,  0), d( 0, -1), d(+1,  0), d( 0, +1, CellType.Start) ]), // Blocked on three sides and open side is also start
      ].flat(),

      [CellType.Finish]: [
        x1([ d(-1,  0), d( 0, -1), d(+1,  0), d( 0, +1) ]), // Blocked on four sides
        x4([ d(-1,  0), d( 0, -1), d(+1,  0), d( 0, +1, CellType.Finish) ]), // Blocked on three sides and open side is also finish
      ].flat(),
    };
  }();

  /** The map ID service by this annotater.
   * @type {number}
   */
  id;

  /** The map holding the information used by this annotater.
   * @type {PatheryMap}
   */
  map;

  /** Initialises an Annotater for the specified map ID.
   * @param {number} id The map ID.
   */
  constructor(id) {
    this.id = id;
    this.map = new PatheryMap(id);
  }

  /** Replaces the map annotation. */
  annotate() {
    this._reset();
    this._annotateInaccessibleCells();
  }

  /** Resets the analysis of the map before finding details to annotate. */
  _reset() {
    this.map.reset();
    
    for(let y = 0; y < this.height; y++) {
      for(let x = 0; x < this.width; x++) {
        this._annotateCell(x, y, AnnotationType.Clear);
      }
    }

    this.solution
      .split(".")
      .filter(cell => cell.length > 0)
      .map(cell => cell.split(","))
      .forEach(cell => this.map.set(cell[1], cell[0], CellType.Inaccessible), this);
  }

  /** Gets the current solution drawn on this annotater's map.
   * @returns {string} The current solution encoded as `{x},{y}` coordinate pairs separated with periods (`.`).
   * @example ".1,2.3,4.5,6."
   */
  get solution() { return solution[this.id]; }

  /** Finds and annotates map cells that cannot be accessed. */
  _annotateInaccessibleCells() {
    this.xyToAssess = [];
    for (let y = 0; y < this.map.height; y++) {
      for (let x = 0; x < this.map.width; x++) {
        this.xyToAssess.push({x, y});
      }
    }

    while (this.xyToAssess.length > 0) {
      const cell = this.xyToAssess.pop();
      const x = cell.x;
      const y = cell.y;

      if (this._isInaccessible(x, y)) {
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
        this.map.set(x, y, CellType.Inaccessible);
        this._annotateCell(x, y, AnnotationType.Inaccessible);
      }
    }
  }

  /** Checks if a map cell should be inaccessible.
   * @param {number} x The cell's X coordinate.
   * @param {number} y The cell's Y coordinate.
   */
  _isInaccessible(x, y) {
    const rules = PatheryAnnotater.InaccessibleRules[this.map.get(x, y)];
    if (!rules) return false;

    return rules
      .some(rule =>
        rule.every(cell =>
          this.map.get(x + cell.x, y + cell.y) === cell.type
        , this)
      , this);
  }

  _annotateBottleneckCells() {}

  /** Annotates the specified cell.
   * @param {number} x The cell's X coordinate.
   * @param {number} y The cell's Y coordinate.
   * @param {AnnotationType} value How to annotate this cell.
   */
  _annotateCell(x, y, value) {
    this.getHTMLElement(x, y).firstElementChild.style.backgroundColor = value;
  }

  /** Gets the HTML element for the specified cell.
   * @param {number} x The cell's X coordinate.
   * @param {number} y The cell's Y coordinate.
   * @returns {HTMLElement} The HTML element for the specified cell.
   */
  getHTMLElement(x, y) { return document.getElementById(`${this.id},${y},${x}`); }
}

var super_clearwalls = super_clearwalls ?? clearwalls;
clearwalls = function(mapid) {
  super_clearwalls(mapid);
  annotate(mapid);
}

var super_grid_click = super_grid_click ?? grid_click;
grid_click = function(obj) {
  super_grid_click(obj);
  const tmp = obj.id.split(',');
  const mapid = tmp[0] - 0;
  annotate(mapid);
}

/** The annotater for each map ID.
 * @type {Object.<number, PatheryAnnotater>}
 */
var annotaters = {};

/** Annotates the specified map id.
 * @param {number} mapid The map's ID.
 */
function annotate(mapid) {
  let annotater = annotaters[mapid];
  if (!annotater)
    annotaters[mapid] = annotater = new PatheryAnnotater(mapid);
  annotater.annotate();
}
