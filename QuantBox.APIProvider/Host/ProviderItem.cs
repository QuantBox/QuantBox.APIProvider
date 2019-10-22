using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantBox.APIProvider
{
    [DefaultPropertyAttribute("Name")]
    public class ProviderItem
    {
        public byte Id { get; set; }
        public string Name { get; set; }
    }
}
