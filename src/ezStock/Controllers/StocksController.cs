using System;
using System.Globalization;
using System.Threading.Tasks;
using ezStock.twse;
using Microsoft.AspNetCore.Mvc;

namespace ezStock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StocksController : ControllerBase
    {
        private readonly ExchangeReportServices _exchangeReportServices;

        public StocksController(ExchangeReportServices exchangeReportServices)
        {
            _exchangeReportServices = exchangeReportServices;
        }

        [HttpGet]
        public Task<IActionResult> Get(string stockNo)
        {
            throw new NotImplementedException(stockNo);
        }

        [HttpGet("{stockNo}/report")]
        public async Task<IActionResult> GetMonthReportAsync(DateTime date, string stockNo, string type = "month")
        {
            return type switch
            {
                "month" => Ok(await _exchangeReportServices.GetMonthlyTransaction(date.ToString("yyyyMM01", CultureInfo.CurrentCulture), stockNo).ConfigureAwait(false)),
                _ => throw new NotImplementedException(),
            };
        }
    }
}