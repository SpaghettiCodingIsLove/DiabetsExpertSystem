import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import {MatButtonModule} from '@angular/material/button';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatInputModule} from '@angular/material/input';
import {MatListModule} from '@angular/material/list';
import {MatCheckboxModule} from '@angular/material/checkbox';
import {MatProgressSpinnerModule} from '@angular/material/progress-spinner';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { LoginComponent } from './components/login/login.component';
import { MenuComponent } from './components/menu/menu.component';
import { JwtInterceptor } from './jwt.interceptor';
import { AuthGuard } from './auth.guard';
import { LoggedGuard } from './logged.guard';
import { PasswordComponent } from './components/password/password.component';
import { AddPatientComponent } from './components/add-patient/add-patient.component';
import { AddDoctorComponent } from './components/add-doctor/add-doctor.component';
import { TrainComponent } from './components/train/train.component';
import { PatientsComponent } from './components/patients/patients.component';
import { AdminGuard } from './admin.guard';
import { PatientComponent } from './components/patient/patient.component';
import { ExaminationComponent } from './components/examination/examination.component';
import { AddExaminationComponent } from './components/add-examination/add-examination.component';


@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    MenuComponent,
    PasswordComponent,
    AddPatientComponent,
    AddDoctorComponent,
    TrainComponent,
    PatientsComponent,
    PatientComponent,
    ExaminationComponent,
    AddExaminationComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    HttpClientModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatListModule,
    MatCheckboxModule,
    MatProgressSpinnerModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    AuthGuard,
    LoggedGuard,
    AdminGuard
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
