import {Component, OnInit} from '@angular/core';
import {CartItem} from "../../../../core/models/CartItem";
import {ShoppingCartService} from "../../../../core/services/cartService/shopping-cart.service";

@Component({
  selector: 'app-cart-main',
  templateUrl: './cart-main.component.html',
  styleUrls: ['./cart-main.component.css']
})
export class CartMainComponent implements OnInit{

  cartItems: CartItem[] = [];

  constructor(
    private shoppingCartService: ShoppingCartService) {}

  ngOnInit(): void {
    this.updateList();
  }

  private updateList(){
    this.shoppingCartService.getCartItems().subscribe(
      cartItems => {this.cartItems = cartItems;}
    );
  }

  getTotalPrice():number{
    let totalPrice = 0;
    for (let cartItem of this.cartItems) {
      totalPrice += cartItem.Quantity! * cartItem.GamePrice!;
    }
    return totalPrice;
  }

  removeFromCart(gameKey:string) {
    this.shoppingCartService.deleteItemFromCart(gameKey).subscribe(
      () => this.updateList()
    );
  }
}
