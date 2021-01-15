using System;
using Newtonsoft.Json;

namespace BookShopCore
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
    public Book()
    {
    }

    public Book(int iGuid, string iName, string iGenre, DateTime iDate, double iCost)
    {
      Id = iGuid;
      Name = iName;
      Genre = iGenre;
      ReceiptDate = iDate;
      Cost = iCost;
    }
  }
}
