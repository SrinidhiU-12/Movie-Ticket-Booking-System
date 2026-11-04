// import { Component } from '@angular/core';

// @Component({
//   imports: [],
//   selector: 'app-login',
//   styleUrl: './login.css',
//   templateUrl: './login.html',
// })
// export class Login {}

import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class Login {
  email: string = '';
  password: string = '';

  onLogin() {
    console.log('Email:', this.email);
    console.log('Password:', this.password);
  }
}
