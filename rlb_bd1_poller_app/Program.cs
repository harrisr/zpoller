using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;

namespace rlb_bd1_poller_app
{
    public class Program
    {
        //  (1)  poll directory for error BD1 files
        //  (2)  keep track of last error DATE/TIME found
        //  (4)  
        //  (3)  if file is newer/past last error
        //          parse file/report error
        //          store latest error in error tracker file
        //  (5)  zzz
        //  (6)  zzz

        //   lastErrorPath = "c:\\dev\\rlb_bd1_poller_app\\rlb_bd1_poller_app\\bin\\Debug\\lasterror.txt"

        public static void MainZZ1(string[] args)
        {
            string appExecutionDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string lastErrorPath =  appExecutionDirectory + "lasterror.txt";


            string val1 = ConfigurationManager.AppSettings["key1"];
            System.Console.WriteLine(val1);


            System.Console.WriteLine("Executing in:  " + appExecutionDirectory);
            


            string strLastErrorDateTime = File.ReadAllText(lastErrorPath);
            DateTime dtLastErrorDateTime = Convert.ToDateTime(strLastErrorDateTime);
            System.Console.WriteLine(strLastErrorDateTime);
            System.Console.WriteLine(dtLastErrorDateTime);


            string dirs = ConfigurationManager.AppSettings["poll_directories"];
            System.Console.WriteLine(dirs);

            string[] arrdirs = dirs.Split(';');
            foreach (string dir in arrdirs)
            {
                System.Console.WriteLine("----------------------------");
                System.Console.WriteLine(dir);
                System.Console.WriteLine(".  .  .  .  .  .  .  .  .  .");
                var arrfiles = Directory.EnumerateFiles(dir, "*.*", SearchOption.TopDirectoryOnly);
                foreach (string file in arrfiles)
                {
                    System.Console.WriteLine(file + "  -   " + File.GetCreationTime(file));

                    dtLastErrorDateTime = File.GetCreationTime(file);
                }
            }
            strLastErrorDateTime = dtLastErrorDateTime.ToString("yyyy-MM-dd  HH:mm:ss");


            System.Console.WriteLine("LAST ERROR FOUND:  " + strLastErrorDateTime);
            File.WriteAllText(lastErrorPath, strLastErrorDateTime);


            System.Console.WriteLine("Press <ENTER> to quit the program...");
            System.Console.ReadLine();
        }
    }
}
