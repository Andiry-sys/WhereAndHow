import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { LabelType } from 'ng5-slider';
import { SearchApartament } from '../../Model/SearchApartament';

@Component({
    selector: 'app-apartment-search',
    templateUrl: './apartment-search.component.html',
    styleUrls: ['./apartment-search.component.css'],
    standalone: false
})
export class ApartmentSearchComponent {
  searchApartament: SearchApartament = {
    name: '',
    minValue: 10,
    maxValue: 1000,
    city: '',
    typeRoom: '',
  };

  apartmentTypes = ['Студія', 'Одна спальня', 'Дві спальні', 'Три спальні'];

  sliderOptions = {
    floor: 10,
    ceil: 10000,
    translate: (value: number, label: LabelType): string => {
      switch (label) {
        case LabelType.Low:
          return '<b>Ціна від:</b> ₴' + value;
        case LabelType.High:
          return '<b>Ціна до:</b> ₴' + value;
        default:
          return '₴' + value;
      }
    },
  };

  constructor(private router: Router) { }

  onSubmit() {
    this.router.navigate(['all-apartments'], {
      queryParams: this.searchApartament,
    });
  }
}
