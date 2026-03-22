import { ChangeDetectorRef, Component } from '@angular/core';
import { NegativeStackComponent } from './negative-stack/negative-stack.component';
import { NegativeStack } from './negative-stack/negative-stack';
import { PositiveStackComponent } from './positive-stack/positive-stack.component';
import { PositiveStack } from './positive-stack/positive-stack';
import { Card } from './card/card';

@Component({
  selector: 'app',
  imports: [NegativeStackComponent, PositiveStackComponent],
  templateUrl: './app.html',
  styleUrls: ['./app.css'],
})
export class App {
  public negativeStack: NegativeStack = new NegativeStack();
  public positiveStack: PositiveStack = new PositiveStack();

  public constructor(private readonly changeDetectorRef: ChangeDetectorRef) {
    this.negativeStack.addCard(new Card("Hungry Family", "tree.jpg", -1));
    this.negativeStack.addCard(new Card("Hungry Family", "tree.jpg", -1));
    this.positiveStack.addCard(new Card("Welcome to Town", "tree.jpg", 1));
    this.positiveStack.addCard(new Card("Town Square", "tree.jpg", 2));
  }

  public onClick(event: MouseEvent): void {
    const card = new Card("Hungry Family", "tree.jpg", -1);
    card.left = 600;
    card.top = 400;
    this.changeDetectorRef.detectChanges();
    this.negativeStack.addCard(card);
    console.log("Card created.");
  }
}
