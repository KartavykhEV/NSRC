using System; 
using System.Collections.Generic;
using System.Linq;
using NSRcommon; 

namespace NeuronServerRemoteControl
{
    public enum ServerState
    {
        Active,
        Waiting,
        Disctonnected,
    }


    /// <summary>
    /// Управляемый элемент
    /// </summary>
    public class NSRCserver
    {
        /// <summary>
        /// Название сервера
        /// </summary>
        public String name { get;  set;  }

        /// <summary>
        /// Дата, время последнего подключения
        /// </summary>
        public DateTime lastConnect { get;  set;  }

        public ServerState State => (DateTime.Now - lastConnect) < TimeSpan.FromSeconds(3) ? ServerState.Active :
            ((DateTime.Now - lastConnect) < TimeSpan.FromSeconds(70) ? ServerState.Waiting : ServerState.Disctonnected);


        /// <summary>
        /// Команды на выполнение
        /// </summary>
        private Queue<NSRCcommand> Commands { get;  set;  } = new Queue<NSRCcommand>();


        /// <summary>
        /// Отправленные команды
        /// </summary>
        public List<NSRCcommand> SentCommands { get; set; } = new List<NSRCcommand>();

        /// <summary>
        /// Добавить команду
        /// </summary>
        /// <param name="command"></param>
        internal void AddCommand(NSRCcommand command)
        {
            Commands.Enqueue(command);
        }

        /// <summary>
        /// Взять следующую команду из очереди
        /// </summary>
        /// <returns></returns>
        public NSRCcommand GetCommand()
        {
            if (Commands.Count == 0) return null;
            NSRCcommand cmd = Commands.Dequeue();
            SentCommands.Add(cmd);
            cmd.State = CommandState.Sent;

            return cmd;
        }
       internal void AddResponse(NSRCresponse response)
        {
            NSRCcommand cmd = SentCommands.FirstOrDefault(i => i.Id == response.Id);
            if (cmd != null)
                cmd.SetResponse(response);
        }


        public NSRCserver()
        {
        }

     }
}
