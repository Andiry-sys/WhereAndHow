import { Component,  OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ApartametRes } from '../../Model/ApartamentRes';
import { SearchApartament } from '../../Model/SearchApartament';
import { ApartamentService } from '../../services/apartament-service.service';
@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent implements OnInit {
  public apartments: ApartametRes[] = [];
  constructor(private apartamentService: ApartamentService,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    this.route.queryParams.subscribe((params) => {
      if (Object.keys(params).length > 0) {
        const searchParams: SearchApartament = {
          name: params['name'] || '',
          minValue: Number(params['minValue']) || 0,
          maxValue: Number(params['maxValue']) || 1000,
          city: params['city'] || '',
          typeRoom: params['typeRoom'] || '',
        };
        this.apartamentService.getSearchApartament(searchParams).subscribe(
          (res) => {  this.apartments = res},
          (err) => console.error('Search error', err)
        );
      } else {
        this.apartamentService.getApartamets().subscribe(
          (res) => {  this.apartments = res },
          (err) => console.error('Fetch error', err)
        );
      }
    });
  }
}
