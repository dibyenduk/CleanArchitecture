using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MES.Core.Infrastructure
{
    public class RequestContext
    {
        public string User { get; private set; }
        public string UserCulture { get; private set; }
        public string TransactionOriginApplication { get; private set; }

        public void SetUserCulture(string userCulture) => this.UserCulture = userCulture;

        public void SetUser(string user) => this.User = user;

        public void SetTransactionOriginApplication(string originApplication) => this.TransactionOriginApplication = originApplication;
    }
}
