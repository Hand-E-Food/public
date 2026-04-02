import type { GameState } from '../states/primitive/game-state.js';

/** Aborts a state transition. */
export class AbortStateTransitionError extends Error {
  constructor() {
    super('State transition aborted.');
  }
}

/** Manages the game's state transitions. */
export class StateMachine {
  private isLocked: string | undefined = undefined;
  private readonly log: { (...data: any[]): void } = (...data) =>
    console.log(new Date().toISOString().slice(11, 23), ...data);
  //private readonly log: {(...data: any[]): void} = () => {};
  private readonly stateStack: GameState[] = [];

  /** The current state. */
  public get current(): GameState | undefined {
    return this.stateStack.at(-1);
  }

  /**
   * Locks the state machine. Ensure this is wrapped in a try block with a finally block that calls `unlock()`.
   * ```typescript
   * try {
   *   stateMachine.lock();
   *   // Perform operations that require the state machine to be locked.
   * } finally {
   *   stateMachine.unlock();
   * }
   * ```
   */
  public lock(reason: string): void {
    if (this.isLocked) throw new Error(`State machine is already locked because it's ${this.isLocked}.`);
    this.isLocked = reason;
  }

  /** Unlocks the state machine. */
  public unlock(): void {
    if (!this.isLocked) throw new Error('State machine is not locked.');
    this.isLocked = undefined;
  }

  /**
   * Pauses the current state and temporarily enters a new state.
   * @param nextState The next state to enter.
   */
  public push(nextState: GameState): void {
    this.throwIfLocked();

    const prevState = this.current;
    if (prevState) {
      this.transitionOut('pausing a state', () => {
        if (prevState.pause) {
          this.log(`Pausing ${prevState.name}`);
          prevState.pause();
        }
      });
    }

    this.stateStack.push(nextState);
    if (nextState.enter) {
      this.log(`Entering ${nextState.name}`);
      nextState.enter?.();
    } else {
      this.log(`Focussing ${nextState.name}`);
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
    this.transitionOut('exiting a state', () => {
      if (prevState.exit) {
        this.log(`Exiting ${prevState.name}`);
        prevState.exit();
      }
      this.stateStack.pop();
    });

    this.stateStack.push(nextState);
    if (nextState.enter) {
      this.log(`Entering ${nextState.name}`);
      nextState.enter();
    } else {
      this.log(`Focussing ${nextState.name}`);
    }
  }

  /** Exits the current state and resumes the previously paused state. */
  public pop(): void {
    this.throwIfLocked();

    const prevState = this.current;
    if (!prevState) throw new Error('Cannot pop a state when there is no current state.');
    this.transitionOut('exiting a state', () => {
      if (prevState.exit) {
        this.log(`Exiting ${prevState.name}`);
        prevState.exit();
      }
      this.stateStack.pop();
    });

    const nextState = this.current;
    if (nextState) {
      if (nextState.resume) {
        this.log(`Resuming ${nextState.name}`);
        nextState.resume();
      } else {
        this.log(`Focussing ${nextState.name}`);
      }
    }
  }

  private throwIfLocked(): void {
    if (this.isLocked) throw new Error(`Cannot change states while ${this.isLocked}.`);
  }

  private transitionOut(reason: string, transition: () => void): void {
    try {
      this.isLocked = reason;
      transition();
    } catch (error) {
      if (error instanceof AbortStateTransitionError) {
        console.log(error.message);
        return;
      }
      throw error;
    } finally {
      this.isLocked = undefined;
    }
  }
}

/** A singleton instance of the state machine. */
export const stateMachine = new StateMachine();
