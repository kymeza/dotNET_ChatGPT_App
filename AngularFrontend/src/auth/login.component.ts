import { Component } from '@angular/core';
import { CommonModule } from '@angular/common'; // Import CommonModule for ngClass and other common directives
import { FormsModule } from '@angular/forms';

import { AuthService } from './auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,  // Mark it as standalone
  imports: [CommonModule, FormsModule],  // Add CommonModule and FormsModule to imports
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  username: string = '';
  password: string = '';
  errorMessage: string = '';

  constructor(private authService: AuthService, private router: Router) {}

  onLogin(): void {
    this.authService.login(this.username, this.password).subscribe({
      next: (response) => {
        const token = response?.token;
        if (token) {
          this.authService.storeToken(token);
          this.router.navigate(['/chat']);  // Redirect after successful login
        }
      },
      error: () => {
        this.errorMessage = 'Invalid username or password';
      }
    });
  }
}
