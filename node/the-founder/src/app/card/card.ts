export class Card {
  public left: number = 0;
  public top: number = 0;
  public height: number = 178;
  public width: number = 126;

  public constructor(
    public readonly name: string,
    public readonly imageUrl: string,
    public readonly morale: number = 0,
  ) { }
}
