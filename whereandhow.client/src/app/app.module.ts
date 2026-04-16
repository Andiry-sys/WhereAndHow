import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './componetes/login/login.component';
import { RegisterComponent } from './componetes/register/register.component';
import { HTTP_INTERCEPTORS, provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HomeComponent } from './componetes/home/home.component';
import { CreateapartamentComponent } from './componetes/createapartament/createapartament.component';
import { UserProfileComponent } from './componetes/user-profile/user-profile.component';
import { ApartmentSearchComponent } from './componetes/apartment-search/apartment-search.component';
import { Ng5SliderModule } from 'ng5-slider';
import { NavbarComponent } from './componetes/navbar/navbar.component';
import { CurrentApartamentComponent } from './componetes/current-apartament/current-apartament.component';
import { FooterComponent } from './componetes/footer/footer.component';
import {CarouselModule} from "ngx-bootstrap/carousel";
import { AboutPageComponent } from './componetes/about-page/about-page.component';
import { AuthInterceptor } from './interceptors/auth-interceptor.interceptor';

@NgModule({ declarations: [
        AppComponent,
        LoginComponent,
        RegisterComponent,
        HomeComponent,
        CreateapartamentComponent,
        UserProfileComponent,
        ApartmentSearchComponent,
        NavbarComponent,
        CurrentApartamentComponent,
        FooterComponent,
        AboutPageComponent,
    ],
    bootstrap: [AppComponent], imports: [BrowserModule,
        AppRoutingModule,
        FormsModule,
        ReactiveFormsModule,
        Ng5SliderModule,
        CarouselModule], providers: [
        { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
        provideHttpClient(withInterceptorsFromDi())
    ] })
export class AppModule {}
