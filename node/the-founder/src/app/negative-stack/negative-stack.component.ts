import { Component, Input } from '@angular/core';
import { NegativeStack } from './negative-stack';
import { CardComponent } from '../card/card.component';

@Component({
  selector: 'negative-stack',
  imports: [CardComponent],
  templateUrl: './negative-stack.component.html',
  styleUrls: ['./negative-stack.component.css'],
})
export class NegativeStackComponent {
  @Input() public data!: NegativeStack;
  @Input() public left!: number;
  @Input() public top!: number;
}
