import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './componetes/login/login.component';
import { RegisterComponent } from './componetes/register/register.component';
import { CreateapartamentComponent } from './componetes/createapartament/createapartament.component';
import { UserProfileComponent } from './componetes/user-profile/user-profile.component';
import { ApartmentSearchComponent } from './componetes/apartment-search/apartment-search.component';
import { AboutPageComponent } from './componetes/about-page/about-page.component';
import { CurrentApartamentComponent } from './componetes/current-apartament/current-apartament.component';
import { HomeComponent } from './componetes/home/home.component';

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'create-apartment', component: CreateapartamentComponent },
  { path: 'userprofile', component: UserProfileComponent },
  { path: 'search-apartment', component: ApartmentSearchComponent },
  { path: 'current-apartament', component: CurrentApartamentComponent },
  { path: 'about', component: AboutPageComponent },
  { path: 'all-apartments', component: HomeComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
