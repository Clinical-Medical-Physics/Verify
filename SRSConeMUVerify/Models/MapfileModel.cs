using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRSConeMUVerify.Models
{
   public class MapfileModel
   {
      public List<CodeSet> CodeSets { get; set; }
      public List<DataSet> DataSets { get; set; }

      public MapfileModel()
      {
         CodeSets = new List<CodeSet>();
         DataSets = new List<DataSet>();
      }

   }
   public class CodeSet
   {
      public string MachineCode { get; set; }
      public List<string> TreatmentMachine { get; set; }
      public List<string> AddOn { get; set; }
   }
   public class DataSet
   {
      public string MachineCode { get; set; }
      public string GeneralParameters { get; set; }
      public string ModelParameters { get; set; }
      public string AddOn { get; set; }
      public string FastPlanImportInfo { get; set; }
      public string AbsoluteDoseCalibration { get; set; }
      public string OutputFactorTable { get; set; }
      public string TMR { get; set; }
      public string OPP { get; set; }
      public string TMR_processed { get; set; }
      public string OPP_processed { get; set; }
      public string TMR_calculated { get; set; }
      public string OPP_calculated { get; set; }
      public string ALL_histogram { get; set; }
      public string TMR_error { get; set; }
      public string OPP_error { get; set; }

   }
}
