using QuantBox.APIProvider.Single;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantBox.APIProvider.UI
{
    public class ServerItemConverter : ComboBoxItemTypeConvert
    {
        public override void GetConvertHash(ITypeDescriptorContext context)
        {
            _hash.Clear();

            if ((context != null) && (context.Instance != null))
            {
                ApiItem instance = (ApiItem)context.Instance;
                int i = 0;
                foreach (var it in instance.LinkedServerList)
                {
                    _hash[i] = it;
                    ++i;
                }
            }
        }
    }
}
