import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { first } from 'rxjs';
import { AuthenticateRequest } from 'src/app/classes/Authentication';
import { DiabetsApiService } from 'src/app/services/diabets-api.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  constructor(private apiService: DiabetsApiService, private router: Router) { }

  ngOnInit(): void {
  }

  public login(login: string, password: string) {
    let request: AuthenticateRequest = {
      login: login,
      password: password
    };

    this.apiService.authenticate(request)
    .pipe(first())
      .subscribe({
          next: () => {
            this.router.navigate(['menu'])
          }
      });
  }
}
