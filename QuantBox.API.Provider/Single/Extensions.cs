using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XAPI.Callback;

using NLog;

namespace QuantBox.APIProvider.Single
{
    public static class XAPI_Extensions
    {
        public static Logger GetLog(this XApi api)
        {
            return (api.Log as Logger);
        }
    }
}
