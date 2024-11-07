using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;

namespace OrganizationsEmployeesDictionaryWPF.Service
{
    public class Logger
    {
        private static readonly Lazy<Logger> _instance = new Lazy<Logger>(() => new Logger());
        public static Logger Instance => _instance.Value;

        private Logger()
        {
            Log.Logger = new LoggerConfiguration().WriteTo.File("my_log.txt", rollingInterval: RollingInterval.Day).CreateLogger();
        }

        public void LogInformation(string message)
        {
            Log.Information(message);
        }
        public void LogError(string mesage)
        {
            Log.Error(mesage);
        }
    }
}
