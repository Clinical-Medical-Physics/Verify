using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

      private ObservableCollection<TMRModel> _tMRModels;

      public ObservableCollection<TMRModel> TMRModels
      {
         get { return _tMRModels; }
         set { SetProperty(ref _tMRModels, value); }
      }
      
      public MachineModel()
      {
         TMRModels = new ObservableCollection<TMRModel>();
      }
   }
}
