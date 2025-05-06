import { Component,  OnInit } from '@angular/core';
import { ApartametRes } from '../../Model/ApartamentRes';
import { ApartamentService } from '../../services/apartament-service.service';
@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent implements OnInit {
  public apartments: ApartametRes[] = [];
  constructor(private apartamentService: ApartamentService) {}

  ngOnInit() {
    this.apartamentService.getApartamets().subscribe(
      (res) => {
        if (Array.isArray(res)) {
          this.apartments = res;
        } else {
          console.error('Invalid API response. Expected an array.');
        }
      },
      (error) => {
        console.error('Error fetching apartamets:', error);
      }
    );
  }
}
