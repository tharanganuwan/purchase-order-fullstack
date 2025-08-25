import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatToolbarModule } from '@angular/material/toolbar';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet,MatButtonModule,MatToolbarModule,RouterModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
  standalone:true
})
export class AppComponent {
  title = 'purchase-order-system';
}

