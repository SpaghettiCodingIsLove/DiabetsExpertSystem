import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.scss']
})
export class MenuComponent implements OnInit {
  public isAdmin: boolean = false;

  constructor(private router: Router) { }

  ngOnInit(): void {
    this.isAdmin = JSON.parse(localStorage.getItem("isAdmin") ?? "false");
  }

  public logout() {
    localStorage.clear()
    this.router.navigate(['login'])
  }

  public navigate(destination: string) {
    this.router.navigate([destination])
  }
}
