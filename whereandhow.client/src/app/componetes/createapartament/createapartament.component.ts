import { Observable } from 'rxjs';
import { AddressService } from './../../services/address.service';
import { ApartamentService } from './../../services/apartament-service.service';
import { Component, OnInit } from '@angular/core';
import { Addresses } from '../../Model/Addresses';
import {FormControl, FormGroup, Validators} from '@angular/forms';
import { Router } from '@angular/router';
import { UpdateUserprofileService } from '../../services/update-userprofile.service';
import { Apartamet } from '../../Model/Apartament';

@Component({
  selector: 'app-createapartament',
  templateUrl: './createapartament.component.html',
  styleUrls: ['./createapartament.component.css'],
})
export class CreateapartamentComponent implements OnInit {
  public addresses!: Addresses[];
  public apartmentForm!: FormGroup;
  public apartament: Apartamet = {
    name: '',
    price: 0,
    typeRoom: '',
    images: [],
    addressId: '',
    ownerId: '',
    description:''
  };
  private ownerId!: string;
  private isLosser!: boolean;
  public apartmentTypes = [
    'Студія',
    'Одна спальня',
    'Дві спальні',
    'Три спальні',
  ];
  public selectedFileName: string = '';

  constructor(
    private apartmentService: ApartamentService,
    private addressServices: AddressService,
    private router: Router,
    private userService: UpdateUserprofileService
  ) {}

  private existsLosser(): void {
    this.userService.getUser().subscribe((data) => {
      this.isLosser = data.isLosser;
      this.ownerId = data.id;
    });
  }

  ngOnInit() {
    this.addressServices.getAddress().subscribe((res) => {
      this.addresses = res;     
    });
    this.existsLosser();

    this.apartmentForm = new FormGroup({
      name: new FormControl(),
      price: new FormControl(),
      images: new FormControl(),
      addressId: new FormControl(),
      typeRoom: new FormControl(),
      description: new FormControl()
      
    });
  }

  onFilesSelected(event: any): void {
    const files: File[] = event.target.files;

    if (files && files.length > 0) {
      this.apartament.images = Array.from(files);
      this.selectedFileName = this.apartament.images.map(file => file.name).join(', ');
    } else {
      this.apartament.images = [];
      this.selectedFileName = '';
    }
  }
  onSubmit() {
    Object.keys(this.apartmentForm.controls).forEach(controlName => {
      this.apartmentForm.controls[controlName].markAsTouched();
    });
    if (this.apartmentForm.valid && this.isLosser) {
      const formData = new FormData();
      formData.append('name', this.apartament.name);
      formData.append('price', this.apartament.price.toString());
      formData.append('typeRoom', this.apartament.typeRoom);
      for (let i = 0; i < this.apartament.images.length; i++) {
        formData.append('images', this.apartament.images[i]);
      }
      formData.append('addressId', this.apartament.addressId);
      formData.append('ownerId', this.ownerId);
      formData.append('description',this.apartament.description)
      this.apartmentService.createApartament(formData).subscribe(() => {
        this.apartmentForm.reset();
        this.router.navigate(['all-apartments']);
      });
    } else {
      alert('Ви повинні бути нашим партнером ');
      this.router.navigate(['userprofile']);
    }
  }

  get name(): FormControl{
    return this.apartmentForm.get("name") as FormControl;
  }
  get price(): FormControl{
    return this.apartmentForm.get("price") as FormControl;
  }

  get typeRoom(): FormControl{
    return this.apartmentForm.get("typeRoom") as FormControl;
  }
  get images(): FormControl{
    return this.apartmentForm.get("images") as FormControl;
  }
  get addressId(): FormControl{
    return this.apartmentForm.get("addressId") as FormControl;
  }
  get description(): FormControl{
    return this.apartmentForm.get("description") as FormControl;
  }
}
