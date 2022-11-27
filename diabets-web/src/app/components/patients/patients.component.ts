import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Patient } from 'src/app/classes/Patient';
import { DiabetsApiService } from 'src/app/services/diabets-api.service';

@Component({
  selector: 'app-patients',
  templateUrl: './patients.component.html',
  styleUrls: ['./patients.component.scss']
})
export class PatientsComponent implements OnInit {
  public patients: Patient[] = new Array;

  constructor(private apiService: DiabetsApiService, private router: Router) { }

  ngOnInit(): void {
    this.apiService.getPatients()
      .subscribe(x => {
        this.patients = x;
      });
  }

  redirect(patient: Patient): void {
    this.router.navigate(['patient'], { state: { patient: patient } });
  }

}
