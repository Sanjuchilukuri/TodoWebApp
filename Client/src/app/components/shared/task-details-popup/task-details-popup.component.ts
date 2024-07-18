import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { taskDetail } from '../../../interfaces/taskDetail';
import { CommonModule } from '@angular/common';
import { UtilsService } from '../../../services/utilServices/utils.service';
import { componentActions } from '../../../constants/constants';

@Component({
  selector: 'app-task-details-popup',
  // standalone: true,
  // imports: [CommonModule],
  templateUrl: './task-details-popup.component.html',
  styleUrl: './task-details-popup.component.scss',
})
export class TaskDetailsPopupComponent implements OnInit {
  @Input() task: taskDetail;
  @Input() componentAction: string;
  componentActions;
  visible: boolean = false;
  @Output() refreshTasksEvent: EventEmitter<any>;
  constructor(private utilsServices: UtilsService) {
    this.task = {} as taskDetail;
    this.componentAction = '';
    this.componentActions = componentActions;
    this.refreshTasksEvent = new EventEmitter<any>();
  }
  ngOnInit(): void {}

  getRelativeTime(date: Date): string {
    const now = new Date();
    const past = new Date(date);
    const diffInSeconds = Math.floor((now.getTime() - past.getTime()) / 1000);
    const diffInMinutes = Math.floor(diffInSeconds / 60);
    const diffInHours = Math.floor(diffInMinutes / 60);
    const diffInDays = Math.floor(diffInHours / 24);

    if (diffInDays > 0) {
      return `Added ${diffInDays} day${diffInDays > 1 ? 's' : ''} ago`;
    } else if (diffInHours > 0) {
      return `Added ${diffInHours} hour${diffInHours > 1 ? 's' : ''} ago`;
    } else if (diffInMinutes > 0) {
      return `Added ${diffInMinutes} minute${diffInMinutes > 1 ? 's' : ''} ago`;
    } else {
      return `Added ${diffInSeconds} second${diffInSeconds > 1 ? 's' : ''} ago`;
    }
  }

  taskCompleted(taskId: number) {
    this.refreshTasksEvent.emit({ action: 'TaskCompleted', taskId: taskId });
  }

  editTask(taskId: number) {
    this.utilsServices.displayAddTaskModel.next(true);
    this.utilsServices.taskToUpdate.next(taskId);
  }

  deleteTask(taskId: number) {
    this.refreshTasksEvent.emit({ action: 'TaskDeleted', taskId: taskId });
  }
}
