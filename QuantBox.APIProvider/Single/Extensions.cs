using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XAPI;

using NLog;

namespace QuantBox.APIProvider.Single
{
    public static class XAPI_Extensions
    {
        public static Logger GetLog(this IXApi api)
        {
            return (api.Log as Logger);
        }
    }
}
