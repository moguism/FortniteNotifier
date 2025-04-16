import { Wishlist } from "./wishlist";

export interface User {
    id : number,
    email : string,
    name : string
    wishlists: Wishlist[]
}
