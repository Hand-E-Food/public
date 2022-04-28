// ==UserScript==
// @name         Pathery Annotator
// @namespace    http://tampermonkey.net/
// @version      1.0
// @description  Annotates Pathery maps.
// @author       Mark Richardson
// @match        https://www.pathery.com/
// @icon         https://www.google.com/s2/favicons?sz=64&domain=pathery.com
// @grant        none
// ==/UserScript==

/* eslint no-multi-spaces: off */
/* global 
    clearwalls: true,
    grid_click: true,
    loadSolution: true,
    mapdata: false,
    solution: false,
*/

/** The data structure of the elements in the global `mapdata`.
 * @typedef {{
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
 * }} MapData
 */

/** The type of a cell.
 * @readonly @enum {string}
 */
const CellType = {
  Checkpoint: "c",
  Finish: "f",
  Open: "o",
  Unbuildable: "p",
  Rock: "r",
  Start: "s",
  TeleportIn: "t",
  TeleportOut: "u",
  Wall: "w",
  SinglePathRock: "x",
  DirectionalForce: "z",
};

/** A cell of the map. */
class PatheryCell {

  /** Creates a cell used for the exterior of the map.
   * @static
   * @param {number} x The cell's X coordinate.
   * @param {number} y The cell's Y coordinate.
   * @returns {PatheryCell} The cell.
   */
  static exterior(x, y) {
    const cell = new PatheryCell(null, x, y, ['r', 1]);
    cell.inaccessible = 10000;
    return cell;
  };

  /** The cell types that are inaccessible.
   * @static @readonly @type {Array<CellType>}
   */
  static InaccessibleCellTypes = [
    CellType.Rock,
    CellType.Wall,
  ];

  /** The cell types that indicate a terminus.
   * @static @readonly @type {Array<CellType>}
   */
  static TerminusCellTypes = [
    CellType.Checkpoint,
    CellType.Finish,
    CellType.Start,
    CellType.TeleportOut,
  ];

  /** The HTML element for this cell.
   * @readonly @type {HTMLDivElement}
   */
  html;

  /** This cell's X coordinate.
   * @readonly @type {number}
   */
  x;

  /** This cell's Y coordinate.
   * @readonly @type {number}
   */
  y;

  /** This cell's type.
   * @readonly @type {CellType}
   */
  type;

  /** The specific index for this cell's type.
   * @readonly @type {number}
   */
  index;

  /** A terminus is a tile where the path could enter and/or exit via only one direction. `null` if
   * this tile is not a terminus. Any other value if this tile is a terminus. The value indicates
   * terminuses of the same purpose, including: normal/reverse start, finish, each checkpoint, and
   * each teleport out.
   * @readonly @type {string}
   */
  terminus;

  /** `null` if the path could be routed here. A positive number if this tile cannot be reached.
   * The number indicates the contiguous group of inaccessible tiles this tile is connected to.
   * @type {boolean}
   */
  inaccessible;

  /** `true` if this cell needs to be reanalysed. Otherwise, `false`.
   * @type {boolean}
   */
  isDirty;

  /** Initialises a new cell from the specified map tile.
   * @param {number} mapid The ID of the map owning this cell.
   * @param {number} x The cell's X coordinate.
   * @param {number} y The cell's Y coordinate.
   * @param {Array<string, number>} tile The tile from the original map data.
   */
  constructor(mapid, x, y, tile) {
    this.html = mapid ? document.getElementById(`${mapid},${y},${x}`) : undefined;
    this.x = x;
    this.y = y;
    this.type = tile[0];
    this.index = tile[1] - 1;
    this.terminus = PatheryCell.TerminusCellTypes.includes(this.type)
      ? this.type + this.index
      : null;
    this.reset();
  }

  /** Resets this cell to its initial state. */
  reset() {
    if (this.type === CellType.Wall) this.type = CellType.Open;
    if (PatheryCell.InaccessibleCellTypes.includes(this.type)) {
      this.makeInaccessible();
    } else {
      this.inaccessible = null;
    }
    this.isDirty = true;
  }

  /** Makes this tile inaccessible.
   * @param {CellType} type The type to set for this tile. If omitted, leaves the type unchanged.
   */
  makeInaccessible(type = undefined) {
    if (type) this.type = type;
    this.inaccessible = (this.y + 1) * 100 + this.x + 1;
  }
}

/** Manages the state of the map being analysed. */
class PatheryMap {

  /** This map's ID.
   * @readonly @type {number}
   */
  id;

  /** This map's width.
   * @readonly @type {number}
   */
  width;

  /** This map's height.
   * @readonly @type {number}
   */
  height;

  /** The information derived for this map.
   * @readonly @type {Array<Array<PatheryCell>>}
   */
  cells;

  /** The locations each path can start at. Always contains two elements:
   * * a list of the normal start locations
   * * a list of the reverse start locations
   * @readonly @type {Array<Array<{x: number, y: number}>>}
   */
  starts = [[],[]];

  /** The finish locations.
   * @readonly @type {Array<{x: number, y: number}>}
   */
  finishes = [];

  /** The locations of each checkpoint. The final list represents the finish.
   * @readonly @type {Array<Array<{x: number, y: number}>>}
   */
  checkpoints;

  /** The in and out location of each teleport.
   * @readonly @type {Array<{in: {x: number, y: number}, out: {x: number, y: number}}>}
   */
  teleports;

  /** Initialises a Map for the specified map ID.
   * @param {number} mapid The map ID.
   */
  constructor(mapid) {
    const data = mapdata[mapid];
    this.id = mapid;
    this.width = data.width;
    this.height = data.height;

    this.cells = new Array(this.height);
    for(let y = 0; y < this.height; y++) {
      const tileRow = data.tiles[y];
      const cellRow = new Array(this.width);
      this.cells[y] = cellRow;
      for(let x = 0; x < this.width; x++) {
        cellRow[x] = new PatheryCell(mapid, x, y, tileRow[x]);
      }
    }

    this.checkpoints = new Array(data.checkpoints);
    for(let i = 0; i < this.checkpoints.length; i++) {
      this.checkpoints[i] = [];
    }

    this.teleports = new Array(data.teleports);
    for(let i = 0; i < this.teleports.length; i++) {
      this.teleports[i] = { in: null, out: null };
    }

    for(let y = 0; y < this.height; y++) {
      for(let x = 0; x < this.width; x++) {
        const cell = this.cell(x, y);
        switch (cell.type) {
          case CellType.Checkpoint:  this.checkpoints[cell.index].push({ x, y }); break;
          case CellType.Finish:      this.finishes.push({ x, y });                break;
          case CellType.Start:       this.starts[cell.index].push({ x, y });      break;
          case CellType.TeleportIn:  this.teleports[cell.index].in = { x, y };    break;
          case CellType.TeleportOut: this.teleports[cell.index].out = { x, y };   break;
        }
      }
    }
  }

  /** Gets the specified cell.
   * @param {number} x The cell's X coordinate.
   * @param {number} y The cell's Y coordinate.
   * @returns {PatheryCell} The specified cell.
   */
  cell(x, y) { return (x >= 0 && x < this.width && y >= 0 && y < this.height) ? this.cells[y][x] : PatheryCell.exterior(x, y); }

  /** Resets every cell. */
  reset() { this.cells.forEach(row => row.forEach(cell => cell.reset())); }
}

/** @typedef {{dx: number, dy: number, fits: (cell: PatheryCell) => boolean}} PatheryRuleCell */

/** @typedef {Array<PatheryRuleCell>} PatheryRule */

/** The detection rules.
 * @readonly
 */
const PatheryRules = function(){
  let terminus;
  function isInaccessible(cell) { return cell.inaccessible > 0; }
  function isPathable(cell) { return !cell.inaccessible && !cell.terminus; }
  function isTerminus(cell) { return terminus = cell.terminus; }
  function isSameTerminus(cell) { return terminus === cell.terminus; }

  /** Generate a simple object describing the CellType expected at a tile offset.
   * @param {number} dx The cell's X offset.
   * @param {number} dy The cell's Y offset.
   * @param {(cell: PatheryCell) => boolean} fits The . Default: `isInaccessible`
   * @returns {PatheryRuleCell}
   */
  function d(dx, dy, fits = isInaccessible) { return {dx, dy, fits}; }

  /** Keep this rule in one orientation.
   * @param {PatheryRule} rule The rule to transform.
   * @returns {Array<PatheryRule>} A set of transformed rules.
   */
  function x1(rule) { return [ rule ]; }

  /** Mirror this rule into two orientations.
   * @param {PatheryRule} rule The rule to transform.
   * @returns {Array<PatheryRule>} A set of transformed rules.
   */
  function x2(rule) {
    return [
      rule,
      rule.map(cell => d(cell.dy, cell.dx, cell.fits)),
    ];
  }

  /** Rotate this rule into four orientations.
   * @param {PatheryRule} rule The rule to transform.
   * @returns {Array<PatheryRule>} A set of transformed rules.
   */
  function x4(rule) {
    return [
      rule,
      rule.map(cell => d(-cell.dy,  cell.dx, cell.fits)),
      rule.map(cell => d(-cell.dx, -cell.dy, cell.fits)),
      rule.map(cell => d( cell.dy, -cell.dx, cell.fits)),
    ];
  }

  /** Mirror and rotate this rule into eight orientations.
   * @param {PatheryRule} rule The rule to transform.
   * @returns {Array<PatheryRule>} A set of transformed rules.
   */
  function x8(rule) { return x2(rule).map(x4).flat(); }

  return {
    /** Rules indicating a directional force cell is inaccessible.
     * @readonly @type {Array<PatheryRule>}
     */
    directionalForce: [
      x4([d(-1, 0), d(0, -1)]), // Blocked on adjacent sides
    ].flat(),

    /** Rules indicating a pathable cell is inaccessible.
     * @readonly @type {Array<PatheryRule>}
     */
    pathable: [ // Block if a dead end, one or two tiles wide:
      x4([d(-1, 0), d(0, -1), d(+1,  0) ]), // Blocked on three sides
      x8([d(-1, 0), d(0, -1), d(+1, -1), d(+2, 0), d(+1, 0, isPathable) ]), // two adjacent pathables blocked on three sides
      x8([d(-1, 0), d(0, -1), d(+1, -1), d(+2, 0), d(+1, 0, isTerminus), d(+1, +1, isSameTerminus)]), // two adjacent pathables blocked on three sides
    ].flat(),

    /** Rules indicating a terminus cell is inaccessible.
     * @readonly @type {Array<PatheryRule>}
     */
    terminus: [
      x1([d(-1, 0), d(0, -1), d(+1, 0), d( 0, +1) ]), // Blocked on four sides
      x4([d(0, 0, isTerminus), d( 0, +1, isSameTerminus), d(-1, 0), d(0, -1), d(+1, 0)]), // Blocked on three sides and open side is the same terminus
    ].flat(),
  };
}();

/** Annotates a map. */
class PatheryAnnotator {

  /** The map ID serviced by this annotater.
   * @type {number}
   */
  id;

  /** The map holding the information used by this annotator.
   * @type {PatheryMap}
   */
  map;

  /** The number of times this annotator has been suspended.
   * @type {number}
   */
  _suspensions = 0;

  /** The cells that need to be reassessed.
   * @type {Array<PatheryCell>}
   */
  _dirtyCells;

  /** Initialises a PatheryAnnotator for the specified map ID.
   * @param {number} id The map ID.
   */
  constructor(id) {
    this.id = id;
    this.map = new PatheryMap(id);
  }

  /** Suspends annotation of this map. */
  suspend() {
    this._suspensions++;
  }

  /** Resumes annotation of this map. This must be called as many times as `suspend` was called. */
  resume() {
    if (this._suspensions > 0) this._suspensions--;
    if (this._suspensions === 0) this.annotateMap();    
  }

  /** Analyses and annotates the map. */
  annotateMap() {
    if (this._suspensions > 0) return;

    this._reset();

    while (this._dirtyCells.length > 0) {
      const cell = this._dirtyCells.pop();

      if (this._analyseCell(cell)) {
        const x = cell.x;
        const y = cell.y;
        this._makeDirty(x-1, y-1);
        this._makeDirty(x  , y-1);
        this._makeDirty(x+1, y-1);
        this._makeDirty(x-1, y  );
        this._makeDirty(x+1, y  );
        this._makeDirty(x-1, y+1);
        this._makeDirty(x  , y+1);
        this._makeDirty(x+1, y+1);
      }
    }
  }

  /** Resets this annotator's analysis. */
  _reset() {
    this.map.reset();

    this.solution
      .split(".")
      .filter(tile => tile.length > 0)
      .map(tile => tile.split(","))
      .forEach(tile => this.map.cell(tile[1], tile[0]).makeInaccessible(CellType.Wall), this);
    
    for(let y = 0; y < this.map.height; y++) {
      for(let x = 0; x < this.map.width; x++) {
        this._annotateCell(this.map.cell(x, y), null);
      }
    }

    this._dirtyCells = [];
    for (let y = 0; y < this.map.height; y++) {
      this._dirtyCells.push(...this.map.cells[y]);
    }
  }

  /** Gets the current solution drawn on this annotator's map.
   * @returns {string} The current solution encoded as `{x},{y}` coordinate pairs separated with periods (`.`).
   * @example ".1,2.3,4.5,6."
   */
  get solution() { return solution[this.id]; }

  /** Analyses and annotates the cell.
   * @param {PatheryCell} cell The cell to analyse.
   * @returns {boolean} `true` if the cell was changed. Otherwise, `false`.
   */
  _analyseCell(cell) {
    if (cell.inaccessible) return this._analyseInaccessibleCell(cell);
    if (cell.type === CellType.DirectionalForce) return this._analyseDirectionalForceCell(cell);
    if (cell.terminus) return this._analyseTerminusCell(cell);
    return this._analysePathableCell(cell);
  }

  /** Analyses an inaccessible cell to see if it is part of a larger group.
   * @param {PatheryCell} cell The cell to analyse.
   * @returns {boolean} `true` if the cell was changed. Otherwise, `false`.
   */
  _analyseInaccessibleCell(cell) {
    let highest = 0;
    for (let dy = -1; dy <= 1; dy++) {
      for (let dx = -1; dx <= 1; dx++) {
        const inaccessible = this.map.cell(cell.x + dx, cell.y + dy).inaccessible;
        if (highest < inaccessible) highest = inaccessible;
      }
    }
    if (cell.inaccessible === highest) return false;
    cell.inaccessible = highest;
    return true;
  }

  /** Analyses a directional force cell to see if it is inaccessible.
   * @param {PatheryCell} cell The cell to analyse.
   * @returns {boolean} `true` if the cell was changed. Otherwise, `false`.
   */
  _analyseDirectionalForceCell(cell) {
    if (this._fitsAnyRule(PatheryRules.directionalForce, cell))
      return this._annotateInaccessibleCell(cell);
    return false;
  }

  /** Analyses a terminus cell to see if it is inaccessible.
   * @param {PatheryCell} cell The cell to analyse.
   * @returns {boolean} `true` if the cell was changed. Otherwise, `false`.
   */
  _analyseTerminusCell(cell) {
    if (this._fitsAnyRule(PatheryRules.terminus, cell))
      return this._annotateInaccessibleCell(cell);
    return false;
  }

  /** Analyses a pathable cell to see if it is inaccessible.
   * @param {PatheryCell} cell The cell to analyse.
   * @returns {boolean} `true` if the cell was changed. Otherwise, `false`.
   */
  _analysePathableCell(cell) {
    if (this._fitsAnyRule(PatheryRules.pathable, cell))
      return this._annotateInaccessibleCell(cell);
    return false;
  }

  /** Checks if any of the specified rules fits the cell.
   * @param {Array<PatheryRule>} rules The rules to check.
   * @param {PatheryCell} cell The cell to analyse.
   * @returns {boolean} `true` if any rule fits. `false` if no rule fits.
   */
  _fitsAnyRule(rules, cell) {
    return rules
      .some(rule =>
        rule.every(ruleCell =>
          ruleCell.fits(this.map.cell(cell.x + ruleCell.dx, cell.y + ruleCell.dy))
        , this)
      , this);
  }

  /** Adds the cell at the specified coordinates to the list.
   * @param {number} x The cell's X coordinate.
   * @param {number} y The cell's Y coordinate.
   */
  _makeDirty(x, y) {
    if (x >= 0 && x < this.map.width && y >= 0 && y < this.map.height) {
      const cell = this.map.cell(x, y);
      if (!this._dirtyCells.includes(cell)) this._dirtyCells.push(cell);
    }
  }

  /** Annotates the specified cell as inaccessible.
   * @param {PatheryCell} cell The cell to annotate.
   * @param {AnnotationColor} value How to annotate this cell.
   * @returns {boolean} `true`
   */
  _annotateInaccessibleCell(cell) {
    this._annotateCell(cell, "#ff000088");
    cell.makeInaccessible();
    this._analyseInaccessibleCell(cell);
    return true;
  }

  /** Annotates the specified cell.
   * @param {PatheryCell} cell The cell to annotate.
   * @param {AnnotationColor} color The color to annotate this cell.
   */
  _annotateCell(cell, color) {
    cell.html.firstElementChild.style.backgroundColor = color;
  }
}

/** Original functions being overridden.
 * @readonly
 */
const overriden = {
  /** @readonly @type {{(mapid: number) => any}} */
  clearwalls: clearwalls,
  /** @readonly @type {{(obj: string) => any}} */
  grid_click: grid_click,
  /** @readonly @type {{(solution: string, moves: number, mapID: number) => any}} */
  loadSolution: loadSolution,
}

clearwalls = function(mapid) {
  const annotator = getAnnotator(mapid);
  annotator.suspend();
  try {
    return overriden.clearwalls(mapid);
  }
  finally {
    annotator.resume();
  }
}

grid_click = function(obj) {
  const mapid = obj.id.split(',')[0] - 0;
  const annotator = getAnnotator(mapid);
  annotator.suspend();
  try {
    return overriden.grid_click(obj);
  }
  finally {
    annotator.resume();
  }
}

loadSolution = function(solution, moves, mapID) {
  const annotator = getAnnotator(mapID);
  annotator.suspend();
  try {
    return overriden.loadSolution(solution, moves, mapID);
  }
  finally {
    annotator.resume();
  }
}

/** The annotator for each map ID.
 * @type {Object.<number, PatheryAnnotater>}
 */
var annotators = {};

/** Annotates the specified map id.
 * @param {number} mapid The map's ID.
 * @returns {PatheryAnnotator} The annotator for the specified map ID.
 */
function getAnnotator(mapid) {
  let annotator = annotators[mapid];
  if (!annotator) {
    annotator = new PatheryAnnotator(mapid);
    annotators[mapid] = annotator;
  }
  return annotator;
}
