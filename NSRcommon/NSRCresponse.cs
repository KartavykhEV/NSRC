using System;
using System.Runtime.Serialization;

namespace NSRcommon
{
    /// <summary>
    /// Ответ системы
    /// </summary>
    [DataContract]
    public class NSRCresponse
    {
        /// <summary>
        /// Идентификатор команды
        /// </summary>
        [DataMember]
        public Guid Id { get; protected set; }

        /// <summary>
        /// Признак ее выполнения
        /// </summary>
        [DataMember]
        public Boolean Success { get; set; } = false;

        /// <summary>
        /// Собственно ответ
        /// </summary>
        [DataMember]
        public String Response { get; set; }

        public NSRCresponse(Guid id)
        {
            Id = id;
        }
    }
}
