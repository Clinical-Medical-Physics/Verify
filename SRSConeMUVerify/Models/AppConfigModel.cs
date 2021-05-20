using Prism.Mvvm;
using SRSConeMUVerify.Utilities;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace SRSConeMUVerify.Models
{
   public class AppConfigModel : BindableBase
   {
      //private PatientShiftModel _xShift;

      //public PatientShiftModel XShift
      //{
      //   get { return _xShift; }
      //   set { SetProperty(ref _xShift, value); }
      //}
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

      //public string Server { get; set; }
      private string _server;

      public string Server
      {
         get { return _server; }
         set {SetProperty(ref  _server,value); }
      }

      //public IEnumerable<XElement> CalcModels { get; set; }
      private IEnumerable<XElement> _calcModels;

      public IEnumerable<XElement> CalcModels
      {
         get { return _calcModels; }
         set { SetProperty(ref _calcModels, value); }
      }

      //public DataTable CalcModelsTable { get; set; }
      private DataTable _calcModelsTable;

      public DataTable CalcModelsTable
      {
         get { return _calcModelsTable; }
         set { SetProperty(ref _calcModelsTable, value); }
      }

      public AppConfigModel()
      {
         Server = "undefined";
         IsServerData = false;
         IsCSVData = false;
      }
      public DataTable CalcModelToTable()
      {
         DataTable calcModelTable = new DataTable();
         DataColumn name = new DataColumn("Name", typeof(string));
         DataColumn algoName = new DataColumn("Algorithm Name");
         DataColumn algoVer = new DataColumn("Algorithm Version");
         DataColumn beamDatDir = new DataColumn("Beam Data Directory");
         DataColumn enabled = new DataColumn("Enabled");

         calcModelTable.Columns.Add(name);
         calcModelTable.Columns.Add(algoName);
         calcModelTable.Columns.Add(algoVer);
         calcModelTable.Columns.Add(beamDatDir);
         calcModelTable.Columns.Add(enabled);

         foreach (var model in CalcModels)
         {
            DataRow dataRow = calcModelTable.NewRow();
            dataRow[0] = (string)model.Attribute("Name");
            dataRow[1] = (string)model.Attribute("AlgorithmName");
            dataRow[2] = (string)model.Attribute("AlgorithmVersion");
            dataRow[3] = (string)model.Attribute("BeamDataDirectory");
            dataRow[4] = (string)model.Attribute("Enabled");
            calcModelTable.Rows.Add(dataRow);
         }
         return calcModelTable;
      }
   }
}
