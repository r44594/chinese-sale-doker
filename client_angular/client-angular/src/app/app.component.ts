import { Component } from '@angular/core';
import { HomeComponent } from './components/home/home.component';
import { RouterOutlet } from '@angular/router';
import { ToastModule } from 'primeng/toast';


@Component({
  selector: 'app-root',
  imports: [RouterOutlet, HomeComponent,ToastModule], 

  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'client-angular';
}
