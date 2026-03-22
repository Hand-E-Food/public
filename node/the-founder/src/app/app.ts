import { Component, ComponentRef, ViewChild, ViewContainerRef } from '@angular/core';
import { Card } from './card/card';

@Component({
  selector: 'app',
  imports: [Card],
  templateUrl: './app.html',
  styleUrls: ['./app.css'],
})
export class App {
  private cardRef: ComponentRef<Card> = undefined!;

  @ViewChild('cardHost', { read: ViewContainerRef })
  private cardHost!: ViewContainerRef;

  ngAfterViewInit() {
    this.addCard();
  }

  addCard() {
    this.cardRef = this.cardHost.createComponent(Card);
  }

  public onClick(event: MouseEvent) {
    const card = this.cardRef.instance;
    card.x = event.clientX - card.width / 2;
    card.y = event.clientY - card.height / 2;
  }
}
