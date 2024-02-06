import { bootstrapApplication } from '@angular/platform-browser';
import { AppComponent } from './app/app.component';
import { config } from './app/app.config.server';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
import { AppModule } from './app/app.module';
import { ServerComponent } from './app/server/server.component';

const bootstrap = () => bootstrapApplication(ServerComponent, config);

export default bootstrap;
