import { OrderService } from './../../services/order.service';
import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ApartametRes } from '../../Model/apartamentRes';
import { OrderDTO } from '../../Model/OrderDTO';
import { ApartamentService } from '../../services/apartament-service.service';
import { AuthService } from '../../services/auth.service';
import { UpdateUserprofileService } from '../../services/update-userprofile.service';
import { FormGroup} from "@angular/forms";
import { environment } from '../../../environments/environment';

@Component({
    selector: 'app-current-apartament',
    templateUrl: './current-apartament.component.html',
    styleUrls: ['./current-apartament.component.css'],
    standalone: false
})
export class CurrentApartamentComponent {
  public currentAppartament!: FormGroup;
  public apartament: ApartametRes = {
    id: '',
    name: '',
    price: 0,
    address: {
      city: '',
      numberHouse: '',
      street: '',
    },
    typeRoom: '',
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

    if(this.authService.isLoggedIn()){
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



}
