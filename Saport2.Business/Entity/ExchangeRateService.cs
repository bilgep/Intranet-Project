using Saport2.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using DAT = Saport2.Data;
using HLP = Saport2.Shared.Helpers;
using SP = Microsoft.SharePoint.Client;
using EXP = Saport2.Shared.Exceptions;


namespace Saport2.Business.Entity
{
    public class ExchangeRateService: ExchangeRateModel
    {
        #region About
        /* It includes methods related to SampleEntity */
        #endregion

        #region Methods

        public static List<ExchangeRate> GetExchangeRates()
        {
            try
            {
                HttpWebRequest request = DataService.CreateRequest(DataStatics.DomainForService, DataStatics.UserNameForService, DataStatics.PasswordForService, DataStatics.saportServiceURL(DataStatics.saportServiceGetLatestExchangeRates), true, true, "application/json; charset=UTF-8", "application/json; odata=verbose", "GET");

                request.ContentLength = 0;

                WebResponse response = request.GetResponse();
                string responseString = DataService.RestfulReader(response);

                response.Close();

                var serializer = new JavaScriptSerializer();
                ExchangeObject exchangeRateObj = serializer.Deserialize<ExchangeObject>(responseString);
                List<ExchangeRate> exchangeRates = exchangeRateObj.Data;
                return exchangeRates;
            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        #endregion
    }
}
