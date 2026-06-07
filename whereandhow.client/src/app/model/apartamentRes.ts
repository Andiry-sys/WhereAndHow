export interface ApartametRes {
  id: string;
  name: string;
  price: number;
  typeRoom: string;
  photos: { imagePath: string }[];
  address: {
    street: string;
    city: string;
    numberHouse: string;
  } | null;
  description: string;
}
