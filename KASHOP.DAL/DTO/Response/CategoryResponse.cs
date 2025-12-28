using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KASHOP.DAL.Models;

namespace KASHOP.DAL.DTO.Response
{
    public class CategoryResponse
    {
        public int Id {get;set;}
        public Status status {get;set;}
        public string CreatedBy {get; set;}

         public List<CategoryTranslationResponse> Trinslations {get;set;}
       
    }
}