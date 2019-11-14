namespace WE.Domain
{
    public class StockInfo
    {
        public StockInfo(string code)
        {
            Code = code;
        }

        public virtual string Code { get; }
        public virtual string Name { get; }
        public virtual string FullName { get; }
        public virtual string Category { get; }
        public virtual string Address { get; }
        public virtual string Phone { get; }
        public virtual string Url { get; }
    }
}