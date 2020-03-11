using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Threading.Tasks; 
using Microsoft.AspNetCore.Mvc; 
using Microsoft.AspNetCore.Mvc.RazorPages; 

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
    }
}
