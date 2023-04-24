import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Campaign } from '../models/Campaign';
import { environment } from 'environments/environment';


@Injectable({
  providedIn: 'root'
})
export class CampaignService {

  constructor(private httpClient: HttpClient) { }


  getCampaignList(): Observable<Campaign[]> {

    return this.httpClient.get<Campaign[]>(environment.getApiUrl + '/campaigns/getall')
  }

  getCampaignById(id: number): Observable<Campaign> {
    return this.httpClient.get<Campaign>(environment.getApiUrl + '/campaigns/getbyid?id='+id)
  }

  addCampaign(campaign: Campaign): Observable<any> {

    return this.httpClient.post(environment.getApiUrl + '/campaigns/', campaign, { responseType: 'text' });
  }

  updateCampaign(campaign: Campaign): Observable<any> {
    return this.httpClient.put(environment.getApiUrl + '/campaigns/', campaign, { responseType: 'text' });

  }

  deleteCampaign(id: number) {
    return this.httpClient.request('delete', environment.getApiUrl + '/campaigns/', { body: { id: id } });
  }


}