using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace WebAppBookShop
{
  #warning зачем создавать DBsettings.json, если есть appsettings.json, туда можно и поместить ConnectionString к базе
  #warning не обнаружил джобов, 
  #warning не обнаружил работы с RMQ 
  #warning выпрямить структуру проектов, сейчас каждый из них можно запустить поотдельности, но целостного приложения нет 
  #warning в Райдере Shift+Alt+L, в студии, кажется, Ctrl+K+D — не забывай делать форматирование кода 
  public class Program
  {
    public static void Main(string[] args)
    {
      CreateWebHostBuilder(args).Build().Run();
    }

    public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>();
  }
}
