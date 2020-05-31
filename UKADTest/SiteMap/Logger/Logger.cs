using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SiteMap.Logger
{
    public class Logger
    {
        private static Logger _Instance;

        private string _FilePathInstance;
        public static Logger Default
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new Logger();
                }
                return _Instance;
            }
        }

        private string filePathGlobal
        {
            get
            {
                if (_FilePathInstance == null)
                {
                    DateTime localDate = DateTime.Now;
                    string date = localDate.ToString("MM-dd-yyyy-HH-mm");
                    string fileName = "\\" + date + "-LoggerFile.txt";
                    string path = Directory.GetCurrentDirectory();
                    path += "\\Logger";
                    _FilePathInstance = path + fileName;
                }
                return _FilePathInstance;
            }
        }

        public async void Write(string data)
        {
            string[] message = { data };
            await File.AppendAllLinesAsync(Logger.Default.filePathGlobal, message);
        }
    }
}

