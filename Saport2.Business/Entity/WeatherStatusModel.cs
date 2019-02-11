using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saport2.Business.Entity
{
    public class WeatherStatusModel
    {
        #region About
        /* It includes fields and properties of SampleEntity */
        #endregion

        #region Enums
        public enum WeatherCityName
        {
            Adana,
            Adapazarı,
            Afyon,
            Alanya,
            Ankara,
            Antalya,
            Aydın,
            Balıkesir,
            Bursa,
            Çanakkale,
            Çorlu,
            Denizli,
            Eskişehir,
            Gaziantep,
            İskenderun,
            İstanbul,
            İzmir,
            İzmit,
            Kahramanmaraş,
            Kayseri,
            Kocaeli,
            Konya,
            Kütahya,
            Manisa,
            Marmaris,
            Mersin,
            Niğde,
            Samsun,
            Tekirdag,
            Trabzon
        }
        #endregion

        #region Statics & Constants
        public const string weatherImgeUrl = "<img src=\"../resources/img/weather/{0}/{1}.png\" alt=\"{2}\" width=\"90%\" height=\"90%\"/>";
        public const string weatherDefaultCity = "İstanbul";
        public const string weatherSaveFileName = "WeatherStatuses.xml";
        public const string weatherSaveFolder = "temp";
        #endregion

        #region Fields
        // Fields
        #endregion

        #region Properties
        public class WeatherObject
        {
            public string Code { get; set; }
            public string Message { get; set; }
            public WeatherStatus Data { get; set; }
        }

        public class WeatherStatus
        {
            public string City { get; set; }
            public WeatherInfo WeatherInfo { get; set; }
        }

        public class WeatherInfo
        {
            public string ObservedAt { get; set; }
            public string Status { get; set; }
            public string StatusImage { get; set; }
            public string Temperature { get; set; }
        }

        #endregion
    }
}
