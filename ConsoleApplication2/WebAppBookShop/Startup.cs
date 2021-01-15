using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebAppBookShop.Services;
using WebAppBookShop.Jobs;
using WebAppBookShop.MassTrasit;
using Quartz.Spi;
using QuartzWithScopedServices;

namespace WebAppBookShop
{
  public class Startup
  {
    public Startup(IConfiguration iConfiguration)
    {
      Configuration = iConfiguration;
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

      iServices.AddSingleton<GetBooksProducer>();
      iServices.AddSingleton<IJobFactory, SingletonJobFactory>();
      iServices.AddSingleton<CheckingEnoughBooks>();
      iServices.AddHostedService<QuartzHostedService>();
      iServices.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
      iServices.AddSingleton(isp => new BookShopService());
      MassTransitConfiguration.ConfigureServices(iServices, Configuration);
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
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
      }

      app.UseHttpsRedirection();
      app.UseStaticFiles();
      app.UseCookiePolicy();

      app.UseMvc(routes =>
      {
        routes.MapRoute(
                  name: "default",
                  template: "{controller=Home}/{action=Index}/{id?}");
      });
    }
  }
}
