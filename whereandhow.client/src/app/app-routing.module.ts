import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './componetes/login/login.component';
import { RegisterComponent } from './componetes/register/register.component';
import { HomeComponent } from './componetes/home/home.component';
import { CreateapartamentComponent } from './componetes/createapartament/createapartament.component';
import { UserProfileComponent } from './componetes/user-profile/user-profile.component';
import { ApartmentSearchComponent } from './componetes/apartment-search/apartment-search.component';
import { CurrentApartamentComponent } from './componetes/current-apartament/current-apartament.component';
import {AboutPageComponent} from "./componetes/about-page/about-page.component";
import { AuthGuard } from './guard/auth-guard.guard';

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'create-apartment', component: CreateapartamentComponent, canActivate: [AuthGuard] },
  { path: 'userprofile', component: UserProfileComponent, canActivate: [AuthGuard] },
  { path: 'search-apartment', component: ApartmentSearchComponent },
  { path: '', component: AboutPageComponent },
  { path: 'current-apartament', component: CurrentApartamentComponent },
  { path: 'about', component: AboutPageComponent},
  { path: 'all-apartments', component: HomeComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
