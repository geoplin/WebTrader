using System;
using System.Collections.Generic;
using System.Text;

namespace Trader.objects
{
    public class OrderBook
    {
        public DateTime AcqTime { get; set; }
        public List<OuterOrder>? Bids { get; set; }
        public List<OuterOrder>? Asks { get; set; }
    }
}
