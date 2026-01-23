using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KASHOP.DAL.Models
{
    public class Category : BaseModel
    {
        
        public List<CategoryTrinslation> Trinslations{get; set;}
        public List<Product> Products{get; set;}
    }
}