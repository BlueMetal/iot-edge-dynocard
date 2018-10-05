import { NgModule } from '@angular/core';
import { AppComponent } from './app.component';

// Components
import { DynoCardComponent } from './dynocard/dynocard.component';

// Modules
import { CoreModule } from '../modules/core.module';

@NgModule({
  declarations: [
    AppComponent,
    DynoCardComponent
  ],
  imports: [
    CoreModule.forRoot()
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
