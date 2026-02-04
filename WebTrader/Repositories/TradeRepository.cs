using System.Text.Json;
using Trader.objects;
using WebTrader.Interfaces;

namespace WebTrader.Repositories
{
    public class TradeRepository : ITradeRepository
    {
        public TradeRepository() { }

        public string? CalculateTrade(string action, float amount)
        {
            string filePath = "resources/order_books_data";
            float endPrice = 0;
            bool sell = true;
            List<OuterOrder> askOrders = [];
            List<OuterOrder> bidOrders = [];
            List<OuterOrder>? bestOrders = [];

            if (!VerifyInput(action, amount))
            {
                return null;
            }
            
            // Process and format the file
            List<OrderBook>? orderBooks = ProcessFile(filePath);

            // Combine and order all avaliable orders
            if (orderBooks != null)
            {
                askOrders = orderBooks
                    .SelectMany(i => i.Asks)
                    .OrderBy(i => i.Order.Price)
                    .ToList();

                bidOrders = orderBooks
                    .SelectMany(i => i.Bids)
                    .OrderByDescending(i => i.Order.Price)
                    .ToList();
            }

            if (action.ToLower().Equals("sell"))
            {
                sell = true;
            }
            else if (action.ToLower().Equals("buy"))
            {
                sell = false;
            }
            else
            {
                return null;
            }

            // Calculation of buy/sell price
            OuterOrder topOrder = new();
            if (sell)
            {
                endPrice = CalculatePrice(bidOrders, bestOrders, amount);
            }
            else
            {
                endPrice = CalculatePrice(askOrders, bestOrders, amount);
            }

            // Output of the end price, number of orders and list the orders used to calculate the price
            return FormatOutput(bestOrders, endPrice);
        }

        private OuterOrder GetNextOrder(int index, List<OuterOrder> orders)
        {
            return orders.Skip(index).Take(1).First();
        }

        private float CalculatePrice(List<OuterOrder> orders, List<OuterOrder>? bestOrders, float amount)
        {
            int orderNumber = 0;
            float endPrice = 0;
            while (amount > 0)
            {
                OuterOrder topOrder = GetNextOrder(orderNumber, orders);

                amount -= topOrder.Order.Amount;
                if (amount > 0)
                {
                    endPrice += topOrder.Order.Amount * topOrder.Order.Price;
                    orderNumber++;
                }
                else
                {
                    topOrder.Order.Amount += amount;
                    endPrice += topOrder.Order.Amount * topOrder.Order.Price;
                }

                bestOrders.Add(topOrder);
            }

            return endPrice;
        }

        private List<OrderBook>? ProcessFile(string filePath)
        {
            char[] separator = ['\n', '\t'];
            string fileString = File.ReadAllText(filePath);
            List<string> orderBookLines = fileString.Split(separator).ToList();
            List<string> linesToRemove = [];
            foreach (string item in orderBookLines)
            {
                if (!item.Contains('{'))
                {
                    linesToRemove.Add(item);
                }
            }
            orderBookLines = orderBookLines.Except(linesToRemove).ToList();
            string jsonString = String.Join(",", orderBookLines);
            jsonString = "[" + jsonString + "]";
            return JsonSerializer.Deserialize<List<OrderBook>>(jsonString);
        }

        private string FormatOutput(List<OuterOrder> orders, float endPrice)
        {
            string result = string.Empty;
            result += $"End price is: {endPrice}€\n";
            result += $"Number of orders required: {orders.Count}\n";
            foreach (OuterOrder order in orders)
            {
                result += ("{\n");
                result += $"\tOrder amount: {order.Order.Amount}\n";
                result += $"\tOrder Price: {order.Order.Price}€\n";
                result += ("}\n");
            }
            return result;
        }

        private bool VerifyInput(string action, float amount)
        {
            if (!action.ToLower().Equals("buy") && !action.ToLower().Equals("sell"))
            {
                throw new Exception("Invalid action");
            }
            if (amount <= 0)
            {
                throw new Exception("Amount must be a positive number");
            }
            return true;
        }
    }
}
