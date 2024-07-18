import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SharedRoutingModule } from './shared-routing.module';
import { AddTaskModelComponent } from '../components/shared/add-task-model/add-task-model.component';
import { SidenavComponent } from '../components/shared/sidenav/sidenav.component';
import { TaskDetailsPopupComponent } from '../components/shared/task-details-popup/task-details-popup.component';
import { TaskManagerComponent } from '../components/shared/task-manager/task-manager.component';
import { TaskStatsComponent } from '../components/shared/task-stats/task-stats.component';
import { TopbarComponent } from '../components/shared/topbar/topbar.component';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { MobileNavigationComponent } from '../components/mobile-navigation/mobile-navigation.component';

@NgModule({
  declarations: [
    AddTaskModelComponent,
    SidenavComponent,
    TaskDetailsPopupComponent,
    TaskManagerComponent,
    TaskStatsComponent,
    TopbarComponent,
  ],
  imports: [
    CommonModule,
    SharedRoutingModule,
    ReactiveFormsModule,
    RouterLink,
    RouterLinkActive,
    MobileNavigationComponent,
  ],
  exports: [
    AddTaskModelComponent,
    SidenavComponent,
    TaskDetailsPopupComponent,
    TaskManagerComponent,
    TaskStatsComponent,
    TopbarComponent,
  ],
})
export class SharedModuleModule {}
