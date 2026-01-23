using System;

namespace KASHOP.DAL.Models;

public class Product : BaseModel
{
    public decimal Price{get;set;}
    public decimal Discount{get;set;}
    public int Quantity{get;set;}
    public double Rate{get;set;}
    public string MainImage{get;set;}
    public int CategoryId{get;set;}
    public Category Category{get;set;}
    public List<ProductTranslation> Translation{get;set;} 
}
