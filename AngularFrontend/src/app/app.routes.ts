import { Routes } from '@angular/router';
import { provideRouter } from '@angular/router';
import { LoginComponent } from '../auth/login.component'; // Your standalone components
import { ChatComponent } from '../chat/chat.component';
import { AuthGuard } from '../auth/auth.guard'; // Your auth guard

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'chat', component: ChatComponent, canActivate: [AuthGuard] },
  { path: '', redirectTo: '/login', pathMatch: 'full' }
];
