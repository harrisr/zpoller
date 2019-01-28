using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Serialization;
using log4net;

[assembly: log4net.Config.XmlConfigurator(Watch = true, ConfigFile = "log4net.config")]
namespace RLBBD1PollerLibrary
{
    public class DirExtPair
    {
        public string Directory { get; set; }
        public string Extension { get; set; }
        public DirExtPair(string dir, string ext)
        {
            this.Directory = dir;
            this.Extension = ext;
        }
    }

    public class RLBBD1PollManager
    {
        //  (1)  poll directory for error BD1 files
        //  (2)  keep track of last error DATE/TIME found
        //  (3)  loop every ZZZ seconds/minutes, searching for new files
        //  (4)       if file is newer/past last error
        //                 parse file/report error
        //                 store latest error in error tracker file
        //  (5)  zzz
        //  (6)  zzz

        //   lastErrorPath = "c:\\dev\\rlb_bd1_poller_app\\rlb_bd1_poller_app\\bin\\Debug\\lasterror.txt"

        private static readonly ILog logger = LogManager.GetLogger(typeof(RLBBD1PollManager));
        private string appExecutionDirectory;
        private string lastErrorPath;
        private DateTime dtLastErrorDateTime;
        private string strLastErrorDateTime;
        private ArrayList pollDirectories;
        private ArrayList pollExtensions;
        private ArrayList pollDirWithExtensions;

        public RLBBD1PollManager()
        {
            pollDirectories = new ArrayList();
            pollExtensions = new ArrayList();
            pollDirWithExtensions = new ArrayList();

            appExecutionDirectory = AppDomain.CurrentDomain.BaseDirectory;
            
            lastErrorPath = Path.Combine(appExecutionDirectory, "lasterror.txt");
            
            System.Console.WriteLine(string.Format("Executing in:  {0}", appExecutionDirectory));
            System.Console.WriteLine(string.Format("File storing last error:  {0}", lastErrorPath));

            logger.Debug(string.Format("Executing in:  {0}", appExecutionDirectory));
            logger.Debug(string.Format("File storing last error:  {0}", lastErrorPath));

            GetLastErrorDateTime();
            GetPollingDirectories();
        }

        private void GetLastErrorDateTime()
        {
            strLastErrorDateTime = File.ReadAllText(lastErrorPath);
            dtLastErrorDateTime = Convert.ToDateTime(strLastErrorDateTime);
            System.Console.WriteLine(strLastErrorDateTime);
            System.Console.WriteLine(dtLastErrorDateTime);

            logger.Debug(string.Format("strLastErrorDateTime:  {0}", strLastErrorDateTime));
            logger.Debug(string.Format("dtLastErrorDateTime:  {0}", dtLastErrorDateTime));
        }

        private void GetPollingDirectories()
        {
            try
            {
                string direxts = ConfigurationManager.AppSettings["poll_dirwithext"];
                
                logger.Debug(string.Format("poll_dirwithext:  {0}", direxts));

                pollDirWithExtensions.AddRange(direxts.Split(';'));

                logger.Debug(string.Format(" > > >: {0}", pollExtensions));
                logger.Debug(string.Format(" > > >: {0}", pollExtensions.Count));

                foreach (string pair in pollDirWithExtensions)
                {
                    string[] dirext = pair.Split('|');
                    DirExtPair dirextPair = new DirExtPair(dirext[0], dirext[1]);
                    pollDirectories.Add(dirextPair);
                }

                foreach (DirExtPair pair in pollDirectories)
                {
                    logger.Debug(string.Format("----------------------------"));
                    logger.Debug(string.Format("poll_directories:  {0}  ext: {1}", pair.Directory, pair.Extension));
                    logger.Debug(string.Format(".  .  .  .  .  .  .  .  .  ."));

                    var arrfiles = Directory.EnumerateFiles(pair.Directory, pair.Extension, SearchOption.TopDirectoryOnly);
                    foreach (string file in arrfiles)
                    {
                        logger.Debug(string.Format("{0}  {1}", file, File.GetCreationTime(file)));
                        dtLastErrorDateTime = File.GetCreationTime(file);
                    }
                }

                strLastErrorDateTime = dtLastErrorDateTime.ToString("yyyy-MM-dd  HH:mm:ss");

                logger.Debug(string.Format("strLastErrorDateTime:  {0}", strLastErrorDateTime));

                File.WriteAllText(lastErrorPath, strLastErrorDateTime);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw;
            }
        }
    }
}
