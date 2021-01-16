using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

#warning неверный namespace, проверь везде, пожалуйста
namespace ServiceProxy
{
  public class ServiceProxy
  {
    private readonly HttpClient _httpClient;

    public ServiceProxy(HttpClient iHttpClient)
    {
      _httpClient = iHttpClient;
    }

    public async Task<T> GetData<T>(string iUrl) where T : class
    {
      var responce = await GetJsonResponceAsync<T>(iUrl, HttpMethod.Get);
      return responce;
    }

    private async Task<T> GetJsonResponceAsync<T>(string iUrl, HttpMethod iMethod, string iContent = null) where T : class
    {
      try
      {
        var httpRequest = new HttpRequestMessage(iMethod, iUrl);
        var responce = await _httpClient.SendAsync(httpRequest);
        var json = await responce.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(json);
      }
      finally
      {
        #warning почему на finally "request failed"?)
        ServicePointManager.ServerCertificateValidationCallback = null;
      }
    }
  }
}
