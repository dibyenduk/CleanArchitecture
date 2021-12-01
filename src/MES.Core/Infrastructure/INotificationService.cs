using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MES.Core.Infrastructure
{
    public interface INotificationService
    {
        void Notify(string message);
    }
}
