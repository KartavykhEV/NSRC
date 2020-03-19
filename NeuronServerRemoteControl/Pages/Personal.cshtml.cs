using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NeuronServerRemoteControl.Pages
{
    public class PersonalModel : PageModel
    {
        public async Task<IActionResult> OnPostLogOut()
        {
            await HttpContext.SignOutAsync(); // SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
            return RedirectToPage("/index");


        }
    }
}
