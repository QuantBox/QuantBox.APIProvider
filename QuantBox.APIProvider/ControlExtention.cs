using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace QuantBox.APIProvider
{
#if NET48
    using System.Windows.Forms;

    public static class ControlExtention
    {
        public delegate void InvokeHandler();

        public static void SafeInvoke(this Control control, InvokeHandler handler)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(handler);
            }
            else
            {
                handler();
            }
        }
    }
#endif
}
