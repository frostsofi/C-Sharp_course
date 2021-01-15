using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebBooksOrder.MassTransit;

namespace WebBooksOrder
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection iServices)
    {
      iServices.Configure<CookiePolicyOptions>(options =>
      {
        // This lambda determines whether user consent for non-essential cookies is needed for a given request.
        options.CheckConsentNeeded = context => true;
        options.MinimumSameSitePolicy = SameSiteMode.None;
      });

      iServices.AddSingleton<SendBooksProducer>();
      MassTransitConfiguration.ConfigureServices(iServices, Configuration);
      iServices.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder iApp, IHostingEnvironment iEnv)
    {
      if (iEnv.IsDevelopment())
      {
        iApp.UseDeveloperExceptionPage();
      }
      else
      {
        iApp.UseExceptionHandler("/Home/Error");
        iApp.UseHsts();
      }

      iApp.UseHttpsRedirection();
      iApp.UseStaticFiles();
      iApp.UseCookiePolicy();

      iApp.UseMvc(routes =>
      {
        routes.MapRoute(
                  name: "default",
                  template: "{controller=Home}/{action=Index}/{id?}");
      });
    }
  }
}
