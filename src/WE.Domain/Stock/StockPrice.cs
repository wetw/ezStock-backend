namespace WE.Domain
{
    public class StockPrice
    {
        public virtual double Today { get; set; }
        public virtual double Yestoday { get; set; }
        public virtual AveragePrice Average { get; set; }
    }
}