using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrarySocket
{
    [Serializable]
    public class Request
    {
        public RemoteMethod MethodNumber;
        public object Parameter;
    }
}
