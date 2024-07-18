import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { SpinnerService } from '../../services/SpinnerServices/spinner.service';

@Component({
  selector: 'app-spinner',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './spinner.component.html',
  styleUrl: './spinner.component.scss'
})
export class SpinnerComponent {
  isVisible!: boolean;
  constructor(spinnerService: SpinnerService) {
    spinnerService.showSpinner.subscribe((value:boolean) => { 
      this.isVisible = value;
    });
  }
}
