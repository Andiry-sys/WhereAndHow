import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { switchMap } from 'rxjs';
import { ApartametRes } from '../../Model/apartamentRes';
import { SearchApartament } from '../../Model/searchApartament';
import { ApartamentService } from '../../services/apartament-service.service';

@Component({
    selector: 'app-home',
    templateUrl: './home.component.html',
    styleUrls: ['./home.component.css'],
    standalone: false
})
export class HomeComponent implements OnInit {
  public apartments: ApartametRes[] = [];

  constructor(
    private apartamentService: ApartamentService,
    private route: ActivatedRoute,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.route.queryParams
      .pipe(
        switchMap(params => {
          if (Object.keys(params).length > 0) {
            const searchParams: SearchApartament = {
              name:     params['name']     || '',
              minValue: Number(params['minValue']) || 0,
              maxValue: Number(params['maxValue']) || 1000,
              city:     params['city']     || '',
              typeRoom: params['typeRoom'] || '',
            };
            return this.apartamentService.getSearchApartament(searchParams);
          }
          return this.apartamentService.getApartamets();
        })
      )
      .subscribe({
        next: (res) => {
          this.apartments = res;
          this.cdr.detectChanges();
          console.log('From back-end:', res);
        },
        error: (err) => console.error('Fetch/Search error', err)
      });
  }
}
