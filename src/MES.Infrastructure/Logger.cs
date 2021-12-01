using MES.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MES.Infrastructure
{
    // Idea will be to use Serilog for logging information into Splunk.
    // Can have multiple 
    public class Logger : ILogService
    {
        public void Debug(string message)
        {
            
        }

        public void Error(Exception ex, string message)
        {
            
        }

        public void Information(string message)
        {
            
        }

        public void Warning(string message)
        {
            
        }
    }
}
