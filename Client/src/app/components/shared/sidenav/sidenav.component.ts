import { Component, Signal, signal } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { AddTaskModelComponent } from '../add-task-model/add-task-model.component';
import { CommonModule } from '@angular/common';
import { UtilsService } from '../../../services/utilServices/utils.service';

@Component({
  selector: 'app-sidenav',
  // standalone: true,
  // imports: [RouterLink, RouterLinkActive, AddTaskModelComponent, CommonModule],
  templateUrl: './sidenav.component.html',
  styleUrl: './sidenav.component.scss',
})
export class SidenavComponent {
  displayAddTaskModel: Signal<boolean>;
  constructor(private utilsService: UtilsService) {
    this.displayAddTaskModel = signal(false);
  }

  showAddTaskModel() {
    this.utilsService.displayAddTaskModel.next(true);
    this.utilsService.displayAddTaskModel.subscribe((value) => { 
      this.displayAddTaskModel = signal(value);
    })
  }
}
