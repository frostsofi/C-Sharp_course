using MassTransit;
using Microsoft.Extensions.Configuration;
using System;
using MassTransit.AspNetCoreIntegration;
using WebBooksOrder.MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace WebBooksOrder
{
  public class MassTransitConfiguration
  {
    public static string _endPoint { get; set; }
    public static void ConfigureServices(IServiceCollection iServices, IConfiguration iConfiguration)
    {
      var massTransitSection = iConfiguration.GetSection("MassTransit");
      var url = massTransitSection.GetValue<string>("Url");
      var host = massTransitSection.GetValue<string>("Host");
      var userName = massTransitSection.GetValue<string>("UserName");
      var password = massTransitSection.GetValue<string>("Password");

      _endPoint = $"rabbitmq://{url}/{host}";

      if (massTransitSection == null || url == null || host == null)
      {
        throw new ApplicationException("Section 'mass-transit' configuration settings are not found in appSettings.json");
      }

      iServices.AddMassTransit(x =>
      {
        return Bus.Factory.CreateUsingRabbitMq(cfg =>
          {
            var hos = cfg.Host(new Uri(_endPoint), configurator =>
            {
              configurator.Username(userName);
              configurator.Password(password);
            });

            cfg.ReceiveEndpoint(hos,
            "GetBooksQueue", ep =>
            {
              ep.PrefetchCount = 1;
              ep.ConfigureConsumer<GetBooksConsumer>(x);
            });

            cfg.UseJsonSerializer();
          });
      }, ispc =>
      {
        ispc.AddConsumers(typeof(GetBooksConsumer).Assembly);
      });

    }
  }
}
