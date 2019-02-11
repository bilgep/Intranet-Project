using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Saport2.Business.Entity;
using System.Web.UI.HtmlControls;
using System.Web.Services;
using System.Web.Script.Services;
using Saport2.Service;
using HtmlAgilityPack;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using SP = Microsoft.SharePoint.Client;
using System.Xml;
using System.Text.RegularExpressions;
using HLP = Saport2.Shared.Helpers;
using EXP = Saport2.Shared.Exceptions;
using System.Globalization;

namespace Saport2.UI
{
    public partial class Saport2 : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                divSearch.Value = string.Empty;
                if (Session["searchValue"] != null) divSearch.Value = Session["searchValue"].ToString();
                Session.Remove("searchValue");

                #region hide Select City Div of Weather Tool
                    //HtmlControl control = FindControl("ex1") as HtmlControl;
                    //if (control.Attributes["display"].ToString().ToLower() == "none")
                    //{
                    //    HtmlControl divSelectCity = FindControl("divWeatherSelectCity") as HtmlControl;
                    //    divSelectCity.Attributes["display"] = "none";
                    //}
                    #endregion

                if (!Page.IsPostBack)
                {
                    #region Load User Info
                    ltrUserName.Text = HttpContext.Current.Request.Cookies["userName"] != null ? HttpContext.Current.Request.Cookies["userName"].Values[0]: "System";
                    CultureInfo culture = new CultureInfo("tr-TR");
                    #endregion

                    #region Prepare Page & Controls
                    string selectedCity = HttpContext.Current.Request.Cookies["cityName"] != null ? HttpContext.Current.Request.Cookies["cityName"].Values[0] : WeatherStatusModel.weatherDefaultCity;
                    selectedCity = HttpUtility.UrlDecode(selectedCity);
                    if (selectedCity == "İstanbul") selectedCity = "Istanbul";
                    ddlSelectCity.SelectedIndex = ddlSelectCity.Items.IndexOf(new ListItem() { Value = selectedCity });
                    #endregion


                    #region Load Exchange Rates
                    List<ExchangeRateModel.ExchangeRate> exRates = Business.Entity.ExchangeRateService.GetExchangeRates();

                    if (exRates.Count > 0)
                    {
                        ExchangeRateModel.ExchangeRate dollar = exRates.Single(x => x.Symbol == "Dolar");
                        ExchangeRateModel.ExchangeRate euro = exRates.Single(x => x.Symbol == "Euro");

                        ltrDovizDolarAlis.Text = dollar.BuyPrice.ToString("₺ #.###");
                        ltrDovizDolarSatis.Text = dollar.SellPrice.ToString("₺ #.###");
                        ltrDovizEuroAlis.Text = euro.BuyPrice.ToString("₺ #.###");
                        ltrDovizEuroSatis.Text = euro.SellPrice.ToString("₺ #.###");
                    }

                    #endregion

                    #region Load Weather Status
                    ltrWeatherDate.Text = DateTime.Now.Day + " " + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Now.Month).Substring(0, 3);

                    WeatherStatusModel.WeatherStatus weatherStat = WeatherStatusService.DeserializeXMLToWeathers(WeatherStatusModel.weatherSaveFolder, WeatherStatusModel.weatherSaveFileName, ddlSelectCity.SelectedItem.Text);

                    if (weatherStat != null)
                    {
                        ltrWeatherImage.Text = WeatherStatusService.GetWeatherImageUrl(weatherStat);
                        ltrWeatherDegree.Text = weatherStat.WeatherInfo.Temperature.ToString();
                    }


                    #endregion


                    #region Load Page Title
                    PageService.SetPageTitle(this.Page, PageService.GetPageTitle(Request.Url.AbsolutePath));
                    #endregion

                    #region Load Menu Items
                    string menuItemsLiteral = "";
                    string menuItemUrl = "../{0}.aspx";
                    foreach (var item in PageModel.menuItems)
                    {
                        string menuText = string.Format(menuItemUrl, item.Key);
                        menuItemsLiteral += string.Format("<li><a href =\"{0}\">{1}</a ></li>", menuText, item.Value);
                    }
                    ltrMenuItems.Text = menuItemsLiteral;
                    #endregion



                }

            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message + ex.StackTrace.ToString());
            }


        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("Search.aspx");
            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message + ex.StackTrace.ToString());
            }
        }


        protected void ddlSelectCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                WeatherStatusService.AddCityCookie(ddlSelectCity.SelectedValue);

                WeatherStatusModel.WeatherStatus weatherStat = WeatherStatusService.DeserializeXMLToWeathers(WeatherStatusModel.weatherSaveFolder, WeatherStatusModel.weatherSaveFileName, ddlSelectCity.SelectedItem.Text);

                if (weatherStat != null)
                {
                    ltrWeatherImage.Text = WeatherStatusService.GetWeatherImageUrl(weatherStat);
                    ltrWeatherDegree.Text = weatherStat.WeatherInfo.Temperature.ToString();
                }

                divModalcik.Style.Remove("display");
                divModalcik.Style.Add("display","block");
            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message + ex.StackTrace.ToString());
            }
        }

    }
    }