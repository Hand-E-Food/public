import { Component } from '@angular/core';

@Component({
  selector: 'card',
  imports: [],
  templateUrl: './card.html',
  styleUrls: ['./card.css'],
})
export class Card {
  private sides: CardSide[] = [
    { name: 'Tree', imageUrl: 'tree.jpg'}
  ]

  public get height(): number { return 178; }
  public get imageUrl(): string { return this.sides[this.side].imageUrl; }
  public get name(): string { return this.sides[this.side].name; }
  public get width(): number { return 126; }
  public side: number = 0;
  public x: number = 100;
  public y: number = 100;
}

export interface CardSide {
  readonly name: string;
  readonly imageUrl: string;
}
