using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SRSConeMUVerify.Models
{
   public class MachineModel : BindableBase
   {
      private string _id;

      public string Id
      {
         get { return _id; }
         set { SetProperty(ref _id, value); }
      }
      private string _name;

      public string Name
      {
         get { return _name; }
         set { SetProperty(ref _name, value); }
      }
      private string _energy;

      public string Energy
      {
         get { return _energy; }
         set { SetProperty(ref _energy, value); }
      }
      private double _absoluteDoseCalibration;
            
      public double AbsoluteDoseCalibration
      {
         get { return _absoluteDoseCalibration; }
         set {SetProperty(ref _absoluteDoseCalibration,  value); }
      }
      private double _outputFactor;

      public double OutputFactor
      {
         get { return _outputFactor; }
         set { SetProperty (ref _outputFactor, value); }
      }
      private ObservableCollection<TmrXmlModel> _tmrXmlModels;

      public ObservableCollection<TmrXmlModel> TmrXmlModels
      {
         get { return _tmrXmlModels; }
         set {SetProperty(ref _tmrXmlModels, value); }
      }

      private ObservableCollection<OutputFacXmlModel> _outPutFactorModel;

      public ObservableCollection<OutputFacXmlModel> OutputFactorModel
      {
         get { return _outPutFactorModel; }
         set { SetProperty(ref _outPutFactorModel, value); }
      }

      private ObservableCollection<TMRModel> _tMRModels;

      public ObservableCollection<TMRModel> TMRModels
      {
         get { return _tMRModels; }
         set { SetProperty(ref _tMRModels, value); }
      }
      
      public MachineModel()
      {
         TMRModels = new ObservableCollection<TMRModel>();
         OutputFactorModel = new ObservableCollection<OutputFacXmlModel>();
         TmrXmlModels = new ObservableCollection<TmrXmlModel>();
      }
      public void ConnectOutputFactorsTMRs()
      {
         foreach(TMRModel tmr in TMRModels)
         {
            double coneValue = Convert.ToDouble(tmr.ConeSize.Split('m').First());

            foreach(OutputFacXmlModel.OutPutFactor ofac in OutputFactorModel.First().OutPutFactors)
            {
               if(coneValue == ofac.ConeSize)
               {
                  OutputFactor = ofac.OutputFactorValue;
               }
            }
            tmr.OutputFactor = OutputFactor;
         }
      }
      public void ConnectTmrCurveTMR()
      {
         try
         {
            foreach (TMRModel tmr in TMRModels)
            {
               //MessageBox.Show($"1");
               double coneValue = Convert.ToDouble(tmr.ConeSize.Split('m').First());
               //MessageBox.Show($"2");
               foreach (TmrXmlModel tmrXmlModel in TmrXmlModels)
               {
                  //MessageBox.Show($"3");
                  List<TmrXmlModel.TmrCurve> tmrCurves = tmrXmlModel.TmrCurves;
                  List<TmrXmlModel.TmrCurve> tmrCalcCurves = tmrXmlModel.TmrCalcCurves;
                  tmrCurvesToPoints(coneValue, tmrCurves, "processed", tmr);
                  tmrCurvesToPoints(coneValue, tmrCalcCurves, "calculated", tmr);
               }
            }
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
      }
      private void tmrCurvesToPoints(double coneValue,List<TmrXmlModel.TmrCurve> tmrCurves,string procOrCalc, TMRModel tmr)
      {
         foreach (TmrXmlModel.TmrCurve tmrcurve in tmrCurves)
         {
            //MessageBox.Show($"4");
            double coneValueXml = Convert.ToDouble(tmrcurve.FieldSize);
            if (coneValue == coneValueXml)
            {
               //MessageBox.Show($"5");
               string[] values = tmrcurve.Values.Split(';');
               foreach (var value in values)
               {
                  //MessageBox.Show($"6 {value}");
                  TMRDataPoint tMRDataPoint = new TMRDataPoint();
                  try
                  {
                     string[] depthTMR = value.Split(',');
                     tMRDataPoint.Depth = Convert.ToDouble(depthTMR[0]);
                     tMRDataPoint.TMRValue = Convert.ToDouble(depthTMR[1]);
                     if (procOrCalc == "calculated")
                     {
                        tmr.DataCalcPoints.Add(tMRDataPoint);
                     }
                     else
                     {
                        tmr.DataPoints.Add(tMRDataPoint);
                     }
                     
                  }
                  catch
                  {

                  }
               }
            }

         }
      }
      // TODO add conversion of output factor to dmax 
   }
}
