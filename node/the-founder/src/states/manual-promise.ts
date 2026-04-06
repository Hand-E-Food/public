export class ManualPromise<T> extends Promise<T> {
  static override get [Symbol.species]() {
    return Promise;
  }

  /**
   * Rejects this promise.
   * @param reason The reason this promise was rejected.
   */
  public readonly reject: (reason?: any) => void;

  /**
   * Resolves this promise.
   * @param value This promise's resulting value. If a promise is passed, this promise will resolve with the same
   * value as that promise.
   */
  public readonly resolve: (value: T | PromiseLike<T>) => void;

  public constructor() {
    let resolve!: (value: T | PromiseLike<T>) => void;
    let reject!: (reason?: any) => void;
    super((res, rej) => {
      resolve = res;
      reject = rej;
    });
    this.resolve = resolve;
    this.reject = reject;
  }
}
