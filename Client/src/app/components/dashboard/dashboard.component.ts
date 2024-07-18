import { CommonModule } from '@angular/common';
import { Component, OnInit, Signal, signal } from '@angular/core';
import { taskSummary } from '../../interfaces/taskSummary';
import { TodoService } from '../../services/todoServices/todo.service';
import { UtilsService } from '../../services/utilServices/utils.service';
import { AddTaskModelComponent } from '../shared/add-task-model/add-task-model.component';
import { TaskStatsComponent } from '../shared/task-stats/task-stats.component';
import { MobileNavigationComponent } from '../mobile-navigation/mobile-navigation.component';
import { FilterComponent } from '../filter/filter.component';
import { SharedModuleModule } from '../../shared-module/shared.module';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    SharedModuleModule,
    CommonModule,
    // AddTaskModelComponent,
    // TaskStatsComponent,
    MobileNavigationComponent,
    FilterComponent,
  ],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss',
})
export class DashboardComponent implements OnInit {
  datetoShow: Date;
  allTasks: taskSummary[];
  completedPercentage: number;
  activePercentage: number;
  tasksAdded: Signal<boolean>;
  filters: { status: string; date: Date };
  action: string;

  constructor(
    private todoService: TodoService,
    private utilsService: UtilsService
  ) {
    this.datetoShow = new Date();
    this.allTasks = [];
    this.completedPercentage = 0;
    this.activePercentage = 0;
    this.tasksAdded = signal(false);
    this.filters = { status: 'all', date: new Date() };
    this.action = this.filters.status;
  }

  ngOnInit(): void {
    this.getAllTasks();
    this.utilsService.newTaskAdded.subscribe((value) => {
      this.tasksAdded = signal(value);
      this.getAllTasks();
    });
  }

  getAllTasks() {
    this.todoService.getallTodoTask(this.filters).subscribe({
      next: (data) => {
        this.allTasks = data;
        this.calculatePercentages();
      },
      error: (error) => {
        if (error.status === 404) {
          this.allTasks = [];
          this.calculatePercentages();
        } else {
          throw error;
        }
      },
    });
  }

  calculatePercentages(): void {
    const completedTasks = this.allTasks.filter(
      (task) => task.isCompleted
    ).length;
    const totalTasks = this.allTasks.length;
    this.completedPercentage = totalTasks
      ? Math.round((completedTasks / totalTasks) * 100)
      : 0;
    this.activePercentage = totalTasks
      ? Math.round(((totalTasks - completedTasks) / totalTasks) * 100)
      : 0;
  }

  deleteAllTasks() {
    this.todoService.deleteAllTasks(this.filters).subscribe({
      next: () => {
        this.getAllTasks();
      },
      error: (error) => {
        if (error.status === 404) {
          this.allTasks = [];
        } else {
          throw error;
        }
      },
    });
  }

  handleFilterChanges(filters: { status: string; date: Date }) {
    this.action = filters.status;
    this.filters = filters;
    this.datetoShow = filters.date;
    this.getAllTasks();
  }

  isToday(): boolean {
    let todayDate = new Date();
    let existedDate = new Date(this.filters.date);
    if (todayDate.toDateString() === existedDate.toDateString()) return true;
    return false;
  }
}
