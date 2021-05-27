using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRSConeMUVerify.Models
{
   public class TmrXmlModel
   {
      
      public string SourceDetectorDistance { get; set; }
      public string ImportedFromFastPlan { get; set; }

      public List<TmrCurve> TmrCurves { get; set; }
      public List<TmrCurve> TmrCalcCurves { get; set; }
      public TmrXmlModel()
      {
         TmrCurves = new List<TmrCurve>();
      }
      public class TmrCurve
      {
         public string Id { get; set; }
         public string FieldSize { get; set; }
         public string Values { get; set; }
      }
   }
}
