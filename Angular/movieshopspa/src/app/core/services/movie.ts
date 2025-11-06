import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { MovieCardModel } from '../../shared/models/MovieCardModel';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class Movie {
  constructor(private http: HttpClient) {}  

  // https://localhost:7121/api/Movies/top-grossing
  // Observable is used for async data streams，可被订阅的异步数据流 data stream 
  getTopGrossingMovies( ): Observable<MovieCardModel[]> {
    return this.http.get<MovieCardModel[]>('https://localhost:7121/api/Movies/top-grossing');
  }
  getMovieDetails(id: number) {}
  getMoviesByGenre(genreId: number) {}
}
