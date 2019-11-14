using System;

namespace WE.Domain.Transaction
{
    public class TransactionInfo
    {
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// 成交股數
        /// </summary>
        public int TradingVolume { get; set; }

        /// <summary>
        /// 成交金額
        /// </summary>
        public int TurnOver { get; set; }

        /// <summary>
        /// 開盤價
        /// </summary>
        public decimal OpeningPrice { get; set; }

        /// <summary>
        ///  最高價
        /// </summary>
        public decimal HightestPrice { get; set; }

        /// <summary>
        /// 最低價
        /// </summary>
        public decimal LowestPrice { get; set; }

        /// <summary>
        /// 收盤價
        /// </summary>
        public decimal ClosePrice { get; set; }

        /// <summary>
        /// 漲跌價差
        /// </summary>
        public string SpreadPercent { get; set; }

        /// <summary>
        /// 成交筆數
        /// </summary>
        public int TotalCount { get; set; }
    }
}