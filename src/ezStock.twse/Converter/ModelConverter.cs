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
                TradingVolume = int.Parse(x[(int)DataDetail.TradingVolume], NumberStyles.AllowThousands, CultureInfo.CurrentCulture),
                TurnOver = int.Parse(x[(int)DataDetail.TurnOver], NumberStyles.AllowThousands, CultureInfo.CurrentCulture),
                OpeningPrice = decimal.Parse(x[(int)DataDetail.OpeningPrice], CultureInfo.CurrentCulture),
                HightestPrice = decimal.Parse(x[(int)DataDetail.HightestPrice], CultureInfo.CurrentCulture),
                LowestPrice = decimal.Parse(x[(int)DataDetail.LowestPrice], CultureInfo.CurrentCulture),
                ClosePrice = decimal.Parse(x[(int)DataDetail.ClosePrice], CultureInfo.CurrentCulture),
                SpreadPercent = x[(int)DataDetail.SpreadPercent],
                TotalCount = int.Parse(x[(int)DataDetail.TotalCount], NumberStyles.AllowThousands, CultureInfo.CurrentCulture)
            });
        }
    }
}