import type { GameState } from '../states/primitive/game-state.js';

/** Aborts a state transition. */
export class AbortStateTransitionError extends Error {
  constructor() {
    super('State transition aborted.');
  }
}

/** Manages the game's state transitions. */
export class StateMachine {
  private isLocked: boolean = false;
  private readonly log: { (...data: any[]): void } = console.log;
  //private readonly log: {(...data: any[]): void} = () => {};
  private readonly stateStack: GameState[] = [];

  /** The current state. */
  public get current(): GameState | undefined {
    return this.stateStack.at(-1);
  }

  /**
   * Pauses the current state and temporarily enters a new state.
   * @param nextState The next state to enter.
   */
  public push(nextState: GameState): void {
    this.throwIfLocked();

    const prevState = this.current;
    if (prevState) {
      this.transitionOut(() => {
        if (prevState.pause) {
          this.log(`Pausing ${prevState.constructor.name}`);
          prevState.pause();
        }
      });
    }

    this.stateStack.push(nextState);
    if (nextState.enter) {
      this.log(`Entering ${nextState.constructor.name}`);
      nextState.enter?.();
    } else {
      this.log(`Focussing ${nextState.constructor.name}`);
    }
  }

  /**
   * Exits the current state and enters a new state.
   * @param nextState The next state to enter.
   */
  public next(nextState: GameState): void {
    this.throwIfLocked();

    const prevState = this.current;
    if (!prevState) throw new Error('Cannot transition to a new state when there is no current state.');
    this.transitionOut(() => {
      if (prevState.exit) {
        this.log(`Exiting ${prevState.constructor.name}`);
        prevState.exit();
      }
      this.stateStack.pop();
    });

    this.stateStack.push(nextState);
    if (nextState.enter) {
      this.log(`Entering ${nextState.constructor.name}`);
      nextState.enter();
    } else {
      this.log(`Focussing ${nextState.constructor.name}`);
    }
  }

  /** Exits the current state and resumes the previously paused state. */
  public pop(): void {
    this.throwIfLocked();

    const prevState = this.current;
    if (!prevState) throw new Error('Cannot pop a state when there is no current state.');
    this.transitionOut(() => {
      if (prevState.exit) {
        this.log(`Exiting ${prevState.constructor.name}`);
        prevState.exit();
      }
      this.stateStack.pop();
    });

    const nextState = this.current;
    if (nextState) {
      if (nextState.resume) {
        this.log(`Resuming ${nextState.constructor.name}`);
        nextState.resume();
      } else {
        this.log(`Focussing ${nextState.constructor.name}`);
      }
    }
  }

  private throwIfLocked(): void {
    if (this.isLocked) throw new Error('Cannot change states in when pausing or exiting a state.');
  }

  private transitionOut(transition: () => void): void {
    try {
      this.isLocked = true;
      transition();
    } catch (error) {
      if (error instanceof AbortStateTransitionError) {
        console.log(error.message);
        return;
      }
      throw error;
    } finally {
      this.isLocked = false;
    }
  }
}

/** A singleton instance of the state machine. */
export const stateMachine = new StateMachine();
