using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Shimanni.Trader.Common
{
    public class SingleStrategyLog 
    {
        
        
        private string _FullPathToStrategyDirectory = Path.GetFullPath(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"ShimanniStrategy"));
        private bool _WriteMarketData;
         private StreamWriter _LogStreamer;
        private int _StragegyNumber;

        private string _LogFileName;
        
        


        /// <summary>
        /// Initializes a new instance of the StreamerForSingleStrategy class.
        /// </summary>
        public SingleStrategyLog(int pStragegyNumber)
        {
            _StragegyNumber = pStragegyNumber;
            _FullPathToStrategyDirectory = Path.GetFullPath(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"ShimanniStrategy"));
            _LogFileName = @"\" + "StragegyNumber__"  +  _StragegyNumber.ToString() + "   "  + DateTime.Today.ToString("yyyyMMdd") + ".txt";
            _WriteMarketData = true;

            if (!Directory.Exists(@FullPathToStrategyDirectory))
            {
                Directory.CreateDirectory(@FullPathToStrategyDirectory);
            }
            string xxx = FullPathToStrategyDirectory + _LogFileName;

            if (_LogStreamer == null)
            {

                _LogStreamer = new StreamWriter(xxx, true);
                _LogStreamer.AutoFlush = true; 


            }

            




        }


        #region Methods

        private void InitateStreamsParmeters(ref StreamWriter pLogStreamer)

        {
            if (!Directory.Exists(@FullPathToStrategyDirectory))
            {
                Directory.CreateDirectory(@FullPathToStrategyDirectory);
            }
                string xxx = FullPathToStrategyDirectory + LogFileName;

                pLogStreamer = new StreamWriter(@xxx, true);
     //           LogStraemer.AutoFlush= true;
            
        }

        public  void WriteLineToLog(string text)
        {

           
            try
            {
                if (LogStraemer != null && WriteMarketData == true)
                {
                    LogStraemer.WriteLine(DateTime.Now.ToString("hh:mm:ss.ffff") + "\t" + text);
                    LogStraemer.Flush();
                } 
            }
            catch (Exception ex)
            {
                string x = ex.InnerException.StackTrace;
                
            }
            finally
            {
                
            }


            
        }

        #endregion


        #region Propeties
        
        
        public int StragegyName
        {
            get { return _StragegyNumber; }
            set { _StragegyNumber = value; }
        }

        public string FullPathToStrategyDirectory
         {
         		get
         		{
         				return _FullPathToStrategyDirectory;
         		}
         		set
         		{
         				_FullPathToStrategyDirectory = value;
         		}
         }
          public StreamWriter LogStraemer
          {
          		get
          		{
          				return _LogStreamer;
          		}
          		
          }
          public bool WriteMarketData
          {
          		get
          		{
          				return _WriteMarketData;
          		}
          		set
          		{
          				_WriteMarketData = value;
          		}
          }
          public string LogFileName
          {
              get
              {
                  return _LogFileName;
              }
              set
              {
                  _LogFileName = value;
              }
          }

        #endregion

    }
}
