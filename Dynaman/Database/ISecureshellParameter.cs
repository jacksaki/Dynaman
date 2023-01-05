using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
namespace Dynaman {
    public interface ISecureshellParameter:IDisposable {
        void Open();
        void Close();
        XElement ToXElement();
        string HostName {
            get;
            set;
        }
        int? Port {
            get;
            set;
        }
        string UserName {
            get;
            set;
        }
        string Password {
            get;
            set;
        }

        uint? LocalPort {
            get;
            set;
        }
        string DbHostName {
            get;
            set;
        }
        uint? DbPort {
            get;
            set;
        }
        string PrivateKeyFile {
            get;
            set;
        }

        void Validate();
    }
}
