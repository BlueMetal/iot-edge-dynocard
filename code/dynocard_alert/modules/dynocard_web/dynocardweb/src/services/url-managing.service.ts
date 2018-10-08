// url-managing.service.ts

/**
 * This class manages URL building for API calls in components.
 */
import { Injectable } from '@angular/core';
import { DataService } from './data.service';

@Injectable()
export class UrlManagingService {
  private getProdUrlFromConfigPath = '/config.json';
  private getProdUrlFromProperty = 'data_web_url';
  private prodUrl = '';
  private baseApiRoute: string;

  constructor(private dataService: DataService) {
  }

  getProdUrl() {
    return this.dataService.get(this.getProdUrlFromConfigPath).toPromise()

      .then((response) => {
        this.prodUrl = response[this.getProdUrlFromProperty];
        this.baseApiRoute = this.prodUrl + '/api';
      })

      .catch(error => {
        console.log('UrlManagingService.getProdUrl() Error');
        console.error(error);
      });
  }

  async getDynoCardData() {
    await this.getProdUrl();
    return this.baseApiRoute + '/DynoCard';
    // return '/assets/Results.json'; // for loading test data locally
  }
}
