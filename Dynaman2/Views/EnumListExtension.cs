using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Dynaman2.Views {
    public class EnumListExtension : MarkupExtension {
        private Type enumType;
        public EnumListExtension(Type type) {
            enumType = type;
        }
        public override object ProvideValue(IServiceProvider serviceProvider) {
            return EnumsNET.Enums.GetValues(enumType);
        }
    }
}
