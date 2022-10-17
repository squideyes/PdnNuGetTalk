// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using Ardalis.GuardClauses;
using PdnNuGetTalk.StatelessDemo.Helpers;

namespace PdnNuGetTalk.StatelessDemo.Contracts;

public class Order
{
    public required DateTime CreatedOn { get; init; }
    public required string CustomerId { get; init; }
    public Dictionary<SkuId, int>? Items { get; init; }

    public static Order From(string customerId)
    {
        Guard.Against.InvalidInput(customerId,
            nameof(customerId), v => v.IsEmailAddress());

        return new Order()
        {
            CreatedOn = DateTime.UtcNow,
            CustomerId = customerId,
            Items = new Dictionary<SkuId, int>()
        };
    }
}