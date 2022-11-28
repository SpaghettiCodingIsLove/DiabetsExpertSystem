import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
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

}
