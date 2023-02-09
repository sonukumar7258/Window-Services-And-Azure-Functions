using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
namespace k190169_Q1
{
    public partial class Service1 : ServiceBase
    {
        Timer timer = new Timer(); // name space(using System.Timers;)  
        public Service1()
        {
            InitializeComponent();
        }
        protected override void OnStart(string[] args)
        {
            WriteToFile("Download web Page Service is started at " + DateTime.Now);
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Interval = (300000 / 1000) / 60; // 5 Minutes 
            timer.Enabled = true;
        }
        protected override void OnStop()
        {
            WriteToFile("Download web Page Service Service is stopped at " + DateTime.Now);
        }
        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            WriteToFile("Download web Page Service Service is recall at " + DateTime.Now);
        }
        public void WriteToFile(string Message)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            String url = "https://www.psx.com.pk/market-summary";
            String finalFolder = path;

            // Create a new WebClient instance.
            WebClient myWebClient = new WebClient();
            // DownloadHtml Format of that webpage and treated as String and Store into the data
            String data = myWebClient.DownloadString(url);
            String todaysDate = "16Oct22";
            // Set the name of folder of todays date
            finalFolder += "\\" + "Summary" + todaysDate + ".html";

            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\ServiceLog_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
            if (!File.Exists(filepath))
            {
                // Create a file to write to.   
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    // Store String into HTML File
                    File.WriteAllText(finalFolder, data);

                    sw.WriteLine(Message);
                    sw.WriteLine("File Downloaded Successfully at Location : {0} from {1} ", finalFolder, url);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(Message);
                    sw.WriteLine("File Downloaded Successfully at Location : {0} from {1} ", finalFolder, url);
                }
            }
        }
    }
}
