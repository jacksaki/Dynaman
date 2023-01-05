using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Livet;
using Npgsql;
namespace Dynaman {
    public class DbConnectionParameter {
        public static DbConnectionParameter CreateFromXElement(XElement element) {
            var p = new DbConnectionParameter();
            p.ConnectionString = element.Attribute("ConnectionString")?.Value;
            if (element.Element("SshParameter") != null) {
                p.UseSsh = "Y".Equals(element.Attribute("UseSsh")?.Value, StringComparison.OrdinalIgnoreCase);
                p.SshParameter = SecureShellParameter.CreateFromXElement(element.Element("SshParameter"));
            }
            return p;
        }
        public string ConnectionString {
            get;
            private set;
        } = "";

        public void Connect() {
            using(var q = new PgQuery(this)) {

            }
        }

        public bool UseSsh {
            get;
            private set;
        }

        public SecureShellParameter SshParameter {
            get;
            set;
        }
    }
}
