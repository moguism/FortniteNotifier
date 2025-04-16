import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { Wishlist } from '../models/wishlist';

@Injectable({
  providedIn: 'root'
})
export class WishlistService {

  constructor(public api: ApiService) { }

  async createWishlist(wishlist: Wishlist): Promise<any> {
    const result = await this.api.post(`Wishlist`, wishlist);
    return result.data
  }

  async deleteWishlist(wishlistId: number): Promise<any> {
    const result = await this.api.delete(`Wishlist/${wishlistId}`);
    return result.data
  }
}
