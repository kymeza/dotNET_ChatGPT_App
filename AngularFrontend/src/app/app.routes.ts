import { Routes } from '@angular/router';
import { LoginComponent } from '../auth/login.component'; // Your standalone components
import { UploadFileComponent } from '../fileUpload/fileupload.component';
import { ChatComponent } from '../chat/chat.component';
import { CommandComponent } from '../command/command.component';
import { AuthGuard } from '../auth/auth.guard'; // Your auth guard
import { ProductListComponent } from '../superTienda/products/product-list.component';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'chat', component: ChatComponent, canActivate: [AuthGuard] },
  { path: 'upload', component: UploadFileComponent, canActivate: [AuthGuard]},
  { path: 'command', component: CommandComponent, canActivate: [AuthGuard]},
  { path: 'supertienda/products', component: ProductListComponent, canActivate: [AuthGuard]},
  { path: '', redirectTo: '/login', pathMatch: 'full' }
];
