import { Component, Input } from '@angular/core';
import { PositiveStack } from './positive-stack';
import { CardComponent } from '../card/card.component';

@Component({
  selector: 'positive-stack',
  imports: [CardComponent],
  templateUrl: './positive-stack.component.html',
  styleUrls: ['./positive-stack.component.css'],
})
export class PositiveStackComponent {
  @Input() public data!: PositiveStack;
  @Input() public left!: number;
  @Input() public top!: number;

}
