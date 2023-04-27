export class CartItem{
  GameKey?: string;
  GameName?: string;
  GamePrice?: number;
  Quantity?: number;

  constructor(gameKey?: string, gameName?: string, gamePrice?: number, quantity?: number) {
    this.GameKey = gameKey;
    this.GameName = gameName;
    this.GamePrice = gamePrice;
    this.Quantity = quantity;
  }
}
