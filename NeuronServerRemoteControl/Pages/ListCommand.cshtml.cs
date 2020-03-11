using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using NSRcommon;

namespace NeuronServerRemoteControl.Pages
{
    public class ListCommandModel : PageModel
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

        public IActionResult OnGet(String passedObject)
        {
            ServerName = JsonConvert.DeserializeObject<String>(passedObject);

            ViewData["Model"] = SentCommands.ToList();
            return Page();
        }

        public IActionResult OnGetListCommand(String passedObject)
        {
            ListCommandModel model = new ListCommandModel();
            model.ServerName = JsonConvert.DeserializeObject<String>(passedObject);

            var myViewData = new ViewDataDictionary(new Microsoft.AspNetCore.Mvc.ModelBinding.EmptyModelMetadataProvider(), new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary()) { { "ListCommandModel", model } };
            myViewData.Model = model;


            PartialViewResult result = new PartialViewResult()
            {
                ViewName = "ListCommand",
                ViewData = myViewData,
            };

            return result;
        }
    }
}