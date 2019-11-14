using ezStock.twse.Converter;
using Refit;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using WE.Domain.Transaction;

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

        public async Task<IEnumerable<TransactionInfo>> GetMonthlyTransaction(string date, string stockNo)
        {
            return (await _exchangeReport.GetMonthlyTransaction(date, stockNo).ConfigureAwait(false))
                .Convert();
        }
    }
}