using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRSConeMUVerify.Models
{
   public class PlanPrescriptionModel
   {
      public double DosePerFraction { get; set; }
      public double NumberOfFraction { get; set; }
      public double TotalDose { get; set; }
      public double TreatmentPercentage { get; set; }
      public double TotalWeight { get; set; }
      public double DoseMaximum3D { get; set; }
      public double RepeatFactor { get; set; }
      public double WeightAtDoseMaximum { get; set; }
      public double DoseIsocenter { get; set; }
      public PlanPrescriptionModel(double dosePerFraction, double numberOfFractions,
         double totalDose, double treatmentPercentage, double totalWeight,
         double doseMaximum, double doseIso)
      {
         DosePerFraction = dosePerFraction;
         NumberOfFraction = numberOfFractions;
         TotalDose = totalDose;
         TreatmentPercentage = treatmentPercentage;
         TotalWeight = totalWeight;
         DoseMaximum3D = doseMaximum;
         DoseIsocenter = doseIso;
         
         WeightAtDoseMaximum = getWeightAtDoseMaximum();
         RepeatFactor = getRepeatFactor();

      }

      private double getWeightAtDoseMaximum()
      {
         return DoseMaximum3D * TotalWeight / DoseIsocenter;
      }

      private double getRepeatFactor()
      {
         return (TotalDose / TreatmentPercentage) / WeightAtDoseMaximum;
      }
   }
}
