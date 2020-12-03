using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using BookShopSystem;

namespace Service
{

  #warning непонятно, зачем этот проект  - console application и зачем прям в main'e ты делаешь запрос 
  #warning классы разнести по файлам
  public class ServiceProxy
  {
    private readonly HttpClient _httpClient;

    public ServiceProxy(HttpClient iHttpClient)
    {
      _httpClient = iHttpClient;
    }

    public async Task<T> GetData<T>(string iUrl) where T:class
    {
      var responce = await GetJsonResponceAsync<T>(iUrl, HttpMethod.Get);
      return responce;
    }

    private async Task<T> GetJsonResponceAsync<T>(string iUrl, HttpMethod iMethod, string iContent = null) where T:class
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

      }
    }

    static void Main(string[] args)
    {
      ServiceProxy proxy = new ServiceProxy(new HttpClient());
      var task = proxy.GetData<List<Book>>("https://getbooksrestapi.azurewebsites.net/api/books/5");

      task.Wait();
      var val = task.Result;
    }
  }

}
