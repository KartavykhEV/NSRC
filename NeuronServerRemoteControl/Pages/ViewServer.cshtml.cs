using System;
using System.Collections.Generic;
using System.Linq;
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

        public IActionResult OnGet(string passedObject)
        {
            ServerName = JsonConvert.DeserializeObject<String>(passedObject);

            if (server == null)
                return NotFound();

            ViewData["ServerName"] = ServerName;
            return Page();
        }

        public void OnPost()
        {
            ServerName = Request.Form["ServerName"];
            NSRCcommand command = new NSRCcommand() { CommandText = Request.Form["CommandText"] };
            server.AddCommand(command);
            ViewData["ServerName"] = ServerName;
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
