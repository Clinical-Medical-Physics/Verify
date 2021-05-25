using Microsoft.Win32;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using SRSConeMUVerify.Models;
using SRSConeMUVerify.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SRSConeMUVerify.ViewModels
{
   public class ConfigurationViewModel : BindableBase
   {
      public DelegateCommand ImportXmlFromServer { get; private set; }
      public DelegateCommand EclipseDataChecked { get; private set; }
      public DelegateCommand CSVDataChecked { get; private set; }
      public DelegateCommand ImportCalcModelData { get; private set; }
      public DelegateCommand SaveAppConfigJson { get; private set; }
      private DatabaseXmlReader databaseXmlReader { get; set; }
      private TmrXmlReader tmrXmlReader { get; set; }
      public string AppDirectory { get; set; }
      public bool IsConfigured { get; set; }
      public ObservableCollection<ConfiguredModel> ConfiguredModel { get; set; }


      public AppConfigModel AppConfigModel { get; set; }
      public ConfigurationViewModel(AppConfigModel appConfigModel)
      {
         AppConfigModel = appConfigModel;
         AppDirectory = Environment.CurrentDirectory;
         EclipseDataChecked = new DelegateCommand(OnEclipseDataChecked);
         CSVDataChecked = new DelegateCommand(OnCSVDataChecked);
         ImportXmlFromServer = new DelegateCommand(OnImportXmlFromServer);
         ImportCalcModelData = new DelegateCommand(OnImportCalcModelData);
         SaveAppConfigJson = new DelegateCommand(OnSaveAppConfigJson);
         ConfiguredModel = new ObservableCollection<ConfiguredModel>();
         IsConfigured = GetConfigurationInfo();
      }

      private bool GetConfigurationInfo()
      {

         string resourceFolder = Path.Combine(AppDirectory, "Resources");
         string appConfigFile = Path.Combine(resourceFolder, "AppConfig.json");
         

         try
         {
            ConfiguredModel configuredModel = new ConfiguredModel();
            using (StreamReader file = File.OpenText(appConfigFile))
            {
               JsonSerializer serializer = new JsonSerializer();
               configuredModel = (ConfiguredModel)serializer.Deserialize(file, typeof(ConfiguredModel));

            }
            bool configured = configuredModel?.IsConfigured ?? false;
            if (configured)
            {


               // TODO make this an init method
               AppConfigModel.Server = configuredModel.Server;
               DatabaseXmlReader databaseXmlReader = new DatabaseXmlReader(AppConfigModel.Server, AppDirectory);
               AppConfigModel.IsServerData = configuredModel.IsServerData;
               AppConfigModel.IsCSVData = configuredModel.IsCSVData;
               AppConfigModel.IsConfigured = configuredModel.IsConfigured;
               AppConfigModel.CalcModels = databaseXmlReader.ReadCopiedXmlFile();
               AppConfigModel.CalcModelToTable();
               string beamDataDirectory = Path.Combine(AppDirectory, "Resources");
               TmrXmlReader tmrXmlReader = new TmrXmlReader(beamDataDirectory, AppDirectory);
               AppConfigModel.MapfileModel = tmrXmlReader.ReadCopiedMapFile();
               ConfiguredModel.Add(configuredModel);
               return true;
            }
            else
            {
               configuredModel = new ConfiguredModel();
               ConfiguredModel.Add(configuredModel);
               return false;
            }
         }
         catch
         {
            MessageBox.Show("Error reading AppConfig.json");
            return false;
         }

      }

      private void OnImportCalcModelData()
      {

         if (AppConfigModel.SelectedCalcModel is null)
         {
            MessageBox.Show("Please select the Calculation Model you would like to import.");
         }
         else
         {
            AppConfigModel.ServerBeamDataDirectory = AppConfigModel.SelectedCalcModel.BeamDataDirectory;
            tmrXmlReader = new TmrXmlReader(AppConfigModel.ServerBeamDataDirectory, AppDirectory);
            if (tmrXmlReader.CopyMapFile())
            {

               AppConfigModel.MapfileModel = tmrXmlReader.ReadCopiedMapFile();
               //copy the files on the server to the local CalculationModels folder based on the mapfilemodel
               CopyCalcModelsFromServer();
               UpdateAppConfigJson();
            }
            else
            {
               MessageBox.Show("Could not copy Mapfile to local directory.");
               // TODO error handling if file is not found
            }
         }
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
            // Generate the observable collection?
            AppConfigModel.CalcModelToTable();

         }
         else
         {
            //didn't work to automatically import so ask to navigate manually
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "All Files (*.*)|*.*";
            if (file.ShowDialog() == true)
            {
               databaseXmlReader.XmlFile = file.FileName;
               if (databaseXmlReader.CopyCalcModel())
               {
                  //read the xml file
                  AppConfigModel.CalcModels = databaseXmlReader.ReadCopiedXmlFile();
                  // Generate the observable collection?
                  AppConfigModel.CalcModelToTable();
                  //MessageBox.Show(AppConfigModel.CalcModels.First().ToString());
               }

            }
         }


      }
      public void CopyCalcModelsFromServer()
      {
         string CalculationModelsDirectory = Path.Combine(AppDirectory, "CalculationModels");
         string calcModelDirectoryName = AppConfigModel.SelectedCalcModel.Name;
         DirectoryInfo directoryInfo = Directory.CreateDirectory(Path.Combine(CalculationModelsDirectory, calcModelDirectoryName));

         string beamDataDirectoryName = AppConfigModel.SelectedCalcModel.BeamDataDirectory;
         foreach (var dataset in AppConfigModel.MapfileModel.DataSets)
         {
            //only need absolute dose file, output factors, and tmr data
            string machineCodeName = GetStringBetweenTag(dataset.MachineCode, '<', '>');
            string machineBeamDataDirectory = Path.Combine(beamDataDirectoryName, machineCodeName);
            DirectoryInfo machineDirectoryInfo = Directory.CreateDirectory(Path.Combine(directoryInfo.FullName, machineCodeName));
            string absDoseCalFile = GetStringBetweenTag(dataset.AbsoluteDoseCalibration, '<', '>');
            File.Copy(Path.Combine(machineBeamDataDirectory, absDoseCalFile), Path.Combine(machineDirectoryInfo.FullName, absDoseCalFile), true);
            string outputFactorFile = GetStringBetweenTag(dataset.OutputFactorTable, '<', '>');
            File.Copy(Path.Combine(machineBeamDataDirectory, outputFactorFile), Path.Combine(machineDirectoryInfo.FullName, outputFactorFile), true);
            string tmrFile = GetStringBetweenTag(dataset.TMR, '<', '>');
            File.Copy(Path.Combine(machineBeamDataDirectory, tmrFile), Path.Combine(machineDirectoryInfo.FullName, tmrFile), true);
            //MessageBox.Show(outputFactorFile);
         }

      }
      public string GetStringBetweenTag(string input, char tagBegin, char tagEnd)
      {

         int startIndex = input.IndexOf(tagBegin);
         int endIndex = input.IndexOf(tagEnd);
         return input.Substring(startIndex + 1, endIndex - startIndex - 1);
      }
      private void UpdateAppConfigJson()
      {
         //TODO actually update the file here
         ConfiguredModel configuredModel = new ConfiguredModel();
         configuredModel.Server = AppConfigModel.Server;
         configuredModel.IsCSVData = AppConfigModel.IsCSVData;
         configuredModel.IsServerData = AppConfigModel.IsServerData;
         configuredModel.defaultCalcModel = AppConfigModel.SelectedCalcModel.Name;
         string CalculationModelsFolder = Path.Combine(AppDirectory, "CalculationModels");
         configuredModel.defaultCalcModelFolder = Path.Combine(CalculationModelsFolder, configuredModel.defaultCalcModel);
         IsConfigured = true;
         configuredModel.IsConfigured = IsConfigured;
         try
         {
            ConfiguredModel.Remove(ConfiguredModel.First());
         }
         catch
         {

         }
         ConfiguredModel.Add(configuredModel);

      }
      private void OnSaveAppConfigJson()
      {
         string resourceFolder = Path.Combine(AppDirectory, "Resources");
         string appConfigFile = Path.Combine(resourceFolder, "AppConfig.json");
         if (IsConfigured)
         {
            // TODO add error handling
            //File.WriteAllText(appConfigFile, JsonConvert.SerializeObject(ConfiguredModel));
            using (StreamWriter file = File.CreateText(appConfigFile))
            {
               JsonSerializer serializer = new JsonSerializer();
               serializer.Serialize(file, ConfiguredModel.FirstOrDefault());
            }
         }
         
      }
   }
}
