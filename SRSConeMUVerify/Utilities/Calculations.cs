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
         //MessageBox.Show($"{tmrModel.DataPoints.Count} {tmrModel.DataCalcPoints.Count}");
         TMRDataPoint tmrDataPoint1 = tmrModel.DataPoints
            .Where(x => x.Depth <= checkedBeam.AverageDepth).Last();
         TMRDataPoint tmrDataPoint2 = tmrModel.DataPoints
            .Where(x => x.Depth >= checkedBeam.AverageDepth).First();
         checkedBeam.TMRValue = GetInterpolatedTMR(tmrDataPoint1,tmrDataPoint2,checkedBeam.AverageDepth).value/100.0;
         TMRDataPoint tmrDataPoint3 = tmrModel.DataCalcPoints.Where(x => x.Depth <= checkedBeam.AverageDepth).Last();
         TMRDataPoint tmrDataPoint4 = tmrModel.DataCalcPoints.Where(x => x.Depth >= checkedBeam.AverageDepth).First();
         //MessageBox.Show($"{tmrDataPoint1.Depth} {tmrDataPoint1.TMRValue}\n{tmrDataPoint3.Depth} {tmrDataPoint3.TMRValue}");
         //MessageBox.Show($"{tmrDataPoint2.Depth} {tmrDataPoint2.TMRValue}\n{tmrDataPoint4.Depth} {tmrDataPoint4.TMRValue}");
         checkedBeam.TMRCalcValue = GetInterpolatedTMR(tmrDataPoint3, tmrDataPoint4, checkedBeam.AverageDepth).value/100.0;
         TMRDataPoint tmrDataPoint5 = tmrModel.DataCalcPoints.Where(x => x.Depth <= 50).Last();
         TMRDataPoint tmrDataPoint6 = tmrModel.DataCalcPoints.Where(x => x.Depth >= 51).First();
         //MessageBox.Show($"{tmrDataPoint1.Depth} {tmrDataPoint1.TMRValue}\n{tmrDataPoint3.Depth} {tmrDataPoint3.TMRValue}");
         //MessageBox.Show($"{tmrDataPoint2.Depth} {tmrDataPoint2.TMRValue}\n{tmrDataPoint4.Depth} {tmrDataPoint4.TMRValue}");
         // TODO make this point to the tmr depth from the cone output parameters
         double tmrCone50 = GetInterpolatedTMR(tmrDataPoint5, tmrDataPoint6, 50).value/100.0;
         //_machineModel.AbsoluteDoseCalibration
         double coneFacDmax = _machineModel.AbsoluteDoseCalibration * tmrModel.OutputFactor / tmrCone50;
         checkedBeam.OutputFactor = coneFacDmax;
         checkedBeam.RefDose = checkedBeam.TPSMU * checkedBeam.OutputFactor;
         checkedBeam.CalcDose = planPrescriptionModel.RepeatFactor * checkedBeam.WeightFactor / checkedBeam.TMRValue;
         checkedBeam.CalcMU = checkedBeam.CalcDose / coneFacDmax;
         double aveMU = (checkedBeam.CalcMU + checkedBeam.TPSMU) / 2.0;
         checkedBeam.PercentDiffMU = 100 * Math.Abs(checkedBeam.CalcMU - checkedBeam.TPSMU) / aveMU;
         return new CheckedBeamModel();
      }
      public static InterpretedValue GetInterpolatedTMR(TMRDataPoint tmrDataPoint1, TMRDataPoint tmrDataPoint2,double depth)
      {
         return LinearInterpolation(tmrDataPoint1.Depth, tmrDataPoint1.TMRValue, tmrDataPoint2.Depth,
            tmrDataPoint2.TMRValue, depth);
         
      }
   }
}
