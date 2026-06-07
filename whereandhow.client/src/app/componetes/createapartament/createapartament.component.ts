import { Observable } from 'rxjs';
import { AddressService } from './../../services/address.service';
import { ApartamentService } from './../../services/apartament-service.service';
import { Component, OnInit } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { Addresses } from '../../Model/Addresses';
import {FormControl, FormGroup, Validators} from '@angular/forms';
import { Router } from '@angular/router';
import { UpdateUserprofileService } from '../../services/update-userprofile.service';
import { Apartamet } from '../../Model/Apartament';
import { AiService } from '../../services/ai.service';
import { AIAnalyzeResponse } from '../../Model/aiAnalyzeResponse';

@Component({
    selector: 'app-createapartament',
    templateUrl: './createapartament.component.html',
    styleUrls: ['./createapartament.component.css'],
    standalone: false
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
  public aiLoading: boolean = false;
  public aiResult: AIAnalyzeResponse | null = null;

  constructor(
    private apartmentService: ApartamentService,
    private addressServices: AddressService,
    private router: Router,
    private userService: UpdateUserprofileService,
    private aiService: AiService
  ) {}

  public improveDescription(): void {
    const current = this.description.value as string;
    if (!current || !current.trim()) {
      alert('Введіть опис перед покращенням');
      return;
    }

    // Resolve the selected address object to extract city and street.
    const selectedAddress = this.addresses?.find(
      a => a.id === (this.addressId.value as string)
    );

    this.aiLoading = true;
    this.aiResult = null;

    this.aiService.analyzeDescription(current, {
      name:     (this.name.value     as string) || undefined,
      price:    (this.price.value    as number) || undefined,
      typeRoom: (this.typeRoom.value as string) || undefined,
      city:     selectedAddress?.city           || undefined,
      street:   selectedAddress?.street         || undefined,
    }).subscribe({
      next: (res: AIAnalyzeResponse) => {
        this.aiResult = res;
        this.apartmentForm.patchValue({ description: res.improvedDescription });
        this.aiLoading = false;
      },
      error: (err: HttpErrorResponse) => {
        console.error('AI analyze error:', err);
        this.aiLoading = false;
        alert('Не вдалося покращити опис. Спробуйте пізніше.');
      },
    });
  }

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

    // Initialize controls with the same defaults the `apartament` object used,
    // so the form's initial state matches the previous ngModel-bound behavior.
    this.apartmentForm = new FormGroup({
      name: new FormControl(''),
      price: new FormControl(0),
      images: new FormControl(),
      addressId: new FormControl(''),
      typeRoom: new FormControl(''),
      description: new FormControl('')
    });
  }

  onFilesSelected(event: Event): void {
    const files = (event.target as HTMLInputElement).files;

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
      // Read text/select values from the reactive form; images come from the
      // file picker (onFilesSelected) and ownerId from the loaded user.
      const value = this.apartmentForm.value;
      const formData = new FormData();
      formData.append('name', value.name);
      formData.append('price', value.price.toString());
      formData.append('typeRoom', value.typeRoom);
      for (let i = 0; i < this.apartament.images.length; i++) {
        formData.append('images', this.apartament.images[i]);
      }
      formData.append('addressId', value.addressId);
      formData.append('ownerId', this.ownerId);
      formData.append('description', value.description)
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
