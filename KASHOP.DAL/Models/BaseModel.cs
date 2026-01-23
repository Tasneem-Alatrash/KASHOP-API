using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace KASHOP.DAL.Models
{
    public class BaseModel
    {
        public int Id{get; set;}
        public Status status{get; set;} = Status.Active;

        public String CreatedBy{get; set;} 
        public DateTime CreatedAt{get; set;}
          public String? UpdatedBy{get; set;} 
        public DateTime? UpdatedAt{get; set;}

        [ForeignKey("CreatedBy")]
        public ApplicationUser User {get; set;}
        

    }
}