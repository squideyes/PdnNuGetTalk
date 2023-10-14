// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using Ardalis.GuardClauses;
using PdnNuGetTalk.NuGetDemo.Contracts.Models;
using System.Collections;

namespace PdnNuGetTalk.StatelessDemo.Contracts;

public class SkuSet : IEnumerable<Sku>
{
    private readonly Dictionary<SkuId, Sku> skus = new();

    public void Add(Sku sku)
    {
        Guard.Against.Null(sku);

        skus.Add(sku.SkuId, sku);
    }

    public bool Contains(SkuId skuId) => skus.ContainsKey(skuId);

    public Sku this[SkuId skuId] => skus[skuId];

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<Sku> GetEnumerator() => skus.Values.GetEnumerator();
}