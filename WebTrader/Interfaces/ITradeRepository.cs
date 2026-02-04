using Trader.objects;
using WebTrader.Repositories;

namespace WebTrader.Interfaces
{
    public interface ITradeRepository
    {
        List<OuterOrder> CalculateTrade(string action, float amount);
    }
}
