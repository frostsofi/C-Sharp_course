using MassTransit;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebBooksOrder.MassTransit
{
  public class SendBooksProducer
  {
    private readonly ISendEndpointProvider _sendEndpointProvider;
    private readonly IConfiguration _configuration;

    public SendBooksProducer(ISendEndpointProvider iSendEndpointProvider, IConfiguration iConfiguration)
    {
      _sendEndpointProvider = iSendEndpointProvider;
      _configuration = iConfiguration;
    }

    public async Task SentBooks(List<BookShopContract.Book> iBooks)
    {
      var message = new SendBooksContract
      {
        _books = iBooks
      };

      var hostConfig = new MassTransitConfiguration();
      _configuration.GetSection("MassTransit").Bind(hostConfig);
      var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri(MassTransitConfiguration._endPoint + '/' + "SendBooksQueue"));
      await endpoint.Send(message);
    }
  }
}
