import { Routes } from '@angular/router';
import { LoginComponent } from '../auth/login.component';
import { ChatComponent } from '../chat/chat.component';
import { AuthGuard } from '../auth/auth.guard';
import { UploadFileComponent } from '../fileUpload/fileupload.component';

export const routes: Routes = [
    {path: 'login', component: LoginComponent},
    {path: 'chat', component: ChatComponent, canActivate: [AuthGuard]},
    {path: 'upload', component: UploadFileComponent, canActivate: [AuthGuard]},
    {path: '', redirectTo: '/login', pathMatch: 'full'}
];
