import { Component, Input } from '@angular/core';
import { MovieCardModel } from '../../models/MovieCardModel';

@Component({
  selector: 'app-movie-card',
  standalone: false,
  templateUrl: './movie-card.html',
  styleUrl: './movie-card.css',
})
export class MovieCard {
  @Input() movieCard!: MovieCardModel;
}
