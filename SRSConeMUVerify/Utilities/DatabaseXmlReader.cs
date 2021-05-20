using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Xml;
using System.Xml.Linq;

namespace SRSConeMUVerify.Utilities
{
   public class DatabaseXmlReader
   {
      public string XmlFile { get; private set; }
      public string ServerName { get; set; }
      public string AppDirectory { get; set; }
      public string ResourceDirectory { get; private set; }
      public string ResourceXmlFile { get; private set; }
      public DatabaseXmlReader(string serverName, string appDirectory)
      {
         // TODO - get the server from the user on initial setup
         // get the CalculationModels.xml file from the DCF folder
         ServerName = @"\\"+$"{serverName}";
         XmlFile = ServerName + @"\dcf$\client\CalculationModels.xml";
         AppDirectory = appDirectory;
         // copy it to the resources folder ?
         ResourceDirectory = Path.Combine(AppDirectory, "Resources");
         ResourceXmlFile = Path.Combine(ResourceDirectory, "SRSCalculationModels.xml");

      }

      public bool CopyCalcModel()
      {
         try
         {
            //move calculation model xml file to local so we dont destroy server copy
            File.Copy(XmlFile, ResourceXmlFile,true);
            return true;
         }
         catch
         {
            MessageBox.Show("Unable to copy Calculation File to Resource Folder");
         }
         return false;
      }
      public IEnumerable<XElement> ReadCopiedXmlFile()
      {
         //file exists otherwise this ins't called 
         // find all the CDC algorithms
         var calcModelsData = XElement.Load(ResourceXmlFile);
         Regex regEx = new Regex("CDC");
         var calcModels = calcModelsData.Descendants("CalculationModel")
            .Where(x => regEx.IsMatch((string)x.Attribute("Name")));
         return calcModels;
      }
   }
}
