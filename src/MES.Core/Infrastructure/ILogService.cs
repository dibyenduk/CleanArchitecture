using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MES.Core.Infrastructure
{
    public interface ILogService
    {
        void Information(string message);

        void Warning(string message);

        void Debug(string message);

        void Error(Exception ex, string message);
    }
}
