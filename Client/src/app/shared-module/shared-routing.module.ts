import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TaskManagerComponent } from '../components/shared/task-manager/task-manager.component';

const routes: Routes = [
  {
    path: 'active',
    component: TaskManagerComponent,
    data: { status: 'active' },
  },
  {
    path: 'completed',
    component: TaskManagerComponent,
    data: { status: 'completed' },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class SharedRoutingModule {}
