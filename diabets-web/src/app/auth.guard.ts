import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { DiabetsApiService } from './services/diabets-api.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private router: Router, private apiService: DiabetsApiService) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
      this.apiService.validateToken()
        .subscribe({
          error: () => { 
            localStorage.clear();
            this.router.navigate(['login']);
          }
        });

      if(localStorage.getItem('id_token') == null) {
        this.router.navigate(['login']);
        return false;
      }

      return true;
  }
  
}
