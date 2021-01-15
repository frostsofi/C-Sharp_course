using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Configuration;

namespace WebAppBookShop.MassTrasit
{
    public class GetBooksProducer
    {
        private readonly ISendEndpointProvider _sendEndpointProvider;
        private readonly IConfiguration _configuration;

        public GetBooksProducer(ISendEndpointProvider iSendEndpointProvider, IConfiguration iConfiguration)
        {
            _sendEndpointProvider = iSendEndpointProvider;
            _configuration = iConfiguration;
        }

        public async Task SentNumberOfBooks()
        {
            var message = new GetBooksContract
            {
                _numberBooks = 5
            };

            var hostConfig = new MassTransitConfiguration();
            _configuration.GetSection("MassTransit").Bind(hostConfig);
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri(MassTransitConfiguration._endPoint + '/' + "GetBooksQueue"));
            await endpoint.Send(message);
        }
    }
}