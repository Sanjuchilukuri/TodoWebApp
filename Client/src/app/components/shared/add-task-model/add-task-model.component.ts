import { Component, OnInit, Signal, signal } from '@angular/core';
import { UtilsService } from '../../../services/utilServices/utils.service';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { TodoService } from '../../../services/todoServices/todo.service';
import { ToastrService } from 'ngx-toastr';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-add-task-model',
  // standalone: true,
  // imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './add-task-model.component.html',
  styleUrl: './add-task-model.component.scss',
})
export class AddTaskModelComponent  implements OnInit{
  newTaskForm: FormGroup;
  openModel: Signal<boolean>;
  taskToUpdate: number;
  
  constructor(private todoService: TodoService, private utilsService: UtilsService, private formBuilder: FormBuilder, private toaster: ToastrService)
  {
    this.newTaskForm = this.formBuilder.group({
      title: ['', Validators.required],
      description: ['', Validators.required],
    });
    this.openModel = signal(false);
    this.taskToUpdate = 0;
  }

  ngOnInit(): void {
    this.utilsService.displayAddTaskModel.subscribe((value) => {
      this.openModel = signal(value);
    });
    this.utilsService.taskToUpdate.subscribe((value) => {
      this.taskToUpdate = value;
      if (this.taskToUpdate != 0)
      {
        this.todoService.getTaskById(this.taskToUpdate).subscribe({
          next: (data) => {
            this.newTaskForm.setValue({
              title: data.title,
              description: data.description,
            });
          },
          error: (error) => {
            this.toaster.error(error.error);
          },
        });
      }
    });
  }

  addTask() {
    if (this.taskToUpdate == 0 )
    {
      this.newTaskForm.setValue({
        title: this.newTaskForm.value.title.trim(),
        description: this.newTaskForm.value.description.trim(),
      });

      this.newTaskForm.markAllAsTouched();
      if(this.newTaskForm.valid)
      {
        this.todoService.addtodoTask(this.newTaskForm.value).subscribe({
          next: () => {
            this.utilsService.displayAddTaskModel.next(false);
            this.toaster.success('Task added successfully');
            this.utilsService.newTaskAdded.next(true);
            this.newTaskForm.reset();
          },
          error: (error) => {
            this.toaster.error(error.error);
          },
        });
      }
    }
    else 
    {
      this.newTaskForm.setValue({
        title: this.newTaskForm.value.title.trim(),
        description: this.newTaskForm.value.description.trim(),
      });
      this.newTaskForm.markAllAsTouched();
      if (this.newTaskForm.valid) {
        this.todoService.updateTaskById(this.taskToUpdate, this.newTaskForm.value).subscribe({
          next: () => {
            this.utilsService.displayAddTaskModel.next(false);
            this.utilsService.newTaskAdded.next(true);
            this.utilsService.taskToUpdate.next(0);
            this.newTaskForm.reset();
            this.toaster.success('Task updated successfully');
          },
          error: (error) => {
            this.toaster.error(error.error);
          },
        });
      }
    }
  }

  cancel() {
    this.newTaskForm.reset();
    this.openModel = signal(false);
    this.utilsService.displayAddTaskModel.next(false);
    this.utilsService.taskToUpdate.next(0);
  }
}
