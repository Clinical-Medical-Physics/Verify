using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRSConeMUVerify.Models
{
   public class CheckedBeamModel
   {
      public string Id { get; set; }
      public string Machine { get; set; }
      public string Energy { get; set; }
      public string ConeSize { get; set; }
      public double AverageDepth { get; set; }
      public double WeightFactor { get; set; }
      public double OutputFactor { get; set; }
      public double TMRValue { get; set; }
      public double RefDose { get; set; }
      public double CalcDose { get; set; }
      public double TPSMU { get; set; }
      public double CalcMU { get; set; }

   }
}
