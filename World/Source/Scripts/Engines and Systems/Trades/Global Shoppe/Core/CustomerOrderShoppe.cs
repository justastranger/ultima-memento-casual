using System;
using System.Collections.Generic;

namespace Server.Engines.GlobalShoppe
{
	[SkipSerializeReq]
    public abstract class CustomerOrderShoppe<TOrderContext> : CustomerShoppe, IOrderShoppe
        where TOrderContext : class, IOrderContext
    {
        protected CustomerOrderShoppe(Serial serial) : base(serial)
        {
        }

        protected CustomerOrderShoppe(int itemId) : base(itemId)
        {
        }

        public void CompleteOrder(int index, Mobile from, TradeSkillContext context)
        {
            if (context.Orders.Count <= index) return;

            var order = context.Orders[index];
            if (!order.IsComplete) return;

            context.Gold += order.GoldReward;
            context.Points += order.PointReward;
            context.Reputation = Math.Min(ShoppeConstants.MAX_REPUTATION, context.Reputation + order.ReputationReward);
            context.Orders.Remove(order);

            from.PlaySound(0x32); // Dropgem1
        }

        public string GetDescription(IOrderContext order)
        {
            var typed = order as TOrderContext;
            if (typed == null) return "invalid_order";

            return GetDescription(typed);
        }

        public void OpenOrderGump(int index, Mobile from, TradeSkillContext context)
        {
            if (context.Orders.Count <= index) return;

            var order = context.Orders[index];
            if (order.IsComplete) return;

            from.CloseGump(typeof(OrderGump));
            from.SendGump(new OrderGump(from, order));
        }

        public void RejectOrder(int index, TradeSkillContext context)
        {
            if (context.Orders.Count <= index) return;

            var order = context.Orders[index];
            context.Reputation = Math.Max(0, context.Reputation - order.ReputationReward);

            context.Orders.Remove(order);
        }

        protected abstract IEnumerable<TOrderContext> CreateOrders(Mobile from, TradeSkillContext context, int amount);

        protected abstract string GetDescription(TOrderContext order);

        protected override void OpenGump(Mobile from, TradeSkillContext context)
        {
            if (context.FeePaid && context.CanRefreshOrders)
            {
                var count = ShoppeConstants.MAX_ORDERS - context.Orders.Count;
                foreach (var order in CreateOrders(from, context, count))
                {
                    context.Orders.Add(order);
                }

                context.CanRefreshOrders = false;
                context.NextOrderRefresh = DateTime.UtcNow.Add(ShoppeConstants.ORDER_REFRESH_DELAY);
            }

            base.OpenGump(from, context);
        }
    }
}