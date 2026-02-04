using System.Text.Json;
using WebTrader.Interfaces;
using WebTrader.Repositories;

namespace WebTrader.Tests
{
    public class Test
    {
        private readonly ITradeRepository _tradeRepository;
        private readonly string expectedValueTestSell = "[ { \"order\": { \"id\": null, \"time\": \"0001-01-01T00:00:00\", \"type\": \"Buy\", \"kind\": \"Limit\", \"amount\": 0.01, \"price\": 2966.95 } }, { \"order\": { \"id\": null, \"time\": \"0001-01-01T00:00:00\", \"type\": \"Buy\", \"kind\": \"Limit\", \"amount\": 0.01, \"price\": 2966.95 } }, { \"order\": { \"id\": null, \"time\": \"0001-01-01T00:00:00\", \"type\": \"Buy\", \"kind\": \"Limit\", \"amount\": 0.009999999329447744, \"price\": 2966.95 } }]";
        private readonly string expectedValueTestBuy = "[ { \"order\": { \"id\": null, \"time\": \"0001-01-01T00:00:00\", \"type\": \"Sell\", \"kind\": \"Limit\", \"amount\": 0.6700000166893005,\"price\": 2955.03}}]";
        private readonly string? expectedValueTestNegativeAmount = null;

        public Test(TradeRepository tradeRepository)
        {
            _tradeRepository = tradeRepository;
        }

        private void TestSell()
        {
            string result = JsonSerializer.Serialize(_tradeRepository.CalculateTrade("seLL", 0.03));
            if (result == expectedValueTestSell)
            {
                Console.WriteLine("TestBuy - Success");
            }
            else
            {
                Console.WriteLine("TestBuy - Failed");
            }
        }

        private void TestBuy()
        {
            string result = JsonSerializer.Serialize(_tradeRepository.CalculateTrade("BuY", 0.67));
            if (result == expectedValueTestBuy)
            {
                Console.WriteLine("TestBuy - Success");
            }
            else
            {
                Console.WriteLine("TestBuy - Failed");
            }
        }

        private void TestNegativeAmount()
        {
            string result = JsonSerializer.Serialize(_tradeRepository.CalculateTrade("BuY", -2));
            if (result == expectedValueTestNegativeAmount)
            {
                Console.WriteLine("TestNegativeAmount - Success");
            }
            else
            {
                Console.WriteLine("TestNegativeAmount - Failed");
            }
        }
    }
}
