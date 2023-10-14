// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using PdnNuGetTalk.NuGetDemo.Contracts;
using PdnNuGetTalk.StatelessDemo.Contracts;
using PdnNuGetTalk.StatelessDemo.Shopping;

var skus = new SkuSet
{
    new Sku("XXX-11111-01", "US Flag; Large", 29.95, 5),
    new Sku("XXX-11111-02", "US Flag; Small", 14.95, 31),
    new Sku("XXX-22222-01", "Mexican Flag", 23.00, 8)
};

var cart = new Cart(skus, "someone@someco.com");

cart.OnPlaceOrder += (s, e) =>
{
    // TODO: place the order
};

cart.Add(SkuId.From("XXX-11111-02"), 3);
cart.Add(SkuId.From("XXX-22222-01"), 1);
cart.Update(SkuId.From("XXX-11111-02"), 9);
cart.Remove(SkuId.From("XXX-22222-01"));
cart.Clear();
cart.Add(SkuId.From("XXX-22222-01"), 8);

cart.PlaceOrder();