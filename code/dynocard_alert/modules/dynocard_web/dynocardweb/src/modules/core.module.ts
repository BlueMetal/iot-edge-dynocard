import {
  ModuleWithProviders,
  NgModule,
  Optional,
  SkipSelf,
  Inject
} from '@angular/core';
import { APP_BASE_HREF, CommonModule } from '@angular/common';

// libs
import { throwIfAlreadyLoaded } from '../utils/index';

// app
import { environment } from '../environments/environment';
import { CORE_PROVIDERS, LogService } from '../services';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';

/**
 * DEBUGGING
 */
LogService.DEBUG.LEVEL_4 = !environment.production;

export const BASE_PROVIDERS: any[] = [
  ...CORE_PROVIDERS,
  {
    provide: APP_BASE_HREF,
    useValue: '/'
  }
];

@NgModule({
  imports: [
    CommonModule,
    BrowserModule,
    HttpClientModule
  ],
  exports: [
    CommonModule,
    BrowserModule,
    HttpClientModule
  ]
})
export class CoreModule {
  // configuredProviders: *required to configure WindowService and others per platform
  static forRoot(configuredProviders?: Array<any>): ModuleWithProviders {
    return {
      ngModule: CoreModule,
      providers: [...BASE_PROVIDERS]
    };
  }

  constructor(
    @Optional()
    @SkipSelf()
      parentModule: CoreModule) {
    throwIfAlreadyLoaded(parentModule, 'CoreModule');
  }
}
