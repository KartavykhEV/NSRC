using System;
using System.Runtime.Serialization;

namespace NSRcommon
{
    /// <summary>
    /// Команда на выполнение сервером
    /// </summary>
    [DataContract]
    public class NSRCcommand
    {
        [DataMember]
        public String Text { get; set; }


        public NSRCcommand()
        {
        }
    }
}
