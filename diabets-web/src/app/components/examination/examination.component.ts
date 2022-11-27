import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Examination } from 'src/app/classes/Examination';
import { Patient } from 'src/app/classes/Patient';

@Component({
  selector: 'app-examination',
  templateUrl: './examination.component.html',
  styleUrls: ['./examination.component.scss']
})
export class ExaminationComponent implements OnInit {
  public patient: Patient;
  public examination: Examination;

  constructor(private router: Router) {
    this.patient = this.router.getCurrentNavigation()?.extras?.state?.['patient'];
    this.examination = this.router.getCurrentNavigation()?.extras?.state?.['examination'];

    if (this.patient == undefined || this.examination == undefined) {
      this.router.navigate(['menu']);
    }
  }

  ngOnInit(): void {
  }

}
