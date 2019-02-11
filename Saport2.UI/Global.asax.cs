using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Threading;
using System.Threading.Tasks;
using Saport2.Business.Entity;
using System.Net;

namespace Saport2.UI
{
    public class Global : System.Web.HttpApplication
    {
        #region Special Tasks
        protected void TimerWorker(object sender, System.Timers.ElapsedEventArgs e)
        {

            //PostService.DeserializeXMLToPostCategoriesForGlobalAsax("temp", "PostCategories.xml");

            //PostService.DeserializeXMLToPostsLightForGlobalAsax("temp", "PostsLight.xml");

            //PortalFeedsService.DeserializeXMLToPortalFeedsLightForGlobalAsax("temp", "PortalFeeds.xml");

            //AnnouncementService.DeserializeXMLToAnnouncementsForGlobalAsax("temp", "Announcements.xml");

            //BillBoardService.DeserializeXMLToAdvertsForGlobalAsax("temp", "AdvertsDiger.xml", 20, "Diğer");

            //BillBoardService.DeserializeXMLToAdvertsForGlobalAsax("temp", "AdvertsElektronik.xml", 20, "Elektronik");

            //BillBoardService.DeserializeXMLToAdvertsForGlobalAsax("temp", "AdvertsEmlak.xml", 20, "Emlak");

            //BillBoardService.DeserializeXMLToAdvertsForGlobalAsax("temp", "AdvertsKisiselUrunler.xml", 20, "Kişisel Ürünler");

            //BillBoardService.DeserializeXMLToAdvertsForGlobalAsax("temp", "AdvertsVasita.xml", 20, "Vasıta");

            //CampaignService.DeserializeXMLToCampsLightForGloalAsax("temp", "Campaigns.xml", 100);

            //NewsService.DeserializeXMLToNewsLightForGlobalAsax("temp", "News.xml", 50);

            //BannerService.DeserializeXMLToBannersLightForGlobalAsax("temp", "Banners.xml", 5);

            //WeatherStatusService.DeserializeXMLToWeathersForGlobalAsax("temp", "Statuses.xml");

        }

        protected void ThreadFunc()
        {
            System.Timers.Timer t = new System.Timers.Timer();
            t.Elapsed += new System.Timers.ElapsedEventHandler(TimerWorker);
            t.Interval = 300000; // Every 5 minutes
            t.Enabled = true;
            t.AutoReset = true;
            t.Start();
        }
        #endregion


        #region Keep Alive Function

        static Thread keepAliveThread = new Thread(KeepAlive);

        static void KeepAlive()
        {
            while (true)
            {
                WebRequest req = WebRequest.Create("https://saportweb.x.com/Home.aspx");
                req.GetResponse();
                try
                {
                    Thread.Sleep(300000); // Sleeps for 5 minutes
                }
                catch (ThreadAbortException)
                {
                    break;
                }
            }
        }
        #endregion

        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
       {
            PostService.DeserializeXMLToPostCategoriesForGlobalAsax("temp", "PostCategories.xml");

            PostService.DeserializeXMLToPostsLightForGlobalAsax("temp", "PostsLight.xml");

            PortalFeedsService.DeserializeXMLToPortalFeedsLightForGlobalAsax("temp", "PortalFeeds.xml");

            AnnouncementService.DeserializeXMLToAnnouncementsForGlobalAsax("temp", "Announcements.xml");

            BillBoardService.DeserializeXMLToAdvertsForGlobalAsax("temp", "AdvertsDiger.xml", 20, "Diğer");

            BillBoardService.DeserializeXMLToAdvertsForGlobalAsax("temp", "AdvertsElektronik.xml", 20, "Elektronik");

            BillBoardService.DeserializeXMLToAdvertsForGlobalAsax("temp", "AdvertsEmlak.xml", 20, "Emlak");

            BillBoardService.DeserializeXMLToAdvertsForGlobalAsax("temp", "AdvertsKisiselUrunler.xml", 20, "Kişisel Ürünler");

            BillBoardService.DeserializeXMLToAdvertsForGlobalAsax("temp", "AdvertsVasita.xml", 20, "Vasıta");

            CampaignService.DeserializeXMLToCampsLightForGloalAsax("temp", "Campaigns.xml", 100);

            NewsService.DeserializeXMLToNewsLightForGlobalAsax("temp", "News.xml", 50);

            BannerService.DeserializeXMLToBannersLightForGlobalAsax("temp", "Banners.xml", 5);

            WeatherStatusService.DeserializeXMLToWeathersForGlobalAsax("temp", "WeatherStatuses.xml");

        }


        protected void Application_Start(object sender, EventArgs e)
        {
            keepAliveThread.Start();

            #region timer
            System.Timers.Timer timer = new System.Timers.Timer();
            //timer.Interval = 120000; //2 minutes
            //timer.Interval = 300000; // 5 minutes
            timer.Interval = 600000; // 10 minutes
            timer.Elapsed += timer_Elapsed;
            timer.Start();
            #endregion

            //ThreadFunc();

            //THREAD IF NEEDED
            //Thread thread = new Thread(new ThreadStart(ThreadFunc));
            //thread.IsBackground = true;
            //thread.Name = "ThreadFunc";
            //thread.Start();
        }

        protected void Application_End()
        {
            keepAliveThread.Abort();
        }


        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {
            #region Delete Temporary Files Related To Session 
            string directoryPath = AppDomain.CurrentDomain.BaseDirectory + "\\temp\\";

            DirectoryInfo dir = new DirectoryInfo(directoryPath);
            FileInfo[] fileInfos = dir.GetFiles();
            for (int i = 0; i < Session.Keys.Count; i++)
            {
                foreach (FileInfo fileInfo in fileInfos)
                {
                    if (fileInfo.Name == Session.Keys.Get(i)) fileInfo.Delete();
                }
            }

            #endregion

        }


    }
}