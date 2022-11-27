import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, Observable, tap, throwError } from 'rxjs';
import { environment } from 'src/environments/environment';
import { AuthenticateRequest, ChangePassword, User } from '../classes/Authentication';
import { Examination } from '../classes/Examination';
import { AddPatientRequest, Patient } from '../classes/Patient';

@Injectable({
  providedIn: 'root'
})
export class DiabetsApiService {
  private httpOptions = {
    headers: new HttpHeaders({
      'Content-Type':  'application/json'
    })
  };
  
  constructor(private http: HttpClient, private router: Router) {}

  public authenticate(request: AuthenticateRequest): Observable<User> {
    return this.http.post<User>(`${environment.apiUrl}/users/authenticate`, request, this.httpOptions)
      .pipe(
        tap(this.setSession),
        catchError(this.handleError)
      );
  }

  public getPatients(): Observable<Patient[]> {
    return this.http.get<Patient[]>(`${environment.apiUrl}/users/get-patients`, this.httpOptions)
      .pipe(
        catchError(this.handleError)
      );
  }

  public getPatientExaminations(patientId: number): Observable<Examination[]> {
    let httpOptions = {
      headers: new HttpHeaders({
        'Content-Type':  'application/json',
        'patientId': String(patientId)
      })
    };

    return this.http.get<Examination[]>(`${environment.apiUrl}/users/get-examinations`,  httpOptions)
      .pipe(
        catchError(this.handleError)
      );
  }

  public validateToken() {
    return this.http.get<{message: string}>(`${environment.apiUrl}/users/validate-token`, {observe: 'response'});
  }

  public changePassword(request: ChangePassword) {
    return this.http.post(`${environment.apiUrl}/users/change-password`, request, this.httpOptions)
      .pipe(
        catchError(this.handleError)
      );
  }

  public addPatient(request: AddPatientRequest): Observable<Patient> {
    return this.http.post<Patient>(`${environment.apiUrl}/users/create-patient`, request, this.httpOptions)
      .pipe(
        catchError(this.handleError)
      );
  }

  private setSession(user: User): void {
    localStorage.setItem('id_token', user.token);
    localStorage.setItem('id', user.id);
    localStorage.setItem('name', user.name);
    localStorage.setItem('last_name', user.lastName);
    localStorage.setItem('isAdmin', String(user.isAdmin));
  } 

  private handleError(error: HttpErrorResponse) {
    if (error.status === 0) {
      console.error('An error occurred:', error.error);
    }
    else {
      console.error(
        `Backend returned code ${error.status}, body was: `, error.error);
    }

    return throwError(() =>
      'Something bad happened; please try again later.');
  }
}
