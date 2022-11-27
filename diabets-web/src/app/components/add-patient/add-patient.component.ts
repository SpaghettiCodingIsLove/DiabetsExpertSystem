import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AddPatientRequest } from 'src/app/classes/Patient';
import { DiabetsApiService } from 'src/app/services/diabets-api.service';

@Component({
  selector: 'app-add-patient',
  templateUrl: './add-patient.component.html',
  styleUrls: ['./add-patient.component.scss']
})
export class AddPatientComponent implements OnInit {

  constructor(private router: Router, private apiService: DiabetsApiService) { }

  ngOnInit(): void {
  }

  public addPatient(name: string, lastName: string, pesel: string, birthDate: string): void {
    let request: AddPatientRequest = {
      name: name,
      lastName: lastName,
      pesel: pesel,
      birthDate: new Date(birthDate)
    };

    this.apiService.addPatient(request)
      .subscribe(x => this.router.navigate(['patient'], { state: { patient: x } }));
  }
}
