import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class UtilsService {
  displayAddTaskModel: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  newTaskAdded: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  taskToUpdate:BehaviorSubject<number> = new BehaviorSubject<number>(0);
}
