// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using PdnNuGetTalk.StatelessDemo.Contracts;

namespace PdnNuGetTalk.StatelessDemo.Shopping;

public class PlaceOrderArgs : EventArgs
{
    public required Order Order { get; init; }
}