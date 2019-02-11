using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saport2.Business.Entity
{
    public class ExchangeRateModel
    {
        #region About
        /* It includes fields and properties of SampleEntity */
        #endregion

        #region Enums
        // Enums
        #endregion

        #region Statics & Constants
        // Statics
        #endregion

        #region Fields
        // Fields
        #endregion

        #region Properties
        public class ExchangeObject
        {
            public string Code { get; set; }
            public string Message { get; set; }
            public List<ExchangeRate> Data { get; set; }
        }

        public class ExchangeRate
        {
            public decimal BuyPrice { get; set; }
            public decimal SellPrice { get; set; }
            public string Symbol { get; set; }
        }
        #endregion
    }
}
