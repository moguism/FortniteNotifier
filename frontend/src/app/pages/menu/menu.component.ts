import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from '../../services/user.service';
import { User } from '../../models/user';
import { FormsModule } from '@angular/forms';
import { Wishlist } from '../../models/wishlist';
import { WishlistService } from '../../services/wishlist.service';
import { NavbarComponent } from "../../components/navbar/navbar.component";

@Component({
  selector: 'app-menu',
  imports: [FormsModule, NavbarComponent],
  templateUrl: './menu.component.html',
  styleUrl: './menu.component.css'
})
export class MenuComponent implements OnInit {

  constructor(private userService: UserService, private wishlistService: WishlistService, private router: Router){}

  user: User | null = null

  itemName: string = ""

  alreadyInShop = false

  async ngOnInit() {
    await this.getUser()
  }

  async createNewWishList()
  {
    const wishlist: Wishlist = 
    {
      id: 0,
      item: this.itemName
    }

    this.alreadyInShop = await this.wishlistService.createWishlist(wishlist)
    await this.getUser()
  }

  async deleteWishlist(wishlistId: number)
  {
    await this.wishlistService.deleteWishlist(wishlistId)
    await this.getUser()
  }

  async getUser()
  {
    this.user =  await this.userService.getAuthorizeUser()
    if(this.user == null)
    {
      this.router.navigateByUrl("login")
    }
  }
}
