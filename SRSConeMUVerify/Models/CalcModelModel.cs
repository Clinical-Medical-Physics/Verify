using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRSConeMUVerify.Models
{
   //DataColumn name = new DataColumn("Name", typeof(string));
   //DataColumn algoName = new DataColumn("Algorithm Name");
   //DataColumn algoVer = new DataColumn("Algorithm Version");
   //DataColumn beamDatDir = new DataColumn("Beam Data Directory");
   //DataColumn enabled = new DataColumn("Enabled");
   public class CalcModelModel
   {
      public string Name { get; set; }
      public string AlgorithmName { get; set; }
      public string AlgorithmVersion { get; set; }
      public string BeamDataDirectory { get; set; }
      public string Enabled { get; set; }
   }
}
