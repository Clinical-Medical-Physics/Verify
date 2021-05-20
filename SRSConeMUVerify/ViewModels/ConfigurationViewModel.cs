using Prism.Commands;
using Prism.Mvvm;
using SRSConeMUVerify.Models;
using SRSConeMUVerify.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SRSConeMUVerify.ViewModels
{
   public class ConfigurationViewModel //: BindableBase
   {
      public DelegateCommand ImportXmlFromServer { get; private set; }
      public DelegateCommand EclipseDataChecked { get; private set; }
      public DelegateCommand CSVDataChecked { get; private set; }
      private DatabaseXmlReader databaseXmlReader { get; set; }
      public string AppDirectory { get; set; }

      //private AppConfigModel _appConfigModel;
      //public AppConfigModel AppConfigModel
      //{
      //   get { return _appConfigModel; }
      //   set { SetProperty(ref _appConfigModel, value);}
      //}

      public AppConfigModel AppConfigModel { get; set; }
      public ConfigurationViewModel(AppConfigModel appConfigModel)
      {
         AppConfigModel = appConfigModel;
         //AppConfigModel = new AppConfigModel();
         //hard coded since running in binary
         // TODO -- make this dynamic using environment variable passed in
         AppDirectory = Environment.CurrentDirectory;
         //AppDirectory = @"W:\NZ\Physicists Share\EclipseScripts\ConfigurationView\Plugins";
         EclipseDataChecked = new DelegateCommand(OnEclipseDataChecked);
         CSVDataChecked = new DelegateCommand(OnCSVDataChecked);
         ImportXmlFromServer = new DelegateCommand(OnImportXmlFromServer);
      }

      private void OnEclipseDataChecked()
      {
         AppConfigModel.IsCSVData = false;
         AppConfigModel.IsServerData = true;
      }

      private void OnCSVDataChecked()
      {
         AppConfigModel.IsServerData = false;
         AppConfigModel.IsCSVData = true;
      }
      private void OnImportXmlFromServer()
      {
         //AppConfigModel.Server is bound to the textbox
         databaseXmlReader = new DatabaseXmlReader(AppConfigModel.Server, AppDirectory);
         if (databaseXmlReader.CopyCalcModel())
         {
            //read the xml file
            AppConfigModel.CalcModels = databaseXmlReader.ReadCopiedXmlFile();

            AppConfigModel.CalcModelsTable = AppConfigModel.CalcModelToTable();
            //MessageBox.Show(AppConfigModel.CalcModels.First().ToString());

         }


      }

   }
}
