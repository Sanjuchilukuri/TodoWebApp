import { HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment.development';
import { Observable } from 'rxjs';
import { taskSummary } from '../../interfaces/taskSummary';
import { taskDetail } from '../../interfaces/taskDetail';
import { patchDocument } from '../../interfaces/patchDocument';
import { BaseService } from '../BaseService/base.service';
import { newTask } from '../../interfaces/newTask';

@Injectable({
  providedIn: 'root',
})
export class TodoService extends BaseService {

  addtodoTask(task: newTask) {
    return this.http.post(`${environment.BASE_API_ENDPOINT}/todoitem`, task);
  }

  getallTodoTask(filters: { status: string; date: Date }): Observable<taskSummary[]> {
    let params = new HttpParams()
      .set('status', filters.status)
      .set('date', filters.date.toISOString());
    return this.http.get<taskSummary[]>(`${environment.BASE_API_ENDPOINT}/todoitem`, {params: params,});
  }

  getActiveTasks(): Observable<taskDetail[]> {
    let params = new HttpParams().set('status', false);
    return this.http.get<taskDetail[]>(`${environment.BASE_API_ENDPOINT}/todoitem/tasksbystatus`,{ params: params });
  }

  getCompletedTasks(): Observable<taskDetail[]> {
    let params = new HttpParams().set('status', true);
    return this.http.get<taskDetail[]>(`${environment.BASE_API_ENDPOINT}/todoitem/tasksbystatus`,{ params: params });
  }

  deleteAllTasks(filters: { status: string; date: Date }): Observable<string> {
    let params = new HttpParams()
      .set('status', filters.status)
      .set('date', filters.date.toISOString());
    return this.http.delete<string>(`${environment.BASE_API_ENDPOINT}/todoitem`,{ params: params, responseType: 'text' as 'json' });
  }

  deleteTaskById(id: number) {
    return this.http.delete(`${environment.BASE_API_ENDPOINT}/todoitem/${id}`);
  }

  getTaskById(id: number): Observable<taskDetail> {
    return this.http.get<taskDetail>(`${environment.BASE_API_ENDPOINT}/todoitem/${id}`);
  }

  updateTaskById(id: number, task: newTask) {
    return this.http.put(`${environment.BASE_API_ENDPOINT}/todoitem/${id}`,task);
  }

  completeTask(taskId: number,patchObjectArray: patchDocument[]): Observable<taskDetail> {
    return this.http.patch<taskDetail>(`${environment.BASE_API_ENDPOINT}/todoitem/${taskId}`,patchObjectArray);
  }
}
