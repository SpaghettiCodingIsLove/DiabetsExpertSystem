import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AddDoctorRequest } from 'src/app/classes/Authentication';
import { DiabetsApiService } from 'src/app/services/diabets-api.service';

@Component({
  selector: 'app-add-doctor',
  templateUrl: './add-doctor.component.html',
  styleUrls: ['./add-doctor.component.scss']
})
export class AddDoctorComponent implements OnInit {

  constructor(private router: Router, private apiService: DiabetsApiService) { }

  ngOnInit(): void {
  }

  public addDoctor(name: string, lastName: string, login: string, password: string, isAdmin: boolean): void {
    let request: AddDoctorRequest = {
      name: name,
      lastName: lastName,
      login: login,
      password: password,
      isAdmin: isAdmin
    };

    this.apiService.addDoctor(request)
      .subscribe(() => this.router.navigate(['menu']));
  }
}
