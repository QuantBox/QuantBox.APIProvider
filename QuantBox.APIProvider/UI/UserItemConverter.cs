using QuantBox.APIProvider.Single;

using System.ComponentModel;


namespace QuantBox.APIProvider.UI
{
    public class UserItemConverter : ComboBoxItemTypeConvert
    {
        public override void GetConvertHash(ITypeDescriptorContext context)
        {
            _hash.Clear();

            if ((context != null) && (context.Instance != null))
            {
                ApiItem instance = (ApiItem)context.Instance;
                int i = 0;
                foreach (var it in instance.LinkedUserList)
                {
                    _hash[i] = it;
                    ++i;
                }
            }
        }
    }
}
