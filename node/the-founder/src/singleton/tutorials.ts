/** Manages which tutorials are displayed. */
const StorageKey = 'tutorials';

export class Tutorials {
  private readonly alwaysClosedKeys: Set<string> = new Set();
  private readonly closedKeys: Set<string> = new Set();
  private allClosed: boolean = false;

  public constructor(private readonly storage: Storage) {
    for (const key of this.storage.getItem(StorageKey) ?? []) this.alwaysClosedKeys.add(key);
  }

  /**
   * Indicates whether the specified tutorial should be shown.
   * @param key The key of the tutorial to check.
   * @returns True if the specified tutorial should be shown.
   */
  public shouldShow(key: string): boolean {
    return !this.allClosed && !this.closedKeys.has(key);
  }

  /**
   * Closes the specified tutorial for this session.
   * @param key The key of the tutorial to close.
   */
  public close(key: string): void {
    this.closedKeys.add(key);
  }

  /**
   * Closes the specified tutorial for all sessions.
   * @param key The key of the tutorial to close.
   */
  public closeAlways(key: string): void {
    this.closedKeys.add(key);
    this.alwaysClosedKeys.add(key);
    this.storage.setItem(StorageKey, JSON.stringify([...this.alwaysClosedKeys]));
  }

  /** Closes all tutorials for this session. */
  public closeAll(): void {
    this.allClosed = true;
  }

  /** Resets the closed state of all tutorials for all sessions. */
  public reset(): void {
    this.closedKeys.clear();
    this.alwaysClosedKeys.clear();
    this.allClosed = false;
    this.storage.setItem(StorageKey, JSON.stringify([]));
  }
}

/** A singleton instance of the Tutorials class. */
export const tutorials = new Tutorials(window.localStorage);
