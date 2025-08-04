import { Routes } from '@angular/router';
import { LandingPageComponent } from './components/landing-page/landing-page.component';
import { RegisterPageComponent } from './components/register-page/register-page.component';
import { LoginPageComponent } from './components/login-page/login-page.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';

export const routes: Routes = [
    {
      path: '',
      component: LandingPageComponent
    },
    {
      path: 'register',
      component: RegisterPageComponent
    },
    {
      path: 'login',
      component: LoginPageComponent
    },
    {
      path: 'dashboard',
      component: DashboardComponent
    }
];
