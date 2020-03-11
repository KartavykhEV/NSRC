using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Threading.Tasks; 
using Microsoft.AspNetCore.Mvc; 
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace NeuronServerRemoteControl.Pages
{
    public class IndexModel : PageModel
    {
        /// <summary>
        /// Подключенные сервера
        /// </summary>
        public IEnumerable<NSRCserver> connectedServers => NSRCservice.servers;



        public void OnGet()
        {

        }

        public JsonResult OnGetListServers()
        {
            var res = JsonConvert.SerializeObject(connectedServers.ToList().ConvertAll(t => new { name = t.name, lastConnect = t.lastConnect.ToString("dd.MM.yyyy HH:mm") } ));
            return new JsonResult(res);
        }
    }
}
