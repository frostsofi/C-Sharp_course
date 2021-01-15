﻿using MassTransit;
using Microsoft.Extensions.Configuration;
using System;
using MassTransit.AspNetCoreIntegration;
using Microsoft.Extensions.DependencyInjection;
using WebAppBookShop.MassTrasit;

namespace WebAppBookShop
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
          var hostCfg = cfg.Host(new Uri(_endPoint), configurator =>
          {
            configurator.Username(userName);
            configurator.Password(password);
          });

          cfg.ReceiveEndpoint(hostCfg,
          "SendBooksQueue", ep =>
          {
            ep.PrefetchCount = 1;
            ep.ConfigureConsumer<SendBooksConsumer>(x);
          });
          cfg.UseJsonSerializer();
        });
      }, ispc =>
      {
        ispc.AddConsumers(typeof(SendBooksConsumer).Assembly);
      });

    }
  }
}