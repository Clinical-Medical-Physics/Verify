using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRSConeMUVerify.Models
{
   public class ConfiguredModel : BindableBase
   {
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
         set { SetProperty(ref _isCSVData, value); }
      }
      private string _server;

      public string Server
      {
         get { return _server; }
         set { SetProperty(ref _server, value); }
      }
      private string _defCalcModel;
      public string defaultCalcModel
      {
         get { return _defCalcModel; }
         set { SetProperty(ref _defCalcModel, value); }
      }
      private string _defCalcModelFolder;
      public string defaultCalcModelFolder
      {
         get { return _defCalcModelFolder; }
         set { SetProperty(ref _defCalcModelFolder, value); }
      }
      private bool _isConfigured;

      public bool IsConfigured
      {
         get { return _isConfigured; }
         set { SetProperty(ref _isConfigured, value); }
      }

      public ConfiguredModel()
      {
         Server = "";
         IsCSVData = false;
         IsServerData = false;
         defaultCalcModel = "";
         defaultCalcModelFolder = "";
         IsConfigured = false;
      }
   }
}
