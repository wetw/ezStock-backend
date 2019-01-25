namespace WE.Domain
{
    public class AveragePrice
    {
        /// <summary>
        /// 5 日
        /// </summary>
        public virtual double Week { get; set; }

        /// <summary>
        /// 20 日
        /// </summary>
        public virtual double Month { get; set; }

        /// <summary>
        /// 60 日
        /// </summary>
        public virtual double Season { get; set; }

        /// <summary>
        /// 120 日
        /// </summary>
        public virtual double HalfYear { get; set; }

        /// <summary>
        /// 240 日
        /// </summary>
        public virtual double Year { get; set; }
    }
}