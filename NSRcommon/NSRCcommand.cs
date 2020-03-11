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
        /// <summary>
        /// Текст команды
        /// </summary>
        [DataMember]
        public String  CommandText { get; set; }

        /// <summary>
        /// Дата создания команды
        /// </summary>
        public DateTime CreateDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Ответ при выполнении команды
        /// </summary>
        public String ResponseText { get; set; }

        /// <summary>
        /// Идентификатор команды
        /// </summary>
        [DataMember]
        public Guid Id { get; protected set; } = Guid.NewGuid();

        /// <summary>
        /// Состояние команды
        /// </summary>
        public CommandState State { get; set; } = CommandState.Created;

        public NSRCcommand()
        {
        }

        public void SetResponse(NSRCresponse response)
        {
            State = response.Success ? CommandState.Success : CommandState.Error;
            ResponseText = response.Response;
        }
    }


    /// <summary>
    /// Состояние команды
    /// </summary>
    public enum CommandState
    {
        Created,
        Sent,
        Success,
        Error,
    }
}
