// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using Ardalis.GuardClauses;
using PdnNuGetTalk.StatelessDemo.Contracts;
using PdnNuGetTalk.StatelessDemo.Helpers;
using Stateless;
using Stateless.Graph;
using System.Text;

namespace PdnNuGetTalk.StatelessDemo.Shopping;

public class Cart
{
    private readonly Dictionary<SkuId, int> items = new();

    private readonly string customerId;
    private readonly SkuSet skus;
    private readonly StateMachine<State, Trigger> machine;

    private readonly StateMachine<State, Trigger>
        .TriggerWithParameters<(SkuId SkuId, int Quantity)> addItemTrigger;

    private readonly StateMachine<State, Trigger>
        .TriggerWithParameters<(SkuId SkuId, int Quantity)> updateItemTrigger;

    private readonly StateMachine<State, Trigger>
        .TriggerWithParameters<SkuId> removeItemTrigger;

    public event EventHandler<PlaceOrderArgs>? OnPlaceOrder;

    public Cart(SkuSet skus, string customerId)
    {
        this.skus = Guard.Against.Null(skus);

        this.customerId = Guard.Against.NullOrInvalidInput(
            customerId, nameof(customerId), c => c.IsEmailAddress());

        machine = new(State.Waiting);

        machine.OnTransitionCompleted(t => Console.WriteLine(GetMessage(t)));

        addItemTrigger = machine
            .SetTriggerParameters<(SkuId SkuId, int Quantity)>(Trigger.Add);

        updateItemTrigger = machine
            .SetTriggerParameters<(SkuId SkuId, int Quantity)>(Trigger.Update);

        removeItemTrigger = machine.SetTriggerParameters<SkuId>(Trigger.Remove);

        machine.Configure(State.Waiting)
            .Permit(Trigger.Clear, State.ClearItems)
            .PermitIf(Trigger.Submit, State.PlaceOrder, () => items.Count > 0)
            .Permit(Trigger.Add, State.AddItem)
            .Permit(Trigger.Update, State.UpdateItem)
            .PermitIf(Trigger.Remove, State.RemoveItem, () => items.Count > 0);

        machine.Configure(State.AddItem)
            .Permit(Trigger.Wait, State.Waiting)
            .OnEntryFrom(addItemTrigger, t =>
            {
                items.Add(t.SkuId, t.Quantity);

                machine.Fire(Trigger.Wait);
            })
            .OnExit(() => Console.WriteLine("Item Added"));

        machine.Configure(State.UpdateItem)
            .Permit(Trigger.Wait, State.Waiting)
            .OnEntryFrom(updateItemTrigger, t =>
            {
                items[t.SkuId] = t.Quantity;

                machine.Fire(Trigger.Wait);
            })
            .OnExit(() => Console.WriteLine("Item Updated"));

        machine.Configure(State.RemoveItem)
            .Permit(Trigger.Wait, State.Waiting)
            .OnEntryFrom(removeItemTrigger, skuId =>
            {
                items.Remove(skuId);

                machine.Fire(Trigger.Wait);
            })
            .OnExit(() => Console.WriteLine("Item Removed"));

        machine.Configure(State.ClearItems)
            .Permit(Trigger.Wait, State.Waiting)
            .OnEntry(() =>
            {
                items.Clear();

                machine.Fire(Trigger.Wait);
            })
            .OnExit(() => Console.WriteLine("Cart Cleared"));

        machine.Configure(State.PlaceOrder)
            .Permit(Trigger.Clear, State.ClearItems)
            .OnEntry(() =>
            {
                var order = new Order()
                {
                    CustomerId = customerId,
                    CreatedOn = DateTime.UtcNow,
                    Items = items
                };

                OnPlaceOrder?.Invoke(this, new PlaceOrderArgs { Order = order });

                machine.Fire(Trigger.Clear);
            })
            .OnExit(() => Console.WriteLine("Order Placed"));

        Console.WriteLine(UmlDotGraph.Format(machine.GetInfo()));
        Console.WriteLine();
    }

    public void Add(SkuId skuId, int quantity)
    {
        AssertSkuIdIsKnown(skuId);

        Guard.Against.NegativeOrZero(quantity);

        machine.Fire(addItemTrigger, (skuId, quantity));
    }

    public void Update(SkuId skuId, int quantity)
    {
        AssertSkuIdIsKnown(skuId);

        Guard.Against.NegativeOrZero(quantity);

        machine.Fire(updateItemTrigger, (skuId, quantity));
    }

    public void Remove(SkuId skuId)
    {
        AssertSkuIdIsKnown(skuId);

        machine.Fire(removeItemTrigger, skuId);
    }

    public void Clear() => machine.Fire(Trigger.Clear);

    public void PlaceOrder() => machine.Fire(Trigger.Submit);

    private void AssertSkuIdIsKnown(SkuId skuId)
    {
        if (!skus.Contains(skuId))
            throw new ArgumentOutOfRangeException(nameof(skuId));
    }

    private static string GetMessage(StateMachine<State, Trigger>.Transition transition)
    {
        var sb = new StringBuilder();

        sb.Append(transition.Source);
        sb.Append(" to ");
        sb.Append(transition.Destination);
        sb.Append(" on ");
        sb.Append(transition.Trigger);

        if (transition.IsReentry || transition.Parameters.Length > 0)
        {
            sb.Append(" (");

            if (transition.IsReentry)
            {
                sb.Append("RE-ENTRY");
            }

            if (transition.Parameters.Length > 0)
            {
                if (transition.IsReentry)
                    sb.Append("; ");

                sb.Append("PARAMS: ");

                sb.Append(transition.Parameters.ToList().ToDelimitedString(x => x.ToString()!, ";", ";"));
            }

            sb.Append(')');
        }

        return sb.ToString();
    }
}