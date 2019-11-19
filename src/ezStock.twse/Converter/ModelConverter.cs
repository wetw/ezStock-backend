using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WE.Domain.Transaction;

namespace ezStock.twse.Converter
{
    public static class ModelConverter
    {
        public static IEnumerable<TransactionInfo> Convert(this twseReport value)
        {
            return value.data.Select(x => new TransactionInfo
            {
                Date = DateTime.ParseExact(x[(int)DataDetail.Date], "yyy/MM/dd", CultureInfo.InvariantCulture).AddYears(1911),
                TradingVolume = long.TryParse(x[(int)DataDetail.TradingVolume], NumberStyles.AllowThousands, CultureInfo.CurrentCulture, out var tV) ? tV : 0,
                TurnOver = long.TryParse(x[(int)DataDetail.TurnOver], NumberStyles.AllowThousands, CultureInfo.CurrentCulture, out var tO) ? tO : 0,
                OpeningPrice = decimal.TryParse(x[(int)DataDetail.OpeningPrice], out var op) ? op : 0,
                HightestPrice = decimal.TryParse(x[(int)DataDetail.HightestPrice], out var hp) ? hp : 0,
                LowestPrice = decimal.TryParse(x[(int)DataDetail.LowestPrice], out var lp) ? lp : 0,
                ClosePrice = decimal.TryParse(x[(int)DataDetail.ClosePrice], out var cp) ? cp : 0,
                SpreadPercent = x[(int)DataDetail.SpreadPercent],
                TotalCount = int.TryParse(x[(int)DataDetail.TotalCount], NumberStyles.AllowThousands, CultureInfo.CurrentCulture, out var tc) ? tc : 0
            });
        }
    }
}