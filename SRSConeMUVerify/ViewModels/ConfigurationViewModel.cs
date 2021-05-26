﻿using Microsoft.Win32;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using SRSConeMUVerify.Events;
using SRSConeMUVerify.Models;
using SRSConeMUVerify.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

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
      private IEventAggregator _eventAggregator { get; set; }
      public bool OnRequestClose { get; set; }
      public AppConfigModel AppConfigModel { get; set; }
      public ConfigurationViewModel(AppConfigModel appConfigModel, IEventAggregator eventAggregator)
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

         _eventAggregator = eventAggregator;
         OnRequestClose = false;
      }
      private void InitFromJson(ConfiguredModel configuredModel)
      {
         // TODO make this an init method
         string resourceFolder = Path.Combine(AppDirectory, "Resources");
         AppConfigModel.Server = configuredModel.Server;
         DatabaseXmlReader databaseXmlReader = new DatabaseXmlReader(AppConfigModel.Server, AppDirectory);
         AppConfigModel.IsServerData = configuredModel.IsServerData;
         AppConfigModel.IsCSVData = configuredModel.IsCSVData;
         AppConfigModel.IsConfigured = configuredModel.IsConfigured;
         AppConfigModel.CalcModels = databaseXmlReader.ReadCopiedXmlFile();
         AppConfigModel.CalcModelToTable();
         TmrXmlReader tmrXmlReader = new TmrXmlReader(resourceFolder, AppDirectory);
         AppConfigModel.MapfileModel = tmrXmlReader.ReadCopiedMapFile();
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
               InitFromJson(configuredModel);
               ConfiguredModel.Add(configuredModel);
               ReadMachineModels();
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
         // TODO make this a reset method
         AppConfigModel.MachineModels = null;
         AppConfigModel.MachineModels = new ObservableCollection<MachineModel>();
         AppConfigModel.SelectedMachineModel = null;
         AppConfigModel.SelectedMachineModel = new MachineModel();
         AppConfigModel.TMRModels = null;
         AppConfigModel.TMRModels = new ObservableCollection<TMRModel>();
         AppConfigModel.SelectedTMRModel = null;
         AppConfigModel.SelectedTMRModel = new TMRModel();
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
               ReadMachineModels();
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
      public void ReadMachineModels()
      {
         if (ConfiguredModel.First().IsConfigured)
         {
            string CalculationModelsDirectory = Path.Combine(AppDirectory, "CalculationModels");
            string calcModelDirectoryName = ConfiguredModel.First().defaultCalcModel;
            string[] directories = Directory.GetDirectories(Path.Combine(CalculationModelsDirectory, calcModelDirectoryName));
            //DirectoryInfo[] directories = directoryInfo.GetDirectories();
            foreach (var directory in directories)
            {
               DirectoryInfo directoryInfo = new DirectoryInfo(directory);
               Console.WriteLine(directoryInfo.Name);
               MachineModel machineModel = new MachineModel();
               machineModel.Id = directoryInfo.Name;
               foreach (var file in Directory.GetFiles(directory))
               {
                  FileInfo fileInfo = new FileInfo(file);
                  Console.WriteLine(fileInfo.Name);
                  if (fileInfo.Name == $"{machineModel.Id}_codeset.txt")
                  {
                     //MessageBox.Show($"{machineModel.Id}_codeset.txt");
                     readCodeSetfile(machineModel, fileInfo);
                  }
                  else if (fileInfo.Name == $"{machineModel.Id}_CN_AbsoluteDoseCalibration.xml")
                  {
                     //MessageBox.Show($"{machineModel.Id}_CN_AbsoluteDoseCalibration.xml");
                     readAbsoluteDoseCalXml(machineModel, fileInfo);
                  }
                  else if (fileInfo.Name == $"{machineModel.Id}_CN_OutputFactorTable.xml")
                  {
                     readOutputFactorTableXml(machineModel, fileInfo);
                  }
                  else if (fileInfo.Name == $"{machineModel.Id}_CN_TMR.xml")
                  {
                     readTmrXml(machineModel, fileInfo);
                  }

               }
               AppConfigModel.MachineModels.Add(machineModel);
            }
         }
      }

      private void readTmrXml(MachineModel machineModel, FileInfo fileInfo)
      {

      }

      private void readOutputFactorTableXml(MachineModel machineModel, FileInfo fileInfo)
      {

      }

      private void readAbsoluteDoseCalXml(MachineModel machineModel, FileInfo fileInfo)
      {
         var absDoseCalData = XElement.Load(fileInfo.FullName);
         var absDoseParameters = absDoseCalData.Descendants("Parameter");
         AbsDoseXmlModel absDoseXmlModel = new AbsDoseXmlModel();
         foreach(XElement element in absDoseParameters)
         {
            string id = element.Attribute("Id").Value;
            string value = element.Attribute("Value").Value;
            //MessageBox.Show($"{id} {value}");
            if (id == "AbsDoseRefFieldSize")
            {
               //MessageBox.Show($"{id} {value}");
               absDoseXmlModel.AbsDoseRefFieldSize = value;
            }
            else if (id == "AbsDoseMeasurementSSD")
            {
               absDoseXmlModel.AbsDoseMeasurementSSD = value;
            }
            else if (id == "AbsDoseMeasurementDepth")
            {
               absDoseXmlModel.AbsDoseMeasurementDepth = value;
            }
            else if (id == "AbsDoseMeasurementGy")
            {
               absDoseXmlModel.AbsDoseMeasurementGy = value;
            }
            else if (id == "AbsDoseMeasurementMU")
            {
               absDoseXmlModel.AbsDoseMeasurementMU = value;
            }
            else if (id == "ImportedFromFastPlan")
            {
               absDoseXmlModel.ImportedFromFastPlan = value;
            }
         }
         //MessageBox.Show($"{absDoseXmlModel.AbsDoseMeasurementMU} {absDoseXmlModel.AbsDoseMeasurementGy}");
         machineModel.AbsoluteDoseCalibration = absDoseXmlModel.AbsoluteDoseCalibration();
         
      }

      private void readCodeSetfile(MachineModel machineModel, FileInfo fileInfo)
      {
         string[] lines = File.ReadAllLines(fileInfo.FullName);
         // should be safe to read them here manually since they were written by the program
         List<string> items = ExtractFromString(lines[1], "<", ">");
         machineModel.Name = items.First();
         machineModel.Energy = items.Last();
         items = ExtractFromString(lines[2], "<", ">");
         foreach (var item in items)
         {
            if (item.Contains("CC"))
            {
               TMRModel tmrModel = new TMRModel();
               tmrModel.ConeSize = item;
               machineModel.TMRModels.Add(tmrModel);
            }
         }
      }
      private static List<string> ExtractFromString(string source, string start, string end)
      {
         var results = new List<string>();

         string pattern = string.Format(
             "{0}({1}){2}",
             Regex.Escape(start),
             ".*?",
              Regex.Escape(end));

         foreach (Match m in Regex.Matches(source, pattern))
         {
            results.Add(m.Groups[1].Value);
         }

         return results;
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

         foreach (var codeset in AppConfigModel.MapfileModel.CodeSets)
         {
            // add codeset information to folder for reading later
            List<string> lines = new List<string>();
            string machineCodeName = GetStringBetweenTag(codeset.MachineCode, '<', '>');
            lines.Add(codeset.MachineCode);
            lines.Add(codeset.TreatmentMachine);
            lines.Add(codeset.AddOn);
            DirectoryInfo machineDirectoryInfo = Directory.CreateDirectory(Path.Combine(directoryInfo.FullName, machineCodeName));
            string codesetName = machineCodeName + "_codeset.txt";
            string pathName = Path.Combine(machineDirectoryInfo.FullName, codesetName);
            File.WriteAllLines(pathName, lines);

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
         OnRequestClose = true;
         _eventAggregator.GetEvent<ConfigViewCloseEvent>().Publish(OnRequestClose);

      }

   }
}
