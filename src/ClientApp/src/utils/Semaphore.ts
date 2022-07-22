export default class Semaphore<T> {
  currentRequest: Promise<T> | null;
  /**
   * Creates a semaphore that only allows one function to run at a time
   */
  constructor() {
    this.currentRequest = null;
  }

  /**
   * Returns a Promise that will eventually return the result of the function passed in
   * Use this to limit the number of concurrent function executions
   * @returns Promise that will resolve with the resolved value as if the function passed in was directly called
   */
  async callFunction(fnToCall: () => Promise<T>) {
    // is the promise pending?
    if (this.currentRequest) {
      // wait for the promise to resolve
      console.log('reusing request');
      return this.currentRequest;
    }

    console.log('creating new request');
    this.currentRequest = fnToCall();
    var result = await this.currentRequest;
    this.currentRequest = null;
    return result;
  }
}
