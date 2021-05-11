using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRSConeMUVerify.Models
{
    
    public class TMRModel
    {
        public string ConeSize { get; set; }
        public string Energy { get; set; }
        public List<TMRDataPoint> DataPoints { get; set; }
        public TMRModel()
        {
            DataPoints = new List<TMRDataPoint>();
        }
    }
    public class TMRDataPoint
    {
        public double Depth { get; set; }
        public double TMRValue { get; set; }
    }
}
