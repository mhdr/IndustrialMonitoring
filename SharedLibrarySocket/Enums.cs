using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrarySocket
{
    public enum MotorStatus
    {
        Off=0,
        Speed1=1,
        Speed2=2,
        Speed3=3
    }

    public enum RemoteMethod
    {
        GetStatus2=1,
        SetStatus=2,
        Authorize=3,
        AuthorizeAndGetSession=4
    }
}
