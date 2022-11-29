import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AddExaminationRequest } from 'src/app/classes/Examination';
import { Patient } from 'src/app/classes/Patient';
import { DiabetsApiService } from 'src/app/services/diabets-api.service';

@Component({
  selector: 'app-add-examination',
  templateUrl: './add-examination.component.html',
  styleUrls: ['./add-examination.component.scss']
})
export class AddExaminationComponent implements OnInit {
  public patient: Patient;

  constructor(private apiService: DiabetsApiService, private router: Router) {
    this.patient = this.router.getCurrentNavigation()?.extras?.state?.['patient'];

    if (this.patient == undefined){
      this.router.navigate(['menu']);
    }
  }

  ngOnInit(): void {
  }

  public classify(pregnanices: string, glucose: string, bloodPreasure: string, skinThickness: string, insulin: string, weight: string, height: string, pedigree: string) {
    let request: AddExaminationRequest = {
      doctorId: localStorage.getItem("id") ?? "",
      patientId: this.patient.id,
      pregnancies: pregnanices,
      glucose: glucose,
      bloodPreasure: bloodPreasure,
      skinThickness: skinThickness,
      insulin: insulin,
      weight: weight,
      height: height,
      diabetesPedigreeFunction: pedigree,
    };

    this.apiService.classify(request)
      .subscribe(x => {
        this.router.navigate(['examination'], { state: { patient: this.patient, examination: x } });
      });
  }

}
