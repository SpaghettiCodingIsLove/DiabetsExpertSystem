import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminGuard } from './admin.guard';
import { AuthGuard } from './auth.guard';
import { AddDoctorComponent } from './components/add-doctor/add-doctor.component';
import { AddPatientComponent } from './components/add-patient/add-patient.component';
import { LoginComponent } from './components/login/login.component';
import { MenuComponent } from './components/menu/menu.component';
import { PasswordComponent } from './components/password/password.component';
import { PatientsComponent } from './components/patients/patients.component';
import { TrainComponent } from './components/train/train.component';
import { LoggedGuard } from './logged.guard';

const routes: Routes = [
  {
    path: 'login',
    component: LoginComponent,
    canActivate: [LoggedGuard]
  },
  {
    path: 'menu',
    component: MenuComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'change-password',
    component: PasswordComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'add-patient',
    component: AddPatientComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'add-doctor',
    component: AddDoctorComponent,
    canActivate: [AdminGuard]
  },
  {
    path: 'train',
    component: TrainComponent,
    canActivate: [AdminGuard]
  },
  {
    path: 'patients',
    component: PatientsComponent,
    canActivate: [AuthGuard]
  },
  {
    path: '**',
    redirectTo: 'login',
    pathMatch: 'full'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
