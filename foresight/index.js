/** A preconfigured condition.
 * @callback Condition
 * @returns {boolean}
 */

/** A conditional paragraph of text.
 * @typedef SceneParagraph
 * @property {Condition} condition The condition on which this is shown.
 * @property {string} text This paragraph's text.
 */

/** A conditional option.
 * @typedef SceneOption
 * @property {Condition} condition The condition on which this is shown.
 * @property {string} text This option's text.
 * @property {Scene} scene The scene this option navigates to.
 */

/** The output from a scene to be displayed to the player.
 * @typedef Script
 * @property {SceneParagraph[]} paragraphs The paragraphs of text to display.
 * @property {SceneOption[]} options The options to offer the player.
 */

/** A scene that generated conditionally and provides options for progression. */
class Scene {

    /** The number of times this scene has been entered.
     * @type {number}
     */
    entries = 0;

    /** The paragraphs this scene can generate.
     * @type {SceneParagraph[]}
     */
    paragraphs = [];

    /** The options this scene can generate.
     * @type {SceneOption[]}
     */
    options = [];

    /** Adds a paragraph or option to this scene.
     * @param {Condition} condition The condition on which this is shown.
     * @param {string} text The text to show.
     * @param {Scene} scene If specified, the scene this option navigates to. If omitted, a paragraph is created instead.
     * @returns {Scene} This scene for call chaining.
     */
    add(condition, text, scene) {
        let result = { condition, text };
        if (scene) {
            result.scene = scene;
            this.options.push(result);
        } else {
            this.paragraphs.push(result);
        }
        return this;
    }

    /** When overridden, executes before entering this scene.
     * @returns {void}
     */
    preEnter() {};

    /** Enters this scene and returns the text and options to display.
     * @returns {Script} The text and options to display.
     */
    enter() {
        this.preEnter();
        this.entries++;
        return {
            paragraphs: this.paragraphs.filter(paragraph => paragraph.condition()),
            options: this.options.filter(option => option.condition()),
        };
    }
}

/** Manages the game flow. */
class Engine {
    
    /** The currently displayed option elements.
     * @type {HTMLElement[]}
     */
    _optionElements = [];

    /** Initialises the game engine. */
    constructor() {
        document.body.onkeydown = e => this.onKeyDown(e);
    }

    /** Selects the option with the same number as the pressed key.
     * @param {KeyboardEvent} e The keyboard event.
     * @returns {boolean} `true` if the key was handled. Otherwise `undefined`.
     */
    onKeyDown(e) {
        if (e.altKey || e.ctrlKey || e.metaKey || e.shiftKey) return undefined;
        const n = e.key.charCodeAt(0) - 49;
        if (n < 0 || n >= this._optionElements.length) return undefined;
        this._optionElements[n].click();
        return true;
    }

    /** Enters the specified scene.
     * @param {Scene} scene The scene to enter.
     * @returns {void}
     */
    enter(scene) {
        const script = scene.enter();
        script.paragraphs.forEach(this._addParagraph, this);
        script.options.forEach(this._addOption, this);
        document.body.appendChild(document.createElement("hr"));
        window.scrollTo(0, document.body.scrollHeight);
    }

    /** Creates and displays an element for a paragraph.
     * @param {SceneParagraph} paragraph The text to write.
     * @returns {HTMLElement} The created element.
     */
    _addParagraph(paragraph) {
        return this._addElement(paragraph.text);
    }

    /** Creates and displays an element for an option.
     * @param {ScriptOption} option The option to write.
     * @returns {HTMLElement} The created element.
     */
    _addOption(option) {
        const key = [ "1️⃣", "2️⃣", "3️⃣" ][this._optionElements.length];
        const element = this._addElement(`${key} ${option.text}`);
        this._optionElements.push(element);
        element.classList.add("option");
        element.classList.add("available");
        element.onclick = e => this._choose(element, option.scene);
    }

    /** Creates and displays a paragraph element with the specified text.
     * @param {string} text The paragraph's text.
     * @returns {HTMLParagraphElement} The created element.
     */
    _addElement(text) {
        const textNode = document.createTextNode(text);
        const paragraphNode = document.createElement("p");
        paragraphNode.appendChild(textNode);
        document.body.appendChild(paragraphNode);
        return paragraphNode;
    }

    /** Follow the chosen option.
     * @param {HTMLElement} element The element presenting the option.
     * @param {Scene} scene The next scene.
     * @returns {boolean} `true`
     */
    _choose(element, scene) {
        this._optionElements.forEach(e => {
            e.onclick = undefined;
            e.classList.remove("available");
            e.classList.add(e === element ? "accepted" : "rejected");
        });
        this._optionElements = [];
        this.enter(scene);

        return true;
    }
}

/** Initialises all scenes.
 * @returns {Scene} The first scene.
 */
function initialiseScenes() {

    /** Always display this.
     * @returns {Condition} A condition that is always true.
     */
    const always = () => true;

    /** Only display this if the specified scene has been entered.
     * @param {Scene} scene The scene to check.
     * @returns {Condition} The condition.
     */
    function ifEntered(scene) { return () => scene.entries > 0; }

    /** Only display this if the specified scene has never been entered.
     * @param {Scene} scene The scene to check.
     * @returns {Condition} The condition.
     */
    function ifNeverEntered(scene) { return () => scene.entries == 0; }

    const beach = new Scene();
    const cliffs = new Scene();
    const docks = new Scene();
    const ladder = new Scene();
    const exterior = new Scene();
    const guardHut = new Scene();
    const start = new Scene();
    const shed = new Scene();
    const shedCellar = new Scene();
    const shedPassword = new Scene();
    const submarinePen = new Scene();
    const yachtCargo = new Scene();
    const yachtPassenger = new Scene();

    /** Adds an option to the scene to return to the start of the loop.
     * @param {Scene} scene The scene to add the option to.
     * @param {Condition} condition The condition on which to display this option. If omitted, this
     * option is always displayed.
     * @returns {void}
     */
    function returnToStart(scene, condition) {
        if (!condition) condition = always();
        scene.add(always, "The whole world is consumed by an infinitely intense white light.", start);
    }
    
    // Start
    start.add(always, "You approach the volcanic island.");
    start.add(always, "Land on the beach.", beach);
    start.add(always, "Land by the cliffs.", cliffs);
    start.add(ifEntered(ladder), "Dive to the submarine entrance.", submarinePen);

    // 1
    beach.add(always, "You walk along the beach.");
    beach.add(always, "Go to the docks.", docks);
    beach.add(always, "Go to the guard hut.", guardHut);
    //beach.add();

    // 11
    docks.add(always, "There is a yacht docked at the dock. Cargo is being unloaded. The gangplank is down.");
    docks.add(always, "Check the cargo.", yachtCargo);
    docks.add(always, "Board the yacht.", yachtPassenger);
    //docks.add();

    // 111
    yachtCargo.add(always, "Lots of crates.");
    //yachtCargo.add();
    //yachtCargo.add();
    //yachtCargo.add();
    returnToStart(yachtCargo);

    // 112
    yachtPassenger.add(always, "You're on a boat!");
    yachtPassenger.add(always, "Evesdrop on new recruits.", shedPassword);
    //yachtPassenger.add();
    //yachtPassenger.add();

    // 1121
    shedPassword.add(always, "You listen to the new recruits. What secrets can you learn? What intel is this group privy to?");
    shedPassword.add(always, "They're maintenance technicians, glorified grounds keepers.");
    shedPassword.add(always, "However, you do learn that the storage shed's password is 7433.");
    returnToStart(shedPassword);

    // 12
    guardHut.add(always, "You're at a guard hut.");
    returnToStart(guardHut);

    // 2
    cliffs.add(always, "You climb the cliffs.");
    cliffs.add(ifNeverEntered(shedPassword), "There is a storage shed. The heavy door is secured by a numeric keypad. It's suprisingly robust and you don't like your chances of breaking in.");
    cliffs.add(ifEntered(shedPassword), "There is a storage shed. The heavy door is secured by a numeric keypad.");
    cliffs.add(always, "Walk to the large structure built into the side of the mountain.", exterior);
    //cliffs.add();
    cliffs.add(ifEntered(shedPassword), "Type 7433 and enter the shed.", shed);

    // 21
    exterior.add(always, "There is a tall concrete wall gutting out of the ground. You can see a windowed section high at the top.");
    returnToStart(exterior);

    // 23
    shed.add(always, "A variety of heavy tools and equipment are here, including shovels, ladders, a portable generator.");
    shed.add(always, "There is a trap door in the ground.");
    shed.add(always, "Open the trap door and explore what's below.", shedCellar);
    shed.add(ifEntered(exterior), "Carry the ladder to the large structure built into the side of the mountain.", ladder);
    //storageShed.add();

    // 231
    shedCellar.add(always, "This room appers to not be part of the shed's original construction. It looks like guards dug out this cellar for unauthorised activitied.");
    shedCellar.add(always, "There are half empty crates of alcohol, cards, gambling chips. You find the wallet of someone named Murphy.");
    returnToStart(shedCellar);

    // 232
    ladder.add(always, "You carry the ladder to the structure and set it up against the concrete wall. You climb to the top and get a good look inside.");
    ladder.add(always, "You see a submarine pen. If there are submarines inside this island, there must be another entrance underwater.");
    returnToStart(ladder);

    // 3
    submarinePen.add(always, "You're in the submarine pen.");
    returnToStart(submarinePen);

    // Giant robot crab

    return start;
}

new Engine().enter(initialiseScenes());
