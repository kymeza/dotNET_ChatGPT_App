import { Routes } from '@angular/router';
import { LoginComponent } from '../auth/login.component';
import { ChatComponent } from '../chat/chat.component';
import { CommandComponent } from '../command/command.component';
import { UploadFileComponent } from '../fileUpload/fileupload.component';
import { AuthGuard } from '../auth/auth.guard';
import { ProductListComponent } from '../supertienda/products/product-list.component';

export const routes: Routes = [
    {path: 'login', component: LoginComponent},
    {path: 'chat', component: ChatComponent, canActivate: [AuthGuard]},
    {path: 'upload', component: UploadFileComponent, canActivate: [AuthGuard]},
    {path: 'command', component: CommandComponent, canActivate: [AuthGuard]},
    {path: 'supertienda/products', component: ProductListComponent, canActivate: [AuthGuard]},
    {path: '', redirectTo: '/login', pathMatch: 'full'}
];
