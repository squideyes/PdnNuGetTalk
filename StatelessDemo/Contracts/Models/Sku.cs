// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using Ardalis.GuardClauses;
using PdnNuGetTalk.StatelessDemo.Contracts;

namespace PdnNuGetTalk.NuGetDemo.Contracts;

public class Sku
{
    private int quantity = 0;

    public Sku(string skuId, string name, double price, int quantity)
    {
        SkuId = SkuId.From(skuId);
        Name = name;
        Price = Price.From(price);
        Quantity = quantity;
    }

    public SkuId SkuId { get; }
    public string Name { get; }
    public Price Price { get; }

    public int Quantity
    {
        get
        {
            return quantity;
        }
        set
        {
            var adjusted = quantity + value;

            Guard.Against.Negative(adjusted);

            quantity = adjusted;
        }
    }

    public override string ToString() => $"{Name} ({SkuId}, {quantity:N0} Items)";
}