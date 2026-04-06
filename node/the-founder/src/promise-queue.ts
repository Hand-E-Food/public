export class PromiseQueue {
  /** A promise that resolves when the queue is not empty. */
  private isNotEmpty!: Promise<void>;

  /** This queue's pending promises. */
  private readonly queue: Promise<void>[] = [];

  /** A function that resolves `isNotEmpty`. */
  private resolveQueueIsNotEmpty!: () => void;

  public constructor() {
    this.createEmptyQueuePromise();
  }

  /** This queue's number of pending promises. */
  public get length(): number {
    return this.queue.length;
  }

  /**
   * Enqueues a promise.
   * @param promise The promise to enqueue.
   */
  push(promise: Promise<void>): void {
    this.queue.push(promise);
    this.resolveQueueIsNotEmpty();
  }

  /**
   * Creates a new promise that blocks until this queue is not empty.
   */
  private createEmptyQueuePromise() {
    this.isNotEmpty = new Promise((resolve) => {
      this.resolveQueueIsNotEmpty = resolve;
    });
  }

  /**
   * Dequeues the next promise, waiting for one to be enqueued if the queue is empty.
   */
  async next(): Promise<void> {
    while (this.queue.length === 0) {
      await this.isNotEmpty;
      this.createEmptyQueuePromise();
    }
    await this.queue.shift()!;
  }

  /**
   * Flushes this queue, waiting for all pending promises to resolve and preventing new promises from being enqueued.
   */
  public async flush(): Promise<void> {
    this.resolveQueueIsNotEmpty();
    await this.isNotEmpty;
    await Promise.all(this.queue);
  }
}
