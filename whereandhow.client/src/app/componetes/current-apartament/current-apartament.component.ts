import { OrderService } from './../../services/order.service';
import { Component } from '@angular/core';
import { ActivatedRoute, ParamMap, Route, Router } from '@angular/router';
import { ApartametRes } from '../../model/apartamentRes';
import { OrderDTO } from '../../model/orderDTO';
import { ApartamentService } from '../../services/apartament-service.service';
import { AuthService } from '../../services/auth.service';
import { UpdateUserprofileService } from '../../services/update-userprofile.service';
import {FormControl, FormGroup} from "@angular/forms";

@Component({
  selector: 'app-current-apartament',
  templateUrl: './current-apartament.component.html',
  styleUrls: ['./current-apartament.component.css'],
})
export class CurrentApartamentComponent {
  public currentAppartament!: FormGroup;
  public apartament: ApartametRes = {
    id: '',
    apartamentName: '',
    apartamentPrice: 0,
    addressCity: '',
    addressNumberHouse: '',
    addressStreet: '',
    apartamentTypeRoom: '',
    photos: [],
    description:''
  };
  public order: OrderDTO = {
    CheckOutDate: '',
    CheckInDate: '',
    apartamentId: '',
    userId: '',

  };
  constructor(
    private route: ActivatedRoute,
    private apartamentService: ApartamentService,
    private userService: UpdateUserprofileService,
    private orderService:OrderService,
    private authService: AuthService,
    private navigator: Router
  ) {}

  ngOnInit() {
    this.route.queryParams.subscribe((params) => {
      this.apartamentService
        .getApartamentById(params['id'])
        .subscribe((res) => {
          this.apartament = res;
          this.order.apartamentId = this.apartament.id;
        });
    });

    this.userService.getUser().subscribe((res) => {
      this.order.userId = res.id;
    });
  }

  orderApartament() {

    if(this.authService.isUserAuthenticated()){
      if (!this.order.CheckInDate || !this.order.CheckOutDate) {
        alert("Ви не вибрали дату в'їзду або виїзду")
      }
      const checkInDate = new Date(this.order.CheckInDate);
      const checkOutDate = new Date(this.order.CheckOutDate);

      if (checkInDate >= checkOutDate) {
        alert("Дата заїзду пізніша або співпадає з датою виїзду");       
      }
    this.orderService.sendOrder(this.order).subscribe(()=>{
      window.alert('Апартамент замовлено повідомлення буде надіслано на вашу електронну скиньку');
      this.navigator.navigate(['']);
    },
    (error)=>{
      console.log(error);

    })
  }
  else{
    alert('Ви повинні бути залогіненим');
    this.navigator.navigate(['login'])
  }
  }

  getCompleteImageUrl(imageUrl: string): string {
    const baseUrl = 'https://localhost:7205/';
    return `${baseUrl}/${imageUrl}`;
  }

}
