using QuantBox.APIProvider.Single;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Design;

namespace QuantBox.APIProvider.UI
{
    class ApiManagerTypeEditor : UITypeEditor
    {
        // Methods
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (((context == null) || (context.Instance == null)) || (provider == null))
            {
                return base.EditValue(context, provider, value);
            }
            IWindowsFormsEditorService service = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            if (service != null)
            {
                ApiManagerForm dialog = new ApiManagerForm();
                SingleProvider privoder = context.Instance as SingleProvider;
                dialog.Init(privoder);
                service.ShowDialog(dialog);
                privoder.Save();
            }
            return value;
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if ((context != null) && (context.Instance != null))
            {
                return UITypeEditorEditStyle.Modal;
            }
            return base.GetEditStyle(context);
        }
    }
}
