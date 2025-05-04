export interface ApartametRes {
  id: string;
  apartamentName: string;
  apartamentPrice: number;
  apartamentTypeRoom: string;
  photos: { photoImagePath: string }[];
  addressStreet: string;
  addressCity: string;
  addressNumberHouse: string;
  description:string;
}
