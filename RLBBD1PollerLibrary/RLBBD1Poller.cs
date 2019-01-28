using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Timers;
using System.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Serialization;
using log4net;


namespace RLBBD1PollerLibrary
{
    public class RLBBD1Poller
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(RLBBD1Poller));

        private static Timer aTimer;
        private string appExecutionDirectory;
        private string pollDirectory;
        private string pollExtensionFilter;
        private string lastErrorPath;
        private string lastErrorDate;
        private string strLastErrorDateTime;
        private DateTime dtLastErrorDateTime;
        private DateTime dtFileCreationTime;
        private DateTime dtHoldErrorDateTime;
        private double timerIntervalInMinutes;
        private double timerIntervalInSeconds;
        private double timerIntervalInMilliSeconds;

        public RLBBD1Poller(string pollDir = "", 
            string pollExt = "", 
            double interval = 0.0)
        {
            try
            {
                ValidateInputArguments(pollDir, pollExt, interval);
                GetLastErrorDateTime();
                SetTimerEvent();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw;
            }
        }

        private void ValidateInputArguments(string pollDir = "", 
            string pollExt = "",
            double interval = 0.0)
        {
            if (pollDir == null || pollExt == null
                || pollDir.Trim().Length == 0 || pollExt.Trim().Length == 0)
            {
                throw new ArgumentException("Invalid Directory Polling Arguments (must be non-null)");
            }


            if (!Directory.Exists(pollDir))
            {
                throw new ArgumentException("Invalid Polling Directory (does not exist)");
            }
            Regex rgx = new Regex(@"\*\.\w\w\w");
            Match match = rgx.Match(pollExt);
            if (!match.Success)
            {
                throw new ArgumentException("Invalid Polling Filter (wrong format)");
            }

            if (interval < 0.1)
            {
                throw new ArgumentException("Invalid Timing Interval (must be non-zero)");
            }

            this.pollDirectory = pollDir;
            this.pollExtensionFilter = pollExt;
            this.timerIntervalInMinutes = interval;
            this.timerIntervalInSeconds = this.timerIntervalInMinutes * 60;
            this.timerIntervalInMilliSeconds = this.timerIntervalInSeconds * 1000;
            
        }


        private void SetTimerEvent()
        {
            // Create the timer and set the desired interval.
            logger.Debug(string.Format("CREATING THE PERIODIC TIMER !!!!"));
            logger.Debug(string.Format("TIMER INTERVAL:  {0} (milliseconds)", this.timerIntervalInMilliSeconds));

            aTimer = new System.Timers.Timer();
            aTimer.Interval = this.timerIntervalInMilliSeconds;
                        
            aTimer.Elapsed += PollDirectory;    // Hook up the Elapsed event for the timer. 
            //aTimer.Elapsed += OnTimedEvent;

            aTimer.AutoReset = true;   // Have the timer fire repeated events (true is the default)
            aTimer.Enabled = true;   // Start the timer
        }


        private void GetLastErrorDateTime()
        {
            appExecutionDirectory = AppDomain.CurrentDomain.BaseDirectory;
            
            lastErrorPath = Path.Combine(pollDirectory, "lasterror.txt");
            strLastErrorDateTime = File.ReadAllText(lastErrorPath);
            dtLastErrorDateTime = Convert.ToDateTime(strLastErrorDateTime);

            logger.Debug(string.Format("JUST READ THE LAST ERROR FROM:  Last Error  Path:  {0}", lastErrorPath));
            logger.Debug(string.Format("JUST READ THE LAST ERROR AS:  strLastErrorDateTime:  {0}", strLastErrorDateTime));
            logger.Debug(string.Format("JUST READ THE LAST ERROR AS:  dtLastErrorDateTime:  {0}", dtLastErrorDateTime));


            logger.Debug(string.Format("Executing in:  {0}", appExecutionDirectory));

            logger.Debug(string.Format("Poll Directory:  {0}", pollDirectory));
            logger.Debug(string.Format("Last Error  Path:  {0}", lastErrorPath));
            logger.Debug(string.Format("Poll ExtensionFilter:  {0}", pollExtensionFilter));

            logger.Debug(string.Format("strLastErrorDateTime:  {0}", strLastErrorDateTime));
            logger.Debug(string.Format("dtLastErrorDateTime:  {0}", dtLastErrorDateTime));

            dtHoldErrorDateTime = dtLastErrorDateTime;
        }

        //private static void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        //{
        //    Console.WriteLine("The Elapsed event was raised at {0}", e.SignalTime);
        //}


        private void PollDirectory(Object source, System.Timers.ElapsedEventArgs e)
        {
            logger.Debug(string.Format("\n------------------------------\n"));
            logger.Debug(string.Format("PERFORMING POLLING: Directory:  {0}", pollDirectory));
            logger.Debug(string.Format("PERFORMING POLLING: Filter:  {0}", pollExtensionFilter));

            var arrfiles = Directory.EnumerateFiles(pollDirectory, pollExtensionFilter, SearchOption.TopDirectoryOnly);
            foreach (string file in arrfiles)
            {
                dtFileCreationTime = File.GetCreationTime(file);

                logger.Debug(string.Format("FL: {0}  CRT: {1}   LST ERR:  {2}", file, dtFileCreationTime, dtLastErrorDateTime));
                logger.Debug(string.Format("Comparing this (FILE): {0}  DT: {1}", file, dtFileCreationTime));
                logger.Debug(string.Format("To this (LAST ERROR): {0}   DT: {1}", file, dtLastErrorDateTime));

                logger.Debug(string.Format("TRUNCATING THE MILLISECONDS !!!"));
                dtFileCreationTime = dtFileCreationTime.AddTicks(-(dtFileCreationTime.Ticks % TimeSpan.TicksPerSecond));
                dtLastErrorDateTime = dtLastErrorDateTime.AddTicks(-(dtLastErrorDateTime.Ticks % TimeSpan.TicksPerSecond));

                logger.Debug(string.Format("Comparing this (FILE): {0}  DT: {1}", file, dtFileCreationTime));
                logger.Debug(string.Format("To this (LAST ERROR): {0}   DT: {1}", file, dtLastErrorDateTime));


                int compare = dtFileCreationTime.CompareTo(dtLastErrorDateTime);
                logger.Debug(string.Format("COMPARE RESULT: {0} ", compare));

                if (compare == 1)
                {
                    dtHoldErrorDateTime = dtFileCreationTime;
                    PerformFileParsing(file);
                }
            }
            logger.Debug(string.Format("SAVING NEW LAST ERROR:  {0}", dtHoldErrorDateTime.ToString("yyyy-MM-dd HH:mm:ss")));
            File.WriteAllText(lastErrorPath, dtHoldErrorDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
        }



        private void PerformFileParsing(string file)
        {
            string user = string.Empty;
            string date = string.Empty;
            string filename = string.Empty;
            char chr;
            
            byte[] bytes = File.ReadAllBytes(file);
            
            int count = bytes.Length;

            for (int i = 0; i < count; i++)
            {
                if (i >= 17 && i < 25)
                {
                    chr = Convert.ToChar(bytes[i]);
                    user += chr;
                }
                if (i >= 34 && i < 43)
                {
                    chr = Convert.ToChar(bytes[i]);
                    date += chr;
                }
                if (i >= 52 && i < 64)
                {
                    chr = Convert.ToChar(bytes[i]);
                    filename += chr;
                }
            }
            logger.Debug(string.Format("\nuser:  {0}", user));
            logger.Debug(string.Format("date:  {0}", date));
            logger.Debug(string.Format("filename:  {0} \n", filename));
        }

    }
}
