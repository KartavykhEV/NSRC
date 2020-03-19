using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using NSRcommon;

namespace NeuronServerRemoteControl.Pages
{
    public class ViewServerModel : PageModel
    {
        /// <summary>
        /// сервер
        /// </summary>
        private NSRCserver server => NSRCservice.servers.FirstOrDefault(i => i.name.Equals(ServerName, StringComparison.InvariantCultureIgnoreCase));

        /// <summary>
        /// Кол-во отправленных команд
        /// </summary>
        public int SentCommandsCount => server.SentCommands.Count;

        /// <summary>
        /// Отправленные команды
        /// </summary>
        public IEnumerable<NSRCcommand> SentCommands => server.SentCommands.OrderByDescending(i => i.CreateDate);

        /// <summary>
        /// Имя сервера
        /// </summary>
        public String ServerName { get; set; }

        /// <summary>
        /// Вывод в консоль
        /// </summary>
        public String ConsoleText {
            get {
                List<string> output = new List<string>();
                foreach (NSRCcommand command in SentCommands.OrderBy(i=>i.CreateDate))
                {
                    output.Add($"console\\> {command.CommandText}");
                    if (command.State == CommandState.Success)
                        output.Add(command.ResponseText);
                }
                return String.Join("\n", output);
            }
        }

        public IActionResult OnGet(string passedObject)
        {
            ServerName = JsonConvert.DeserializeObject<String>(passedObject);

            if (server == null)
                return NotFound();

            ViewData["ServerName"] = ServerName;
            return Page();
        }

        public async Task<JsonResult> OnPost()
        {
            ServerName = Request.Form["ServerName"];
            NSRCcommand command = new NSRCcommand() { CommandText = Request.Form["CommandText"] };
            server.AddCommand(command);

            Guid commandId = command.Id;
            await Task.Run(() => waitCommand(commandId));

            ViewData["ServerName"] = ServerName;

            var sentCommand = server.SentCommands.Where(t => t.Id == commandId).FirstOrDefault();
            return new JsonResult(JsonConvert.SerializeObject(new {
                CommandText = sentCommand.CommandText,
                CreateDate = sentCommand.CreateDate.ToString("dd.MM.yyyy HH:mm:ss"),
                ResponseText = sentCommand.ResponseText
            }));
        }

        /// <summary>
        /// Ждем ответа от команды
        /// </summary>
        /// <param name="id">Идентификатор команды</param>
        public void waitCommand(Guid id)
        {
            Boolean result = false;
            Int32 countSent = 10; //количество попыток ожидания ответа от команды
            Int32 sent = 0;
            while (!result && sent < countSent)
            {
                Thread.Sleep(500);
                NSRCcommand command = server.SentCommands.Where(t => t.Id == id).FirstOrDefault();
                if(command != null)
                {
                    if (command.State == CommandState.Sent)
                        sent++;
                    result =  command.State == CommandState.Success || command.State == CommandState.Error;
                }
            }
        }

        public JsonResult OnGetConsole(string passedObject)
        {
            ServerName = JsonConvert.DeserializeObject<String>(passedObject);
            return new JsonResult(JsonConvert.SerializeObject(this.ConsoleText));
        }


        public JsonResult OnGetListCommand(string passedObject)
        {
            ServerName = JsonConvert.DeserializeObject<String>(passedObject);
            if (this.SentCommandsCount > 0)
            {
                var res = JsonConvert.SerializeObject(this.SentCommands.ToList().ConvertAll(t => new
                {
                    CommandText = t.CommandText,
                    CreateDate = t.CreateDate.ToString("dd.MM.yyyy HH:mm:ss"),
                    ResponseText = t.ResponseText
                }));
                return new JsonResult(res);
            }
            else
                return new JsonResult(null);
        }
    }
}
