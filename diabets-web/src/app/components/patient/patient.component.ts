import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Examination } from 'src/app/classes/Examination';
import { Patient } from 'src/app/classes/Patient';
import { DiabetsApiService } from 'src/app/services/diabets-api.service';

@Component({
  selector: 'app-patient',
  templateUrl: './patient.component.html',
  styleUrls: ['./patient.component.scss']
})
export class PatientComponent implements OnInit {
  public examinations: Examination[] = new Array;
  public patient: Patient;

  constructor(private apiService: DiabetsApiService, private router: Router) 
  { 
    this.patient = this.router.getCurrentNavigation()?.extras?.state?.['patient'];

    if (this.patient == undefined){
      this.router.navigate(['menu']);
    }
  }

  ngOnInit(): void {
    this.apiService.getPatientExaminations(history.state.patient.id)
      .subscribe(x => {
        this.examinations = x;
      });
  }

  public addExamination() {
    this.router.navigate(['add-examination'], { state: { patient: this.patient } });
  }

  public openExamination(examination: Examination) {
    this.router.navigate(['examination'], { state: { patient: this.patient, examination: examination } });
  }
}
