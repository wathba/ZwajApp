import { Photo } from "./photo";

export interface User {
 id: number;
 userName: string;
 age: string;
 gender: string;
 kownAs: string;
 created: Date;
 lastActive: Date;
 introduction?: string;
 lookingFor?: string;
 interests?: string;
 city: string; 
 country: string;
 photoUrl: string;
 photos: Photo[];
 roles:string[]
}
