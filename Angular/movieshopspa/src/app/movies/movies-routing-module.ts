import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MoviesRoutingRoutingModule } from './movies-routing-routing-module';
import { Movies } from './movies';
import { RouterModule, Routes } from '@angular/router';
import { MovieDetails } from './movie-details/movie-details';
import { CastDetails } from './cast-details/cast-details';

const routes: Routes = [
  {
    path: '', component: Movies,  // http://localhost:4200/movies
    children: [
      {path:':id', component: MovieDetails},  // http://localhost:4200/movies/1
      {path: 'cast/:id', component: CastDetails}  // http://localhost:4200/movies/cast/1
    ]
  }
];

@NgModule({
  declarations: [],
  imports: [
    CommonModule, RouterModule.forChild(routes),
    // MoviesRoutingRoutingModule
  ],
  exports: [RouterModule]
})
export class MoviesRoutingModule { }
