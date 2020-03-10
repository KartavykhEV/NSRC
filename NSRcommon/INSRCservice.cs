using System;
using System.ServiceModel;

namespace NSRcommon
{



    /// <summary>
    /// 
    /// </summary>
    [ServiceContract]
    public interface INSRCservice
    {
        [OperationContract]
        NSRCcommand Connect(String name); 

    }
}
