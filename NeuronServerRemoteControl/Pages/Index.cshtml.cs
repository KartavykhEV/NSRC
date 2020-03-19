using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace NeuronServerRemoteControl.Pages
{
    public class IndexModel : PageModel
    {
        /// <summary>
        /// Подключенные сервера
        /// </summary>
        public IEnumerable<NSRCserver> connectedServers => NSRCservice.servers;

        private readonly IConfiguration configuration;
        public IndexModel(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        [BindProperty]
        public string UserName { get; set; }
        [BindProperty, DataType(DataType.Password)]
        public string Password { get; set; }
        public string Message { get; set; }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            var user = configuration.GetSection("ControlUser").Get<ControlUser>();
            if (!String.IsNullOrEmpty(UserName) && UserName.Equals(user.UserName, StringComparison.InvariantCultureIgnoreCase) && Password == user.Password)
            {
                //var passwordHasher = new PasswordHasher<string>();
                //if (passwordHasher.VerifyHashedPassword(null, user.Password, Password) == PasswordVerificationResult.Success)
                //{
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, UserName)
                    };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                returnUrl = returnUrl ?? Url.Content("~/Index");
                //return RedirectToPage("~" + returnUrl);
                return Redirect(returnUrl);
                //}
            }
            Message = "Имя пользователя или пароль неверны";
            return Page();
        }

        public void OnGet()
        {

        }

        public JsonResult OnGetListServers()
        {
            var res = JsonConvert.SerializeObject(connectedServers.ToList().ConvertAll(t => new
            {
                name = t.name,
                lastConnect = t.lastConnect.ToString("dd.MM.yyyy HH:mm"),
                State = t.State.ToString()
            }));
            return new JsonResult(res);
        }
    }
}
