using WebTrader.Repositories;

namespace WebTrader.Interfaces
{
    public interface ITradeRepository
    {
        string? CalculateTrade(string action, float amount);
    }
}
