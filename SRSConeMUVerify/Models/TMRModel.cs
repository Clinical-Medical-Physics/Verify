using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRSConeMUVerify.Models
{

   public class TMRModel : BindableBase
   {
      // TODO add a connection to the sourcedetector distance fomr tmrxmlmodel
      private string _coneSize;

      public string ConeSize
      {
         get { return _coneSize; }
         set { SetProperty(ref _coneSize , value); }
      }
      private double _outputFactor;

      public double OutputFactor
      {
         get { return _outputFactor; }
         set { SetProperty(ref _outputFactor , value); }
      }
      private ObservableCollection<TMRDataPoint> _dataPoints;

      public ObservableCollection<TMRDataPoint> DataPoints
      {
         get { return _dataPoints; }
         set { SetProperty(ref _dataPoints, value); }
      }
      private ObservableCollection<TMRDataPoint> _dataCalcPoints;

      public ObservableCollection<TMRDataPoint> DataCalcPoints
      {
         get { return _dataCalcPoints; }
         set { SetProperty(ref _dataCalcPoints, value); }
      }

      public TMRModel()
      {
         DataPoints = new ObservableCollection<TMRDataPoint>();
         DataCalcPoints = new ObservableCollection<TMRDataPoint>();
      }
   }
   public class TMRDataPoint
   {
      public double Depth { get; set; }
      public double TMRValue { get; set; }
   }
}
