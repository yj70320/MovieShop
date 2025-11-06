import { Component } from '@angular/core';
import { Movie } from '../core/services/movie';
import { MovieCardModel } from '../shared/models/MovieCardModel';

@Component({
  selector: 'app-home',
  standalone: false,
  templateUrl: './home.html',
  styleUrl: './home.css',
})
export class Home {
  movies !: MovieCardModel[];
  constructor(private movieService: Movie) {}
    ngOnInit(): void {
      console.log("Inside the Home Component init method  ");
      this.movieService.getTopGrossingMovies().subscribe(m => {
        this.movies = m; 
        // console.log("Movies received:", this.movies);
        console.table(this.movies);
      });
  }
}