using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.ServiceModel; 
using System.Threading.Tasks; 
using Microsoft.AspNetCore.Builder; 
using Microsoft.AspNetCore.Hosting; 
using Microsoft.AspNetCore.Http; 
using Microsoft.AspNetCore.HttpsPolicy; 
using Microsoft.AspNetCore.Mvc; 
using Microsoft.Extensions.Configuration; 
using Microsoft.Extensions.DependencyInjection; 
using NSRcommon; 
using SoapCore; 

namespace NeuronServerRemoteControl
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration; 
        }

        public IConfiguration Configuration { get;  }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true; 
                options.MinimumSameSitePolicy = SameSiteMode.None; 
            });
            services.AddHttpsRedirection(options =>
            {
                options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
                options.HttpsPort = Convert.ToInt32(Configuration["https_port"]);
                
            });
            services.AddSingleton(typeof(INSRCservice), new NSRCservice()); 

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2); 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(); 
            }
            else
            {
                app.UseExceptionHandler("/Error"); 
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts(); 
            }

            app.UseHttpsRedirection(); 
            app.UseStaticFiles(); 
            //app.UseCookiePolicy(); 

            BasicHttpsBinding bind = new BasicHttpsBinding(); 
            bind.Security.Mode = BasicHttpsSecurityMode.Transport;
            app.UseSoapEndpoint<INSRCservice>(path: "/nsrc_service.asmx", binding: bind);

            app.UseMvc(); 
        }
    }
}
