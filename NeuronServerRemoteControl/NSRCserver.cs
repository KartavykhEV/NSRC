using System; 
using System.Collections.Generic; 
using NSRcommon; 

namespace NeuronServerRemoteControl
{

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


        /// <summary>
        /// Команды на выполнение
        /// </summary>
        internal Queue<NSRCcommand> Commands { get;  set;  } = new Queue<NSRCcommand>(); 

        public NSRCserver()
        {
        }
    }
}
