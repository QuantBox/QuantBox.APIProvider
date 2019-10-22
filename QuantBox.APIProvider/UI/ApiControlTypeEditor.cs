﻿using System;
using System.ComponentModel;

using QuantBox.APIProvider.Single;

namespace QuantBox.APIProvider.UI
{
#if NET48
    using System.Drawing.Design;
    using System.Windows.Forms.Design;

    class ApiControlTypeEditor : UITypeEditor
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
                ApiControlForm dialog = new ApiControlForm();
                SingleProvider privoder = context.Instance as SingleProvider;
                dialog.Init(privoder);
                //service.DropDownControl(dialog);
                //privoder.Save();
                dialog.Show();
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
#endif
}
