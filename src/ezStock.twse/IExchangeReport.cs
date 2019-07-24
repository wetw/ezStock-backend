using Refit;
using System.Threading.Tasks;

namespace ezStock.twse
{
    public interface IExchangeReport
    {
        // https://www.twse.com.tw/exchangeReport/MI_INDEX?response=json&date=20190709&type=ALL
        [Get("/exchangeReport/MI_INDEX")]
        Task<object> GetAsync([Query]string date, [Query]string type = "ALL", [Query]string response = "json");
    }
}