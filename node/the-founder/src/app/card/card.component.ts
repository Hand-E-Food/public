import { Component, Input } from '@angular/core';
import { Card } from './card';

@Component({
  selector: 'card',
  imports: [],
  templateUrl: './card.component.html',
  styleUrls: ['./card.component.css'],
})
export class CardComponent {
  @Input() public data!: Card;
}
