using QuantBox.APIProvider.Single;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using XAPI;

namespace QuantBox.APIProvider.UI
{
    class ApiTypeSelectorEditor : ObjectSelectorEditor
    {
        private ObjectSelectorEditor.Selector selector;

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            base.EditValue(context, provider, value);
            this.selector.BeforeSelect -= new TreeViewCancelEventHandler(this.method_0);
            int num = 0;
            foreach (ObjectSelectorEditor.SelectorNode node in this.selector.Nodes)
            {
                if (node.Checked)
                {
                    num |= (int)node.value;
                }
            }
            return (ApiType)num;
        }

        protected override void FillTreeWithData(ObjectSelectorEditor.Selector selector, ITypeDescriptorContext context, IServiceProvider provider)
        {
            if ((context != null) && (context.Instance != null))
            {
                ApiItem instance = (ApiItem)context.Instance;
                this.selector = selector;
                selector.CheckBoxes = true;
                selector.BeforeSelect += new TreeViewCancelEventHandler(this.method_0);
                selector.Clear();
                foreach (ApiType category in Enum.GetValues(typeof(ApiType)))
                {
                    if (category != ApiType.None)
                    {
                        if((instance.Type & category) == category)
                        {
                            selector.AddNode(category.ToString(), (int)category, null).Checked = (instance.UseType & category) == category;
                        }
                    }
                }
                selector.SelectedNode = null;
            }
            else
            {
                base.FillTreeWithData(selector, context, provider);
            }
        }

        private void method_0(object sender, TreeViewCancelEventArgs e)
        {
            e.Cancel = true;
        }
    }

}
