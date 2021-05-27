using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRSConeMUVerify.Models
{
   public class OutputFacXmlModel
   {
      public string SourcePhantomDistance { get; set; }
      public string DetectorDepth { get; set; }
      public string JawOpening { get; set; }
      public string ImportedFromFastPlan { get; set; }
      public List<OutPutFactor> OutPutFactors { get; set; }
      public OutputFacXmlModel()
      {
         OutPutFactors = new List<OutPutFactor>();
      }

      public class OutPutFactor
      {
         public double ConeSize { get; set; }
         public double OutputFactorValue { get; set; }
      }
   }
}
