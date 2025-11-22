using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KASHOP.DAL.Models
{
    public class BaseModel
    {
        public int Id{get; set;}
        public Status status{get; set;} = Status.Active;
        public DateTime CreatedAt{get; set;} = DateTime.UtcNow;
    }
}