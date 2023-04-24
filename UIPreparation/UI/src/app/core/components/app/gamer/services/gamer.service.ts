import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Gamer } from '../models/Gamer';
import { environment } from 'environments/environment';


@Injectable({
  providedIn: 'root'
})
export class GamerService {

  constructor(private httpClient: HttpClient) { }


  getGamerList(): Observable<Gamer[]> {

    return this.httpClient.get<Gamer[]>(environment.getApiUrl + '/gamers/getall')
  }

  getGamerById(id: number): Observable<Gamer> {
    return this.httpClient.get<Gamer>(environment.getApiUrl + '/gamers/getbyid?id='+id)
  }

  addGamer(gamer: Gamer): Observable<any> {

    return this.httpClient.post(environment.getApiUrl + '/gamers/', gamer, { responseType: 'text' });
  }

  updateGamer(gamer: Gamer): Observable<any> {
    return this.httpClient.put(environment.getApiUrl + '/gamers/', gamer, { responseType: 'text' });

  }

  deleteGamer(id: number) {
    return this.httpClient.request('delete', environment.getApiUrl + '/gamers/', { body: { id: id } });
  }


}