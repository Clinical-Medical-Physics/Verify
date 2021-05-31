using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRSConeMUVerify.Models
{
   public class PlanPrescriptionModel : BindableBase
   {
      private double _dosePerFraction;

      public double DosePerFraction
      {
         get { return _dosePerFraction; }
         set { SetProperty(ref _dosePerFraction,  value); }
      }
      private double _numFractions;

      public double NumberOfFraction
      {
         get { return _numFractions; }
         set { SetProperty(ref _numFractions,  value); }
      }
      private double _totalDose;

      public double TotalDose
      {
         get { return _totalDose; }
         set { SetProperty(ref _totalDose, value); }
      }
      private double _treatmentPercentage;

      public double TreatmentPercentage
      {
         get { return _treatmentPercentage; }
         set { SetProperty(ref _treatmentPercentage, value); }
      }
      private double _totalWeight;

      public double TotalWeight
      {
         get { return _totalWeight; }
         set { SetProperty(ref _totalWeight, value); }
      }
      private double _doseMaximum3D;

      public double DoseMaximum3D
      {
         get { return _doseMaximum3D; }
         set { SetProperty(ref _doseMaximum3D, value); }
      }
      private double _repeatFactor;

      public double RepeatFactor
      {
         get { return _repeatFactor; }
         set { SetProperty(ref _repeatFactor, value); }
      }
      private double _weightAtDoseMaximum;

      public double WeightAtDoseMaximum
      {
         get { return _weightAtDoseMaximum; }
         set { SetProperty(ref _weightAtDoseMaximum, value); }
      }
      private double _doseIsocenter;

      public double DoseIsocenter
      {
         get { return _doseIsocenter; }
         set { SetProperty(ref _doseIsocenter, value); }
      }
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
