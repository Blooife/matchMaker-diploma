import {Component, OnInit} from '@angular/core';
import {NavigationEnd, Router, RouterOutlet} from '@angular/router';
import {HeaderComponent} from "./components/header/header.component";
import {HomeComponent} from "./components/home/home.component";
import {NgClass, NgIf} from "@angular/common";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, HeaderComponent, HomeComponent, NgIf, NgClass],
  templateUrl: 'app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit{
  showHeader = true;

  constructor(private router: Router) {

  }

  ngOnInit() {
    this.router.events.subscribe(
      (val) =>{

        if(val instanceof NavigationEnd){

          if(val.url == '/login' || val.url == '/register' || val.url == '/' || val.url == '/home'){
            this.showHeader = false;
          }
        }else{
          this.showHeader = true;
        }
      }
    )
  }
}
