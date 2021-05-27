using SRSConeMUVerify.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using VMS.TPS.Common.Model.API;

namespace SRSConeMUVerify.Utilities
{
   public class Calculations
   {
      public class InterpretedValue
      {
         public double value;
         public bool isValidInput;

         public InterpretedValue(double value = 0, bool isValidInput = false)
         {
            this.value = value;
            this.isValidInput = isValidInput;
         }

      }
      /// <summary>
      /// Returns the linear interpolation between the given points
      /// Tries to ensure valid arguments but might fail in some cases
      /// </summary>
      /// <param name="x1">
      /// The lower value of x, usually from a table
      /// </param>
      /// <param name="f1">
      /// The function value at x1, usually from a table
      /// </param>
      /// <param name="x2">
      /// The upper value of x, usually from a table
      /// </param>
      /// <param name="f2">
      /// the function value at x2, usually from a table
      /// </param>
      /// <param name="x">
      /// The value of x where you want to know f
      /// </param>
      /// <returns></returns>
      public static InterpretedValue LinearInterpolation(double x1, double f1, double x2, double f2, double x)
      {
         InterpretedValue interpretedValue = new InterpretedValue(0,true);

         if ((x1 == x2) || (x < x1 && x < x2) || (x > x1 && x > x2))
         {
            interpretedValue.isValidInput = false;
            return interpretedValue;
         }
         interpretedValue.value = ((x2 - x) / (x2 - x1)) * f1 + ((x - x1) / (x2 - x1)) * f2;
         return interpretedValue;
      }
      public static CheckedBeamModel CalculateCheckBeam(CheckedBeamModel checkedBeam, 
         ObservableCollection<MachineModel> machineModels,
         PlanPrescriptionModel planPrescriptionModel)
      {
         //
       
         
         MachineModel _machineModel = machineModels.Where(x => x.Name == checkedBeam.Machine && x.Energy == checkedBeam.Energy).First();
         
         TMRModel tmrModel = _machineModel.TMRModels.Where(x => x.ConeSize == checkedBeam.ConeSize).FirstOrDefault();
         
         TMRDataPoint tmrDataPoint1 = tmrModel.DataPoints
            .Where(x => x.Depth <= checkedBeam.AverageDepth).Last();
         TMRDataPoint tmrDataPoint2 = tmrModel.DataPoints
            .Where(x => x.Depth >= checkedBeam.AverageDepth).First();
         MessageBox.Show($"{tmrDataPoint1.Depth} {checkedBeam.AverageDepth} {tmrDataPoint2.Depth}");
         InterpretedValue tmrValue = LinearInterpolation(tmrDataPoint1.Depth, tmrDataPoint1.TMRValue, tmrDataPoint2.Depth,
            tmrDataPoint2.TMRValue, checkedBeam.AverageDepth);
         MessageBox.Show($"{tmrDataPoint1.TMRValue} {tmrValue.value} {tmrDataPoint2.TMRValue}");
         checkedBeam.TMRValue = tmrValue.value;
         checkedBeam.OutputFactor = tmrModel.OutputFactor;
         

         return new CheckedBeamModel();
      }
   }
}
