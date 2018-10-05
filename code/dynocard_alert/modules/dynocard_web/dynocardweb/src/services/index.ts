import { LogService } from './log.service';
import { DataService } from './data.service';
import { UrlManagingService } from './url-managing.service';

export const CORE_PROVIDERS: any[] = [LogService, DataService,
  UrlManagingService];

export * from './data.service';
export * from './log.service';
export * from './url-managing.service';

