import { OrderService } from './../../services/order.service';
import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { switchMap } from 'rxjs';
import { FormControl, FormGroup } from '@angular/forms';
import { ApartametRes } from '../../Model/apartamentRes';
import { OrderDTO } from '../../Model/orderDTO';
import { ApartamentService } from '../../services/apartament-service.service';
import { AuthService } from '../../services/auth.service';
import { UpdateUserprofileService } from '../../services/update-userprofile.service';

@Component({
    selector: 'app-current-apartament',
    templateUrl: './current-apartament.component.html',
    styleUrls: ['./current-apartament.component.css'],
    standalone: false
})
export class CurrentApartamentComponent implements OnInit {
  public photos: { imagePath: string }[] = [];
  public selectedPhotoIndex: number = 0;

  apartmentForm = new FormGroup({
    name:        new FormControl(''),
    price:       new FormControl(0),
    typeRoom:    new FormControl(''),
    description: new FormControl(''),
    city:        new FormControl(''),
    street:      new FormControl(''),
    numberHouse: new FormControl(''),
  });

  public order: OrderDTO = {
    CheckOutDate: '',
    CheckInDate:  '',
    apartamentId: '',
    userId:       '',
  };

  constructor(
    private route:             ActivatedRoute,
    private apartamentService: ApartamentService,
    private userService:       UpdateUserprofileService,
    private orderService:      OrderService,
    private authService:       AuthService,
    private navigator:         Router,
    private cdr:               ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.route.queryParams
      .pipe(switchMap(params => this.apartamentService.getApartamentById(params['id'])))
      .subscribe({
        next: (res: ApartametRes) => {
          this.photos = res.photos;
          this.order.apartamentId = res.id;

          this.apartmentForm.patchValue({
            name:        res.name,
            price:       res.price,
            typeRoom:    res.typeRoom,
            description: res.description,
            city:        res.address?.city        ?? '',
            street:      res.address?.street      ?? '',
            numberHouse: res.address?.numberHouse ?? '',
          });

          this.cdr.detectChanges();
          console.log('Apartment model:', res);
        },
        error: (err) => {
          console.error('[CurrentApartament] Failed to load apartment:', err);
        }
      });

    this.userService.getUser().subscribe({
      next: (res) => {
        this.order.userId = res.id;
      },
      error: (err) => {
        console.error('[CurrentApartament] Failed to load user:', err);
      }
    });
  }

  public selectPhoto(index: number): void {
    this.selectedPhotoIndex = index;
  }

  orderApartament(): void {
    if (this.authService.isLoggedIn()) {
      if (!this.order.CheckInDate || !this.order.CheckOutDate) {
        alert("Ви не вибрали дату в'їзду або виїзду");
        return;
      }
      const checkInDate  = new Date(this.order.CheckInDate);
      const checkOutDate = new Date(this.order.CheckOutDate);

      if (checkInDate >= checkOutDate) {
        alert("Дата заїзду пізніша або співпадає з датою виїзду");
        return;
      }

      this.orderService.sendOrder(this.order).subscribe({
        next: () => {
          window.alert('Апартамент замовлено повідомлення буде надіслано на вашу електронну скиньку');
          this.navigator.navigate(['']);
        },
        error: (error) => {
          console.error('[CurrentApartament] Order failed:', error);
        }
      });
    } else {
      alert('Ви повинні бути залогіненим');
      this.navigator.navigate(['login']);
    }
  }
}
