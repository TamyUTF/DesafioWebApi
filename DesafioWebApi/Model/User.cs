using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DesafioWebApi.Model
{
    public class User
   {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime RegisterDate { get; set; }
        public DateTime LastChangedDate { get; set; }
        public int CanCreatePlan { get; set; }
        public int Removed { get; set; }
    }
}
