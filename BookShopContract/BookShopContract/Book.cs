using System;
using Newtonsoft.Json;

namespace BookShopContract
{
    public class Book
    {
    [JsonProperty("id")]
    public int Id { get; set; }
    [JsonProperty("title")]
    public string Name { get; set; }
    [JsonProperty("genre")]
    public string Genre { get; set; }
    [JsonProperty("dateOfDelivery")]
    public DateTime ReceiptDate { get; set; }
    [JsonProperty("price")]
    public double Cost { get; set; }
  }
}
