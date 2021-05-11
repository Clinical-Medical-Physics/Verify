using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRSConeMUVerify.Models
{
    public class CourseModel
    {
        public string CourseId { get; set; }
        public List<PlanModel> Plans { get; set; }
        public CourseModel()
        {
            Plans = new List<PlanModel>();
        }
    }
}
