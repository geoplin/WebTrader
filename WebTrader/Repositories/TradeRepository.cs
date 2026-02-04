using System.Text.Json;
using Trader.objects;
using WebTrader.Interfaces;

namespace WebTrader.Repositories
{
    public class TradeRepository : ITradeRepository
    {
        public TradeRepository() { }

        public List<OuterOrder> CalculateTrade(string action, float amount)
        {
            string filePath = "resources/order_books_data";
            List<OuterOrder> orders = [];

            if (!VerifyInput(action, amount))
            {
                return null;
            }
            
            // Process and format the file
            List<OrderBook>? orderBooks = ProcessFile(filePath);

            // Combine and order all avaliable orders
            if (orderBooks == null)
            {
                return null;
            }

            if (action.ToLower().Equals("sell"))
            {
                orders = orderBooks
                    .SelectMany(i => i.Bids)
                    .OrderByDescending(i => i.Order.Price)
                    .ToList();

                return FindOrders(orders, amount);
            }
            else if (action.ToLower().Equals("buy"))
            {
                orders = orderBooks
                    .SelectMany(i => i.Asks)
                    .OrderBy(i => i.Order.Price)
                    .ToList();

                return FindOrders(orders, amount);
            }
            else
            {
                return null;
            }
        }

        private OuterOrder GetNextOrder(int index, List<OuterOrder> orders)
        {
            return orders.Skip(index).Take(1).First();
        }

        private List<OuterOrder> FindOrders(List<OuterOrder> orders, float amount)
        {
            int orderNumber = 0;
            List<OuterOrder> bestOrders = [];
            while (amount > 0)
            {
                OuterOrder topOrder = GetNextOrder(orderNumber, orders);

                amount -= topOrder.Order.Amount;
                if (amount > 0)
                {
                    orderNumber++;
                }
                else
                {
                    topOrder.Order.Amount += amount;
                }
                bestOrders.Add(topOrder);
            }
            return bestOrders;
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
