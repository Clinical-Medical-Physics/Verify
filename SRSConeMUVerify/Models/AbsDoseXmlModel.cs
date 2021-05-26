using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRSConeMUVerify.Models
{
   //<Parameter Id = "AbsDoseRefFieldSize" Value="100"/>
   // <Parameter Id = "AbsDoseMeasurementSSD" Value="950"/>
   // <Parameter Id = "AbsDoseMeasurementDepth" Value="50"/>
   // <Parameter Id = "AbsDoseMeasurementGy" Value="1.00"/>
   // <Parameter Id = "AbsDoseMeasurementMU" Value="109.7695"/>
   // <Parameter Id = "ImportedFromFastPlan" Value="false"/>
   public class AbsDoseXmlModel
   {
      public string AbsDoseRefFieldSize { get; set; }
      public string AbsDoseMeasurementSSD { get; set; }
      public string AbsDoseMeasurementDepth { get; set; }
      public string AbsDoseMeasurementGy { get; set; }
      public string AbsDoseMeasurementMU { get; set; }
      public string ImportedFromFastPlan { get; set; }
      public AbsDoseXmlModel()
      {

      }
      public double AbsoluteDoseCalibration()
      {
         // TODO implement
         try
         {
            return Convert.ToDouble(AbsDoseMeasurementGy) * 100.0 / Convert.ToDouble(AbsDoseMeasurementMU);
         }
         catch
         {
            return Double.NaN;
         }
      }
   }
}
