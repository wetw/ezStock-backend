using System.Diagnostics;

namespace WE.Domain
{
    [DebuggerDisplay("Stock#{Info.Code} [{Info.Name}]")]
    public class Stock
    {
        public virtual StockInfo Info { get; }
        public virtual StockPrice Price { get; set; }
    }
}
