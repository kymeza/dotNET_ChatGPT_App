import { bootstrapApplication } from '@angular/platform-browser';
import { provideHttpClient } from '@angular/common/http';  // Import provideHttpClient
import { AppComponent } from './app/app.component';  // Root component
import { appConfig } from './app/app.config';  // Import ApplicationConfig

bootstrapApplication(AppComponent, {
  providers: [
    provideHttpClient(),  // Globally provide HttpClient
    ...appConfig.providers  // Existing providers from appConfig
  ]
})
  .catch(err => console.error(err));
