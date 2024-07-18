import { CommonModule } from '@angular/common';
import { Component, Input, Signal, signal } from '@angular/core';
import { Router } from '@angular/router';
import { UtilsService } from '../../../services/utilServices/utils.service';
import { MobileNavigationComponent } from '../../mobile-navigation/mobile-navigation.component';

@Component({
  selector: 'app-topbar',
  // standalone: true,
  // imports: [CommonModule, MobileNavigationComponent],
  templateUrl: './topbar.component.html',
  styleUrl: './topbar.component.scss',
})
export class TopbarComponent {
  @Input() headerName!: string;  
  displayAddTaskModel: Signal<boolean>;
  
  constructor(private utilsService: UtilsService, private router: Router) {
    this.displayAddTaskModel = signal(false);
  }

  logout() {
    localStorage.clear();
    this.router.navigateByUrl('/home'); 
  }

  showAddTaskModel() {
    this.utilsService.displayAddTaskModel.next(true);
    this.utilsService.displayAddTaskModel.subscribe((value) => { 
      this.displayAddTaskModel = signal(value);
    })
  }
}
