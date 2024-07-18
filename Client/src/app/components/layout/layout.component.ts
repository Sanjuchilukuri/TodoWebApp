import { Component, DoCheck, OnInit } from '@angular/core';
import { SidenavComponent } from '../shared/sidenav/sidenav.component';
import { TopbarComponent } from '../shared/topbar/topbar.component';
import { Router, RouterOutlet } from '@angular/router';
import { AddTaskModelComponent } from '../shared/add-task-model/add-task-model.component';
import { CommonModule } from '@angular/common';
import { SharedModuleModule } from '../../shared-module/shared.module';

@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [
    // SidenavComponent,
    // TopbarComponent,
    RouterOutlet,
    // AddTaskModelComponent,
    CommonModule,
    SharedModuleModule,
  ],
  templateUrl: './layout.component.html',
  styleUrl: './layout.component.scss',
})
export class LayoutComponent implements DoCheck {
  componentName!: string;
  constructor(private router: Router) {}

  ngDoCheck(): void {
    const fullUrl = this.router.url;
    if (fullUrl.includes('dashboard')) {
      this.componentName = 'dashboard';
    } else if (fullUrl.includes('active')) {
      this.componentName = 'active';
    } else if (fullUrl.includes('completed')) {
      this.componentName = 'completed';
    }
  }
}
