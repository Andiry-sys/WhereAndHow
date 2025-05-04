import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AboutPageComponent } from './componetes/about-page/about-page.component';
import { ApartmentSearchComponent } from './componetes/apartment-search/apartment-search.component';
import { CreateapartamentComponent } from './componetes/createapartament/createapartament.component';
import { CurrentApartamentComponent } from './componetes/current-apartament/current-apartament.component';
import { FooterComponent } from './componetes/footer/footer.component';
import { HomeComponent } from './componetes/home/home.component';
import { LoginComponent } from './componetes/login/login.component';
import { NavbarComponent } from './componetes/navbar/navbar.component';
import { RegisterComponent } from './componetes/register/register.component';
import { UserProfileComponent } from './componetes/user-profile/user-profile.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Ng5SliderModule } from 'ng5-slider';
import { CarouselModule } from "ngx-bootstrap/carousel";



@NgModule({
  declarations: [
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
    AboutPageComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    Ng5SliderModule,
    CarouselModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
