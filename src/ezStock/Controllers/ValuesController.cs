using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ezStock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<Stock> Get()
        {
            var util = new StockUtil();

            return util.GetStockList();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
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

    public class Stock
    {

        public string Id;
        public string Name;

        public Stock(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }

    public class StockUtil
    {
        // 本國上市證券國際證券辨識號碼一覽表
        // https://quality.data.gov.tw/dq_download_json.php?nid=18419&md5_url=9791ec942cbcb925635aa5612ae95588
        private const string _url2 = "https://quality.data.gov.tw/dq_download_json.php?nid=18419&md5_url=9791ec942cbcb925635aa5612ae95588";
        // 本國上櫃證券國際證券辨識號碼一覽表
        // https://quality.data.gov.tw/dq_download_json.php?nid=28568&md5_url=7c1850be92ee9191486528d32244b751
        private const string _url4 = "https://quality.data.gov.tw/dq_download_json.php?nid=28568&md5_url=7c1850be92ee9191486528d32244b751";

        // TODO: 要存在JSON定期更新，避免記憶體使用過大
        public IEnumerable<Stock> GetStockList()
        {
            //var doc = new HtmlWeb
            //{
            //    AutoDetectEncoding = false,
            //    OverrideEncoding = Encoding.Default,
            //}.Load(_url);
            IEnumerable<Stock> list2 = GetOfficeStockList(_url2);
            IEnumerable<Stock> list4 = GetOfficeStockList(_url4);

            return list2?.Concat(list4);
        }

        private static IEnumerable<Stock> GetOfficeStockList(string url)
        {
            //指定來源網頁
            WebClient webClient = new WebClient
            {
                //Encoding = Encoding.GetEncoding("Big5")
                Encoding = Encoding.Unicode
            };
            webClient.Headers.Add(HttpRequestHeader.ContentType, "application/json");
            //將網頁來源資料暫存到記憶體內
            // MemoryStream ms = new MemoryStream(Encoding.GetEncoding("Big5").GetBytes(webClient.DownloadData(url).ToString()));
            var data = webClient.DownloadData(url);
            //var ss = HttpUtility.HtmlDecode(data);

            var data2 = Encoding.UTF8.GetString(data);
            //var s = JsonConvert.DeserializeObject<StockInfo[]>(data);
            // 使用預設編碼讀入 HTML 
            // HtmlDocument doc = new HtmlDocument();
            // doc.Load(ms, Encoding.GetEncoding("Big5"));
            // var list = doc.DocumentNode.SelectNodes("//table[@class='h4']/tr[position()>2]/td[1]")
            //     .Select(x =>
            //         new Stock(x.InnerText.Split(null)[0], x.InnerText.Split(null)[1])
            //     );
            return null;
        }
    }

    public class StockInfo
    {
        [JsonProperty("\u51fa\u8868\u65e5\u671f")]
        public string Update { get; }

        [JsonProperty("\u516c\u53f8\u4ee3\u865f")]
        public string Id { get; }

        [JsonProperty("\u516c\u53f8\u540d\u7a31")]
        public string FullName { get; }

        [JsonProperty("公司簡稱")]
        public string Name { get; }
    }
}
