import { Routes } from '@angular/router';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { AuthenticationComponent } from './components/authentication/authentication.component';
import { LayoutComponent } from './components/layout/layout.component';
import { TaskManagerComponent } from './components/shared/task-manager/task-manager.component';
import { authGuard } from './services/authGuard/auth.guard';
import { PageNotFoundComponent } from './components/page-not-found/page-not-found.component';
import { HomeComponent } from './components/home/home.component';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'dashboard',
    pathMatch: 'full',
  },
  {
    path: 'home',
    component: HomeComponent,
  },
  {
    path: 'login',
    component: AuthenticationComponent,
    data: { status: 'login' },
  },
  {
    path: 'register',
    component: AuthenticationComponent,
    data: { status: 'register' },
  },
  {
    path: '',
    component: LayoutComponent,
    canActivate: [authGuard],
    children: [
      {
        path: 'dashboard',
        component: DashboardComponent,
      },
      {
        path: '',
        // component: TaskManagerComponent,
        loadChildren: () =>
          import('./shared-module/shared.module').then(
            (m) => m.SharedModuleModule
          ),
        data: { status: 'active' },
      },
      // {
      //   path: 'completed',
      //   component: TaskManagerComponent,
      //   data: { status: 'completed' },
      // },
    ],
  },
  {
    path: '**',
    pathMatch: 'full',
    component: PageNotFoundComponent,
  },
];
