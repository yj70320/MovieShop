import { Injectable } from '@angular/core';
import { BehaviorSubject, catchError, map, Observable, of } from 'rxjs';
import { LoginModel } from '../../shared/models/LoginModel';
import { HttpClient } from '@angular/common/http';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})
export class Account {
  private apiUrl = 'https://localhost:7121/api'; // API 后端地址

  // BehaviorSubject 用于存储当前用户信息，保存最近一次的用户 subject 状态/值
  // 普通 subject 不会保存最近一次的状态
  // null 表示当前没有用户登录
  private CurrentUserSubject = new BehaviorSubject<any>(null);
  public currentUser = this.CurrentUserSubject.asObservable(); // 公开的可观察对象，供其他组件订阅，无法直接修改

  private isLoggedInSubject = new BehaviorSubject<any>(false);
  public isLoggedIn = this.isLoggedInSubject.asObservable(); // 公开的可观察对象，供其他组件订阅，无法直接修改

  private jwtHelper = new JwtHelperService();

  constructor(private http: HttpClient) {}

  login(login: LoginModel): Observable<boolean> {
    return this.http.post(`${this.apiUrl}/Account/login`, login).pipe(map((response: any) => {
    // return this.http.post('https://localhost:7121/api/Account/login', login).pipe(map((response: any) => {
        if (response && response.token) {
          localStorage.setItem('token', response.token);
          this.populateUserInfoFromJwtToken();  // 从 JWT token 中提取用户信息并更新当前用户状态
          return true;
        }
        return false;
      }),catchError((error: any) => {return of(false);})
    );
  }

  logout() {
    localStorage.removeItem('token');
    this.CurrentUserSubject.next(null);          // 清除当前用户信息
    this.isLoggedInSubject.next(false);          // 更新登录状态为未登录
  }

  populateUserInfoFromJwtToken() {
    var tokenValue = localStorage.getItem('token');  // 从浏览器本地存储中获取 JWT token
    if (tokenValue && !this.jwtHelper.isTokenExpired(tokenValue)) { // 如果 token 存在且未过期
      const decodedToken = this.jwtHelper.decodeToken(tokenValue);  // 解码 token 获取用户信息
      this.CurrentUserSubject.next(decodedToken);                   // 更新当前用户信息
      this.isLoggedInSubject.next(true);                            // 更新登录状态为已登录
    }
  }
}
