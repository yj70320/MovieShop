import { Component } from '@angular/core';
import { LoginModel } from '../../shared/models/LoginModel';
import { Account } from '../../core/services/account';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {
  // Dependency Injection
  constructor(private accountService: Account, private router: Router) {}

  loginModel: LoginModel = {
    email: '',
    password: ''
  };

  invalidLogin: boolean = false;  // 默认登录成功
  onSubmit() {
    console.log('User clicked on submit button');
    console.log(this.loginModel);

    this.accountService.login(this.loginModel).subscribe(response => {
      if (response == true) {
        // redirect to home page
        // console.log('Login successful');
        this.router.navigateByUrl('/');    // http://localhost:4200/
      } else {
        // show error message
        this.invalidLogin = true;
        // console.log('Login failed');
      }
    }
  );
  }
}
