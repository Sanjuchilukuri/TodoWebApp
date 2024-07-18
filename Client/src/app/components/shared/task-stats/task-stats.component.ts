import { Component, Input } from '@angular/core';
import { componentActions } from '../../../constants/constants';

@Component({
  selector: 'app-task-stats',
  // standalone: true,
  // imports: [],
  templateUrl: './task-stats.component.html',
  styleUrl: './task-stats.component.scss',
})
export class TaskStatsComponent {
  @Input() Percentage: number;
  @Input() tasksStatus: string;
  componentAction: { [key: string]: string };

  constructor() {
    this.Percentage = 0;
    this.tasksStatus = '';
    this.componentAction = componentActions;
  }
}
