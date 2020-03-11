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

     /*   public PartialViewResult GetListCommand()
        {
            if(this.SentCommandsCount > 0)
            {
            foreach (var item in this.SentCommands)
            {
                <p>@($"{item.CreateDate.ToString("dd.MM.yyyy HH:mm:ss")} {item.CommandText}")</p>
                <p>@item.ResponseText</p>
            }

            Cars = _carService.GetAll();
            return Partial("_CarPartial", Cars);

           /*return new PartialViewResult
            {
                ViewName = "_CarPartial",
                ViewData = new ViewDataDictionary<List<Car>>(ViewData, Cars)
            };
        }*/

        //public IActionResult OnGet
    }
}
