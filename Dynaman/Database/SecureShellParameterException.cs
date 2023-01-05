using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynaman {
    public class SecureShellParameterException:ApplicationException {
        public SecureShellParameterException() : base() {

        }
        public SecureShellParameterException(string message) : base(message) {

        }
        public SecureShellParameterException(string message,Exception innerException) : base(message, innerException) {

        }
    }
}
