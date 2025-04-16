import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { Route, Router } from '@angular/router';

@Component({
  selector: 'app-navbar',
  imports: [],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent implements OnInit {

  constructor(public authService: AuthService, public router: Router) { }

  user: string | null = ""

  ngOnInit(): void {
    const user = this.authService.getUser()
    if (user) {
      this.user = user.replace('"', "").replace('"', "")
    }
  }

  login() {
    this.router.navigateByUrl("login")
  }

  logout() {
    this.authService.logout()
    this.router.navigateByUrl("login")
  }
}
