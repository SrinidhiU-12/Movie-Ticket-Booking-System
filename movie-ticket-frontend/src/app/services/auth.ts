// import { Service } from '@angular/core';

// @Service()
// export class Auth {}


import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class Auth {
  //private apiUrl = 'http://localhost:5082/api/auth';
  private apiUrl = 'https://localhost:7275/api/Auth';
  constructor(private http: HttpClient) { }

  register(data: {
    fullName: string;
    email: string;
    password: string;
    phoneNumber: string;
  }): Observable<any> {
    return this.http.post(`${this.apiUrl}/register`, data);
  }

  login(data: {
    email: string;
    password: string;
  }): Observable<any> {
    return this.http.post(`${this.apiUrl}/login`, data);
  }
}

  
