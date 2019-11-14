using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;

namespace ezStock.twse
{
    public interface IExchangeReport
    {
        // https://www.twse.com.tw/exchangeReport/MI_INDEX?response=json&date=20190709&type=ALL
        [Get("/exchangeReport/MI_INDEX")]
        Task<object> GetAsync([Query]string date, [Query]string type = "ALL", [Query]string response = "json");

        //http://www.twse.com.tw/exchangeReport/STOCK_DAY?response=json&date=20170801&stockNo=1301
        /// <summary>
        /// 取得每月的交易量
        /// </summary>
        /// <param name="date"></param>
        /// <param name="stockNo"></param>
        /// <param name="response">json</param>
        /// <returns></returns>
        [Get("/exchangeReport/STOCK_DAY")]
        Task<twseReport> GetMonthlyTransaction([Query]string date, [Query]string stockNo, [Query]string response = "json");
    }

    public class twseReport
    {
        public string date;
        public string title;
        public IEnumerable<string> fields;
        public IEnumerable<IList<string>> data;
    }

    public enum DataDetail
    {
        /// <summary>
        /// 日期
        /// </summary>
        Date,

        /// <summary>
        /// 成交股數
        /// </summary>
        TradingVolume,

        /// <summary>
        /// 成交金額
        /// </summary>
        TurnOver,

        /// <summary>
        /// 開盤價
        /// </summary>
        OpeningPrice,

        /// <summary>
        ///  最高價
        /// </summary>
        HightestPrice,

        /// <summary>
        /// 最低價
        /// </summary>
        LowestPrice,

        /// <summary>
        /// 收盤價
        /// </summary>
        ClosePrice,

        /// <summary>
        /// 漲跌價差
        /// </summary>
        SpreadPercent,

        /// <summary>
        /// 成交筆數
        /// </summary>
        TotalCount
    }
}