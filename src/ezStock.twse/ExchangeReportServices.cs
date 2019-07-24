using Refit;
using System.Threading.Tasks;

namespace ezStock.twse
{
    public class ExchangeReportServices
    {
        private readonly IExchangeReport _exchangeReport;
        private const string BaseUrl = "https://www.twse.com.tw";

        public ExchangeReportServices()
        {
            //_httpClient = new HttpClient(new HttpClientDiagnosticsHandler());
            _exchangeReport = RestService.For<IExchangeReport>(BaseUrl);
        }

        public async Task<object> GetAsync(string date, string type = "ALL", string response = "json")
        {
            return await _exchangeReport.GetAsync(date, type, response).ConfigureAwait(false);
        }
    }
}