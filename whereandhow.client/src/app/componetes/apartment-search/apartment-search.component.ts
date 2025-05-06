import { HttpParams } from '@angular/common/http';
import { SearchApartament } from './../../Model/SearchApartament';
import { ApartamentService } from './../../services/apartament-service.service';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { LabelType } from 'ng5-slider';
import { ApartametRes } from '../../Model/ApartamentRes';
@Component({
  selector: 'app-apartment-search',
  templateUrl: './apartment-search.component.html',
  styleUrls: ['./apartment-search.component.css'],
})
export class ApartmentSearchComponent implements OnInit {
  public searchForm!: FormGroup;
  public apartments: ApartametRes[] = [];
  public searchApartament: SearchApartament = {
    name: '',
    minValue: 0,
    maxValue: 1000,
    city: '',
    typeRoom: '',
  };

  public apartmentTypes = [
    'Студія',
    'Одна спальня',
    'Дві спальні',
    'Три спальні',
  ];

  constructor(
    private apartamentService: ApartamentService,
    private router: Router
  ) {}

  ngOnInit() {
    this.searchForm = new FormGroup({
      name: new FormControl(),
      guests: new FormControl(),
      price: new FormControl(),
      city: new FormControl(),
      typeRoom: new FormControl(),
    });
  }

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

  onSubmit() {
    this.apartamentService
      .getSearchApartament(this.searchApartament)
      .subscribe((res) => {
        const param = new HttpParams();
        param.append('aprtaments', res.join(', '));
        this.router.navigate([''], { queryParams: param });
      },
      (error)=>{
        alert("Not found " + error['status']+"! Поміняйте параметри пошуку !!!")
        console.error('Not found apartaments', error);

      });
  }
}
