
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { Auth } from '../../services/auth';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, RouterLink],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class Login {
  email = '';
  password = '';
  message = '';

  constructor(
    private auth: Auth,
    private router: Router
  ) { }

  onLogin() {
    console.log('SIGN IN BUTTON CLICKED');
    const data = {
      email: this.email.trim(),
      password: this.password
    };

    this.auth.login(data).subscribe({
      next: (response) => {
        this.message = response.message || 'Login successful!';
        console.log('Login response:', response);
        this.router.navigate(['/movies']);
      },
      error: (error) => {
        this.message = error.error?.message || 'Login failed!';
        console.error('Login error:', error);
      }
    });
  }
}
