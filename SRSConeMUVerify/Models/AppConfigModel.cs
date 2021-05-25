using Prism.Mvvm;
using SRSConeMUVerify.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace SRSConeMUVerify.Models
{
   public class AppConfigModel : BindableBase
   {
      
      private bool _isConfigured;

      public bool IsConfigured
      {
         get { return _isConfigured; }
         set { SetProperty(ref _isConfigured , value); }
      }

      private bool _isServerData;
      public bool IsServerData
      {
         get { return _isServerData; }
         set { SetProperty(ref _isServerData, value); }
      }
      //public bool IsCSVData { get; set; }
      private bool _isCSVData;

      public bool IsCSVData
      {
         get { return _isCSVData; }
         set { SetProperty(ref _isCSVData , value); }
      }
      private string _serverBeamDataDirectory;

      public string ServerBeamDataDirectory
      {
         get { return _serverBeamDataDirectory; }
         set { SetProperty(ref _serverBeamDataDirectory, value); }
      }
      
      private string _server;

      public string Server
      {
         get { return _server; }
         set {SetProperty(ref  _server,value); }
      }

      
      private IEnumerable<XElement> _calcModels;

      public IEnumerable<XElement> CalcModels
      {
         get { return _calcModels; }
         set { SetProperty(ref _calcModels, value); }
      }

      
      private ObservableCollection<CalcModelModel> _calcModelModels;

      public ObservableCollection<CalcModelModel> CalcModelModels
      {
         get { return _calcModelModels; }
         set { SetProperty(ref _calcModelModels, value); }
      }

      private CalcModelModel _selectedCalcModel;

      public CalcModelModel SelectedCalcModel
      {
         get { return _selectedCalcModel; }
         set { SetProperty(ref _selectedCalcModel, value); }
      }

      private MapfileModel _mapfileModel;

      public MapfileModel MapfileModel
      {
         get { return _mapfileModel; }
         set { SetProperty(ref _mapfileModel,  value); }
      }
      public AppConfigModel()
      {
         Server = "undefined";
         IsServerData = false;
         IsCSVData = false;
         CalcModelModels = new ObservableCollection<CalcModelModel>();
      }
      public void CalcModelToTable()
      {
         foreach (var model in CalcModels)
         {
            CalcModelModel calcModel = new CalcModelModel();
            calcModel.Name = (string)model.Attribute("Name");
            calcModel.AlgorithmName = (string)model.Attribute("AlgorithmName");
            calcModel.AlgorithmVersion = (string)model.Attribute("AlgorithmVersion");
            calcModel.BeamDataDirectory = (string)model.Attribute("BeamDataDirectory");
            calcModel.Enabled = (string)model.Attribute("Enabled");
            CalcModelModels.Add(calcModel);
         }
      }
   }
}
