// import { Component } from '@angular/core';

// @Component({
//   imports: [],
//   selector: 'app-register',
//   styleUrl: './register.css',
//   templateUrl: './register.html',
// })
// export class Register {}
// import { Component } from '@angular/core';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { Auth } from '../../services/auth';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule, RouterLink],
  templateUrl: './register.html',
  styleUrl: './register.css'
})
export class Register {
  fullName = '';
  email = '';
  password = '';
  confirmPassword = '';
  phoneNumber = '';

  message = '';

  constructor(
    private auth: Auth,
    private router: Router
  ) { }

  onRegister() {
    if (this.password !== this.confirmPassword) {
      this.message = 'Passwords do not match!';
      return;
    }

    const data = {
      fullName: this.fullName,
      email: this.email,
      password: this.password,
      phoneNumber: this.phoneNumber
    };

    this.auth.register(data).subscribe({
      next: (response) => {
        this.message = response.message || 'Registration successful!';
        this.router.navigate(['/']);
      },
      error: (error) => {
        this.message = error.error?.message || 'Registration failed!';
      }
    });
  }
}
