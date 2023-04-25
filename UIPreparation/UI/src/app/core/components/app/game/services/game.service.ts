import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Game } from '../models/Game';
import { environment } from '../../../../../../environments/environment';
import { LookUp } from 'app/core/models/lookUp';

@Injectable({
  providedIn: 'root'
})
export class GameService {

  constructor(private httpClient: HttpClient) { }


  getGameList(): Observable<Game[]> {

    return this.httpClient.get<Game[]>(environment.getApiUrl + '/games/')
  }

  getGameById(id: number): Observable<Game> {
    return this.httpClient.get<Game>(environment.getApiUrl + '/games/getbyid?id='+id)
  }

  addGame(game: Game): Observable<any> {

    return this.httpClient.post(environment.getApiUrl + '/games/', game, { responseType: 'text' });
  }

  updateGame(game: Game): Observable<any> {
    return this.httpClient.put(environment.getApiUrl + '/games/', game, { responseType: 'text' });

  }

  deleteGame(id: number) {
    return this.httpClient.request('delete', environment.getApiUrl + '/games/', { body: { id: id } });
  }


}