
import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  template: `
    <div class="navbar">
      <div class="container nav-inner">
        <div class="brand"><span class="dot"></span> Recipe-Web</div>
        <div class="nav-links">
          <a routerLink="/">Home</a>
          <a routerLink="/categories">Categories</a>
          <a routerLink="/profile">Profile</a>
          <a routerLink="/login">Login</a>
        </div>
      </div>
    </div>
    <div class="container">
      <router-outlet></router-outlet>
    </div>
    <div class="footer">© prototype</div>
  `
})
export class AppComponent {}
