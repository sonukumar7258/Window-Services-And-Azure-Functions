using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Xml;
using HtmlToXml;

namespace k190169_Q2
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
            //WriteToFile("Service is started at " + DateTime.Now);
            ConvertHtmlToXml convert = new ConvertHtmlToXml();
            convert.conversion();
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Interval = (600000 / 1000) / 60; //number in minutes it is equal to 10 minutes  
            timer.Enabled = true;
        }

        protected override void OnStop()
        {
        }
        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
        }
    }
}
