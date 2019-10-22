using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantBox.APIProvider.UI
{
    public class JTypeDescriptor : ICustomTypeDescriptor
    {
        public JTypeDescriptor(JObject jobject)
        {
            if (jobject == null)
                throw new ArgumentNullException("jobject");

            JObject = jobject;
        }

        // NOTE: the property grid needs at least one r/w property otherwise it will not show properly in collection editors...
        public JObject JObject { get; set; }

        public override string ToString()
        {
            // we display this object's serialized json as the display name, for example
            return JObject.ToString(Formatting.None);
        }

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
        {
            // browse the JObject and build a list of pseudo-properties
            List<PropertyDescriptor> props = new List<PropertyDescriptor>();

            foreach (var kv in JObject)
            {
                props.Add(new Prop(kv.Key, kv.Value, null));
            }
            return new PropertyDescriptorCollection(props.ToArray());
        }

        AttributeCollection ICustomTypeDescriptor.GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this, true);
        }

        string ICustomTypeDescriptor.GetClassName()
        {
            return TypeDescriptor.GetClassName(this, true);
        }

        string ICustomTypeDescriptor.GetComponentName()
        {
            return TypeDescriptor.GetComponentName(this, true);
        }

        TypeConverter ICustomTypeDescriptor.GetConverter()
        {
            return TypeDescriptor.GetConverter(this, true);
        }

        EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(this, true);
        }

        PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
        {
            return TypeDescriptor.GetDefaultProperty(this, true);
        }

        object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(this, editorBaseType, true);
        }

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(this, attributes, true);
        }

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
        {
            return TypeDescriptor.GetEvents(this, true);
        }

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
        {
            // 这个地方不一样
            return ((ICustomTypeDescriptor)this).GetProperties(null);
        }

        object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }

        // represents one dynamic pseudo-property
        private class Prop : PropertyDescriptor
        {
            private Type _type;
            private object _value;

            public Prop(string name, object value, Attribute[] attrs)
                : base(name, attrs)
            {
                _value = ComputeValue(value);
                _type = _value == null ? typeof(object) : ((JValue)_value).Value.GetType();
            }

            private static object ComputeValue(object value)
            {
                if (value == null)
                    return null;

                JArray array = value as JArray;
                if (array != null)
                {
                    // we use the arraylist because that's all the property grid needs...
                    ArrayList list = new ArrayList();
                    for (int i = 0; i < array.Count; i++)
                    {
                        JObject subo = array[i] as JObject;
                        if (subo != null)
                        {
                            JTypeDescriptor td = new JTypeDescriptor(subo);
                            list.Add(td);
                        }
                        else
                        {
                            JValue jv = array[i] as JValue;
                            if (jv != null)
                            {
                                list.Add(jv.Value);
                            }
                            else
                            {
                                // etc.
                            }
                        }
                    }
                    // we don't support adding things
                    return ArrayList.ReadOnly(list);
                }
                else
                {
                    // etc.
                }
                return value;
            }

            public override bool CanResetValue(object component)
            {
                return true;
            }

            public override Type ComponentType
            {
                get { return typeof(object); }
            }

            public override object GetValue(object component)
            {
                return _value;
            }

            public override bool IsReadOnly
            {
                get { return false; }
            }

            public override Type PropertyType
            {
                get { return _type; }
            }

            public override void ResetValue(object component)
            {
            }

            public override void SetValue(object component, object value)
            {
                _value = value;
                (component as JTypeDescriptor).JObject[Name] = JToken.FromObject(_value);
                OnValueChanged(component, EventArgs.Empty);
            }

            public override bool ShouldSerializeValue(object component)
            {
                return true;
            }
        }
    }
}
