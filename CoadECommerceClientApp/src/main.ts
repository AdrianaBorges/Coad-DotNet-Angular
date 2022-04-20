import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app/app.module';
import { environment } from './environments/environment';

if (environment.production) {
  enableProdMode();
}

export function getEndPointURL() {

  if (environment) {
    return environment.endpointPath;
  }
  return '';
}

const providers = [
  { provide: 'ENDPOINT_URL', useFactory: getEndPointURL, deps: [] }
];

platformBrowserDynamic(providers).bootstrapModule(AppModule)
  .catch(err => console.error(err));
