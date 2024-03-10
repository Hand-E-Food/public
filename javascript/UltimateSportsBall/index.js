/**
 * The current game.
 * @type {Game}
 */
let game;

window.onload = () => {
    window.onload = undefined;
    game = new Game(
        new Field({
            height: 5,
            width: 9, // Including goals.
        })
    );
    document.body.replaceChildren(game.node);
}

/**
 * A game.
 */
class Game {
    /**
     * This game's field.
     * @readonly
     */
    field;

    /**
     * This game's HTML element.
     * @type {HTMLElement}
     */
    node;

    /**
     * Creates a new game.
     * @param {Field} field The sports field.
     */
    constructor(field) {
        this.field = field;

        const node = document.createElement('div');
        node.classList.add('game');
        node.appendChild(field.node);
        this.node = node;
    }
}

/**
 * A sports field.
 */
class Field {

    /**
     * This field's cells.
     * @type {FieldCell[]}
     * @readonly
     */
    cells = [];

    /**
     * This field's HTML element.
     * @type {HTMLElement}
     * @readonly
     */
    node;

    /**
     * Creates a new sports field.
     * @param {Size} size This field's size, not including the goals.
     */
    constructor() {
        const types = {
            ' ': FieldCellType.OutOfBounds,
            '.': FieldCellType.Normal,
            '-': FieldCellType.Shadow,
            'B': FieldCellType.BlueGoal,
            'R': FieldCellType.RedGoal,
            'b': FieldCellType.BlueStart,
            'r': FieldCellType.RedStart,
            's': FieldCellType.BothStart,
        };
        const map = [
            ' --s...s-- ',
            ' -.......- ',
            'Rr...s...bB',
            ' -.......- ',
            ' --s...s-- ',
        ];
        
        const node = document.createElement('div');
        node.classList.add('field');
        this.node = node;

        for (let y = 0; y < map.length; y++) {
            const line = map[y];
            for (let x = 0; x < line.length; x++) {
                const type = types[line[x]];
                if (type !== FieldCellType.OutOfBounds) {
                    const cell = new FieldCell({x, y}, type);
                    this.cells.push(cell);
                    this.node.appendChild(cell.node);
                }
            }
        }
    }
}

/**
 * A cell on a sports field.
 */
class FieldCell {

    /**
     * This field cell's HTML element.
     * @type {HTMLElement}
     * @readonly
     */
    node;

    /**
     * This cell's position on the field.
     * @type {Position}
     * @readonly
     */
    position;

    /**
     * This cell's type.
     * @type {FieldCellType}
     * @readonly
     */
    type;

    /**
     * Creates a new cell on a sports field.
     * @param {Position} position This cell's position on the field.
     * @param {FieldCellType} type This cell's type.
     */
    constructor(position, type) {
        this.position = position;
        this.type = type;
        
        const node = document.createElement('span');
        node.classList.add('cell');
        if (type === FieldCellType.BlueGoal) {
            node.classList.add('blue-goal');
        } else if (type === FieldCellType.RedGoal) {
            node.classList.add('red-goal');
        } else if (!(type & FieldCellType.CanShoot)) {
            node.classList.add('shadow');
        } else if (type & FieldCellType.Start) {
            node.classList.add('start');
        }
        node.style.left = `calc(var(--size) * ${position.x})`;
        node.style.top = `calc(var(--size) * ${position.y})`;
        this.node = node;
    }
}

/**
 * A field cell's type.
 * @readonly
 * @enum {number}
 */
const FieldCellType = {
    /** Outside the allowed play area. */
    OutOfBounds: 0,
    /** Players can enter this cell. */
    CanEnter: 1,
    /** Players can shoot for the goal from this cell. */
    CanShoot: 2,
    /** This cell is where a player starts. */
    Start: 4,
    /** This cell is a goal. */
    Goal: 8,
    /** This cell is red-specific. */
    Red: 16,
    /** This cell is blue-specific. */
    Blue: 32,

    /** Players can enter but not shoot for the goal from this cell. */
    Shadow: 1,
    /** This is a normal field cell. */
    Normal: 1 + 2,
    /** A red player starts here. */
    RedStart: 1 + 2 + 4 + 16,
    /** A blue player starts here. */
    BlueStart: 1 + 2 + 4 + 32,
    /** A red and a blue player starts here. */
    BothStart: 1 + 2 + 4 + 16 + 32,
    /** Red team's goal. */
    RedGoal: 8 + 16,
    /** Blue team's goal. */
    BlueGoal: 8 + 32,
}

/**
 * @typedef Position
 * An X,Y position.
 * @type {object}
 * @property {number} x The x position.
 * @property {number} y The y position.
 */

/**
 * A player ability.
 */
class Ability {
    
    /**
     * This ability's description.
     * @type {string}
     */
    description;
    
    /**
     * This ability's name.
     * @type {string}
     */
    name;

    /**
     * Creates a new player ability.
     * @param {string} name This ability's name.
     * @param {string} description This ability's description.
     */
    constructor(name, description) {
        this.name = name;
        this.description = description;
    }
}

/**
 * The player abilities.
 * @readonly
 * @enum {Ability}
 */
const Abilities = {
    Move: new Ability("Move", "Move 1 square."),
    Pass: new Ability("Pass", "Pass the ball up to 2 squares away."),

    Bluff: new Ability("Bluff", "Use if your opponent has not yet acted. Force the opponent to act next. Then this player must act immediately after that."),
    Cover: new Ability("Cover", "Your opponent loses their action."),
    Deflect: new Ability("Deflect", "Free action. Intercept the ball. This can only be used immedately after a pass. You must immediately pass the ball 2 squares away."),
    Kick: new Ability("Kick", "Pass the ball up to 4 squares away."),
    Intercept: new Ability("Intercept", "Free action. Intercept the ball. This can only be used immedately after a pass."),
    Push: new Ability("Push", "Move your opponent 1 square."),
    Run: new Ability("Run", "Move 2 squares."),
    Tackle: new Ability("Tackle", "The opponent loses their action, if they have one. If the opponent has the ball, take it and pass it 1 square away."),
    Throw: new Ability("Throw", "Pass the ball up to 3 squares away, except into a goal. The ball can only be intercepted in the thrower's or the receiver's square."),
}

/**
 * @typedef Player
 * @type {object}
 * @property {string} name This player's name.
 * @property {Ability[]} abilities This player's abilities.
 */

/**
 * The players in the game.
 * @readonly
 * @enum {Player}
 */
const Players = {
    Baseball: {
        name: "Baseball",
        unique: new Ability("Steal Base", "This player and a team mate act at the same time. You can decide which action occurs first."),
        abilities: [Abilities.Deflect, Abilities.Throw],
    },
    Basketball: {
        name: "Basketball",
        unique: new Ability("Slam Dunk", "When adjacent to the goal, throw the ball into the goal. This cannot be intercepted."),
        abilities: [Abilities.Throw,,],
    },
    BullFighting: {
        name: "Bull Fighting",
        abilities: [Abilities.Bluff, Abilities.Tackle],
    },
    Curling: {
        name: "Curling",
        unique: new Ability("Sweep", "This player can receive a pass from an additional 1 square away."),
        abilities: [,,],
    },
    Gridiron: {
        name: "Gridiron",
        unique: new Ability("Charge", "Move then tackle with the same action."),
        abilities: [Abilities.Bluff, Abilities.Tackle, Abilities.Throw],
    },
    Gymnastics: {
        name: "Gymnastics",
        unique: new Ability("Technique", "Immune to Bluff and Cover."),
        abilities: [Abilities.Deflect, Abilities.Intercept, Abilities.Run],
    },
    Hockey: {
        name: "Hockey",
        abilities: [Abilities.Bluff, Abilities.Kick, Abilities.Tackle],
    },
    Judo: {
        name: "Judo",
        unique: new Ability("Momentum", "Can Tackle without using a card immediately after an opponent moves into this square."),
        abilities: [Abilities.Bluff, Abilities.Tackle, Abilities.Throw],
    },
    LawnBowls: {
        name: "Lawn Bowls",
        abilities: [,,],
    },
    Polo: {
        name: "Polo",
        unique: new Ability("Horse", "Passes cannot be made through this square (but may start or end here.)"),
        abilities: [Abilities.Kick, Abilities.Run],
    },
    Skateboarding: {
        name: "Skateboarding",
        abilities: [Abilities.Push, Abilities.Run],
    },
    Soccer: {
        name: "Soccer",
        abilities: [Abilities.Cover, Abilities.Kick, Abilities.Run],
    },
    SumoWrestling: {
        name: "Sumo Wrestling",
        unique: new Ability("Huge", "Immune to Push and Tackle."),
        abilities: [Abilities.Intercept, Abilities.Push, Abilities.Tackle],
    },
    Tennis: {
        name: "Tennis",
        unique: new Ability("Serve", "Pass or Deflect an additional 1 square away."),
        abilities: [Abilities.Deflect, Abilities.Run],
    },
    TrackAndField: {
        name: "Track & Field",
        unique: new Ability("Sprint", "Use a Run card and move 2 squares in the same direction. Keep the Run card."),
        abilities: [Abilities.Run, Abilities.Throw],
    },
    Volleyball: {
        name: "Volleyball",
        abilities: [Abilities.Deflect, Abilities.Throw],
    },
}
