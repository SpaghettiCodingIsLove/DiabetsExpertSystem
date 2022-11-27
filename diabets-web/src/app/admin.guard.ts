import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AdminGuard implements CanActivate {
  constructor(private router: Router) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
      
      const isAdmin :boolean = JSON.parse(localStorage.getItem("isAdmin") ?? "false");

      if(localStorage.getItem('id_token') == null) {
        this.router.navigate(['login'])
        return false
      }
      else if(!isAdmin) {
        this.router.navigate(['menu'])
        return false
      }

      return true;
  }
  
}
