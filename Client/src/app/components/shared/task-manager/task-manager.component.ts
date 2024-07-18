import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { componentActions } from '../../../constants/constants';
import { taskDetail } from '../../../interfaces/taskDetail';
import { TodoService } from '../../../services/todoServices/todo.service';
import { UtilsService } from '../../../services/utilServices/utils.service';
import { ToastrService } from 'ngx-toastr';
import { TaskDetailsPopupComponent } from '../task-details-popup/task-details-popup.component';
import { patchDocument } from '../../../interfaces/patchDocument';

@Component({
  selector: 'app-task-manager',
  // standalone: true,
  // imports: [CommonModule, TaskDetailsPopupComponent],
  templateUrl: './task-manager.component.html',
  styleUrl: './task-manager.component.scss',
})
export class TaskManagerComponent implements OnInit {
  componentAction: string;
  componentActions;
  tasks: taskDetail[];
  taskToSend: taskDetail;
  tasksByDate: taskDetail[][];
  clickeditem: number;
  todayDate: Date;

  constructor(
    private router: ActivatedRoute,
    private todoService: TodoService,
    private utilsServices: UtilsService,
    private toaster: ToastrService
  ) {
    this.componentAction = '';
    this.componentActions = componentActions;
    this.tasks = [];
    this.tasksByDate = [];
    this.taskToSend = {} as taskDetail;
    this.clickeditem = 0;
    this.todayDate = new Date();
  }

  ngOnInit() {
    this.router.data.subscribe((data) => {
      this.componentAction = data['status'];
    });

    this.getTasks();

    this.utilsServices.newTaskAdded.subscribe(() => {
      this.getTasks();
    });
  }

  getTasks() {
    if (this.componentAction == componentActions.Active) {
      this.todoService.getActiveTasks().subscribe({
        next: (activeTasks) => {
          this.tasks = activeTasks;
          this.groupTasksByDate(activeTasks);
        },
        error: (error) => {
          if (error.status === 404) {
            this.tasks = [];
            this.tasksByDate = [];
          }
        },
      });
    } else if (this.componentAction === componentActions.Completed) {
      this.todoService.getCompletedTasks().subscribe({
        next: (completedTasks) => {
          this.tasks = completedTasks;
          this.groupTasksByDate(completedTasks);
        },
        error: (error) => {
          if (error.status === 404) {
            this.tasks = [];
            this.tasksByDate = [];
          }
        },
      });
    }
  }

  groupTasksByDate(tasks: taskDetail[]): void {
    if (tasks.length === 0) {
      this.tasksByDate = [];
      return;
    }
    let prevDate = new Date(tasks[0].createdOn);
    let index = 0;
    this.tasksByDate = [[]];
    tasks.forEach((task) => {
      const currentDate = new Date(task.createdOn);
      if (
        prevDate.getDate() === currentDate.getDate() &&
        prevDate.getMonth() === currentDate.getMonth() &&
        prevDate.getFullYear() === currentDate.getFullYear()
      ) {
        this.tasksByDate[index].push(task);
      } else {
        index++;
        this.tasksByDate[index] = [task];
        prevDate = currentDate;
      }
    });
    this.tasksByDate = this.tasksByDate.sort((a, b) => {
      if (a[0].createdOn < b[0].createdOn) return 1;
      if (a[0].createdOn > b[0].createdOn) return -1;
      return 0;
    });
  }

  displayDetails(task: taskDetail) {
    this.taskToSend = task;
    if (this.clickeditem === task.id) {
      this.clickeditem = 0;
    } else {
      this.clickeditem = task.id;
    }
  }

  isToday(dateString: Date): boolean {
    const taskDate = new Date(dateString);
    const todayDate = new Date();
    return (
      taskDate.getDate() === todayDate.getDate() &&
      taskDate.getMonth() === todayDate.getMonth() &&
      taskDate.getFullYear() === todayDate.getFullYear()
    );
  }

  refreshTasks(data: { action: string; taskId: number }) {
    if (data.action == 'TaskCompleted') {
      const patchObject: patchDocument = {
        path: 'IsCompleted',
        op: 'replace',
        value: true,
      };
      const patchObjectArray: patchDocument[] = [patchObject];
      this.todoService.completeTask(data.taskId, patchObjectArray).subscribe({
        next: () => {
          debugger;
          this.getTasks();
          this.toaster.info('Task Completed');
        },
        error: (error) => {
          this.toaster.error(error.error);
        },
      });
    } else if (data.action == 'TaskDeleted') {
      this.todoService.deleteTaskById(data.taskId).subscribe({
        next: () => {
          this.toaster.success('Task Deleted Successfully');
          this.getTasks();
        },
        error: (error) => {
          this.toaster.error(error.error);
        },
      });
    }
  }
}
