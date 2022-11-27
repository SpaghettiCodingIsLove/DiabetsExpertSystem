import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { first } from 'rxjs';
import { ChangePassword } from 'src/app/classes/Authentication';
import { DiabetsApiService } from 'src/app/services/diabets-api.service';

@Component({
  selector: 'app-password',
  templateUrl: './password.component.html',
  styleUrls: ['./password.component.scss']
})
export class PasswordComponent implements OnInit {

  constructor(private apiService: DiabetsApiService, private router: Router) { }

  ngOnInit(): void {
  }

  public changePassword(oldPassword: string, newPassword: string, repeatPassword: string) {
    if (newPassword === repeatPassword) {
    let request: ChangePassword = {
      oldPassword: oldPassword,
      newPassword: newPassword
    };

    this.apiService.changePassword(request)
    .pipe(first())
      .subscribe({
          next: () => {
            this.router.navigate(['menu']);
          }
      });
    }
  }
}
