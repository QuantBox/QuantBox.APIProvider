using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantBox.APIProvider.UI
{
    public abstract class ComboBoxItemTypeConvert : TypeConverter  
    {
        public SortedDictionary<int,object> _hash = null;  
        public ComboBoxItemTypeConvert()  
        {  
            _hash = new SortedDictionary<int,object>();
        }

        public abstract void GetConvertHash(ITypeDescriptorContext context);  
  
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)  
        {  
            return true;
        }  
  
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)  
        {
            GetConvertHash(context);

            return new StandardValuesCollection(_hash.Keys);
        }  
  
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)  
        {
            if (sourceType == typeof(string))  
            {  
                return true;
            }
  
            return base.CanConvertFrom(context, sourceType);  
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return true;
        }
  
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object v)  
        {  
            if (v is string)
            {  
                foreach (var myDE in _hash)
                {
                    if (myDE.Value.ToString().Equals((v.ToString())))
                        return myDE.Key;
                }
            }
            
            return base.ConvertFrom(context, culture, v);  
        }
  
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture,object v, Type destinationType)  
        {
            GetConvertHash(context);
            if (destinationType == typeof(string))  
            {
                foreach (var myDE in _hash)
                {  
                    if (myDE.Key.Equals(v))
                        return myDE.Value.ToString();
                }
                return "";
            }  
  
            return base.ConvertTo(context, culture, v, destinationType);  
        }  
  
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)  
        {
            return true;
        }
    }
}
