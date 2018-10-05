// url-managing.service.ts

/**
 * This class manages URL building for API calls in components.
 */
import { Injectable } from '@angular/core';

const prodUrl = 'http://192.168.1.239:8201';
const hostName: string = prodUrl

@Injectable()
export class UrlManagingService {

  // These are all site root relative
  baseApiRoute: string = hostName + '/api';
  getDynoCardData: string = this.baseApiRoute + '/DynoCard';

}
