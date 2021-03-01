using XAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if NET48
using System.Drawing.Design;
#endif

namespace QuantBox.APIProvider.Single

{
    [DefaultProperty("Label")]
    public class ServerItem : ICloneable
    {
        [Category("标签")]
        public string Label { get; set; }

#if NET48
        [Editor(typeof(System.Windows.Forms.Design.FolderNameEditor), typeof(UITypeEditor))]
#endif
        public string Path { get; set; }
        
        public override string ToString()
        {
            return string.Format("Label={0}", this.Label);
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
