using System;
using System.Collections.Generic;
using System.Text;

namespace Trader.objects
{
    public class Order
    {
        public int? Id { get; set; }
        public DateTime Time { get; set; }
        public string Type { get; set; } // buy/sell
        public string Kind { get; set; } // limit/;
        public float Amount { get; set; }
        public float Price { get; set; }
    }
}
