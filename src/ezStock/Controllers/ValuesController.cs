using System.Linq;
using System.Threading.Tasks;
using ezStock.twse;
using Microsoft.AspNetCore.Mvc;

namespace ezStock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ExchangeReportServices _exchangeReportServices;

        public ValuesController(ExchangeReportServices exchangeReportServices)
        {
            _exchangeReportServices = exchangeReportServices;
        }
        // GET api/values
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            //var util = new StockUtil();
            var result = await _exchangeReportServices.GetMonthlyTransaction("20190801", "2884").ConfigureAwait(false);
            return Ok(result.ToList());
        }

        // GET api/values/5
        [HttpGet("{stockNo}")]
        public async Task<ActionResult> Get(string stockNo)
        {
            return Ok(await _exchangeReportServices.GetMonthlyTransaction("20190801", stockNo).ConfigureAwait(false));
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
