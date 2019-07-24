using Refit;
using System.Net.Http;
using System.Threading.Tasks;

namespace ezStock.twse
{
    public class ExchangeReportServices
    {
        private readonly IExchangeReport _exchangeReport;
        private const string BaseUrl = "https://www.twse.com.tw";

        public ExchangeReportServices()
        {
            _exchangeReport = RestService.For<IExchangeReport>(BaseUrl);
        }

        public ExchangeReportServices(HttpClient httpClient)
        {
            _exchangeReport = RestService.For<IExchangeReport>(httpClient);
        }

        public async Task<object> GetAsync(string date, string type = "ALL", string response = "json")
        {
            return await _exchangeReport.GetAsync(date, type, response).ConfigureAwait(false);
        }
    }
}