using DesafioWebApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DesafioWebApi.Filters
{
    public static class PlannerFilterExtensions
    {
        public static IEnumerable<Planner> Filter(this IEnumerable<Planner> query, PlannerFilter filter)
        {
            if (filter != null){
                if (!string.IsNullOrEmpty(filter.Status))
                {
                    query = query.Where(p => p.Status.Name.ToLower().Contains(filter.Status.ToLower()));
                }
            }
            return query;
        }
    }
    public class PlannerFilter
    {
        public string Status { get; set; }
    }
}
