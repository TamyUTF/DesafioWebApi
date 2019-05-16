using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DesafioWebApi.Model
{
    public class Planner
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public float Cost { get; set; }
        public TypePlan Type{ get; set; }
        public User Responsible { get; set; }
        public Status Status { get; set; }
        public List<User> Interested { get; set; }
    }
}
