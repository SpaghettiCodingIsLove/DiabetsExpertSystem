import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, tap, throwError } from 'rxjs';
import { environment } from 'src/environments/environment';
import { AuthenticateRequest, User } from '../classes/Authentication';

@Injectable({
  providedIn: 'root'
})
export class DiabetsApiService {
  private httpOptions = {
    headers: new HttpHeaders({
      'Content-Type':  'application/json'
    })
  };
  
  constructor(private http: HttpClient) {}

  public authenticate(request: AuthenticateRequest): Observable<User> {
    return this.http.post<User>(`${environment.apiUrl}/users/authenticate`, request, this.httpOptions)
      .pipe(
        tap(this.setSession),
        catchError(this.handleError)
      )

  }
  private setSession(user: User) {
    localStorage.setItem('id_token', user.token);
    localStorage.setItem('id', user.id);
    localStorage.setItem('name', user.name);
    localStorage.setItem('last_name', user.lastName);
    localStorage.setItem('isAdmin', String(user.isAdmin));
  } 
  private handleError(error: HttpErrorResponse) {
    if (error.status === 0) {
      console.error('An error occurred:', error.error);
    } else {
      console.error(
        `Backend returned code ${error.status}, body was: `, error.error);
    }

    return throwError(() =>
      'Something bad happened; please try again later.');
  }
}
