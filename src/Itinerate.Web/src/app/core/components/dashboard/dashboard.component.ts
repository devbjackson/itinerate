import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Observable } from 'rxjs';
import { Itinerary } from '../../../core/models/itinerary.model';
import { ApiService } from '../../../core/services/api.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent implements OnInit {
  private apiService = inject(ApiService);
  public itineraries$!: Observable<Itinerary[]>;

  ngOnInit(): void {
    this.itineraries$ = this.apiService.getItineraries();
  }
}