using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLBBD1PollerLibrary;
using log4net;

[assembly: log4net.Config.XmlConfigurator(Watch = true, ConfigFile = "log4net.config")]
namespace rlb_bd1_poller_app
{
    public class Program3
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(Program3));

        public static void Main()
        {
            //double interval = Convert.ToDouble(ConfigurationManager.AppSettings["poll_timerinterval"]);
            //logger.Debug(interval);


            //DateTime time1 = new DateTime(2019, 01, 27, 16, 09, 53);
            //DateTime time2 = new DateTime(2019, 01, 27, 16, 09, 54);
            //int compare = time1.CompareTo(time2);
            //System.Console.WriteLine(compare);


            //RLBBD1Poller poller = new RLBBD1Poller();
            RLBBD1Poller poller = new RLBBD1Poller("C:\\polling\\watch1", "*.bd1", 0.3);


            System.Console.WriteLine("Press <ENTER> to finish   ZZZZZZZZZZZZZZZZZz...");
            System.Console.ReadLine();
        }

    }
}
