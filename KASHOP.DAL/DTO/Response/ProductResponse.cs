using System;
using System.Text.Json.Serialization;
using KASHOP.DAL.Models;

namespace KASHOP.DAL.DTO.Response;

public class ProductResponse
{
    public int Id { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Status status { get; set; }
    public string CreatedBy { get; set; }
    public string MainImage{get;set;}
    public List<ProductTranslationResponse> Translations{get;set;}
}
