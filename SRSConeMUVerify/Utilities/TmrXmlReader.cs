using SRSConeMUVerify.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SRSConeMUVerify.Utilities
{
   public class TmrXmlReader
   {
      public string MapFile { get; set; }
      public string AppDirectory { get; set; }
      public string BeamDataDirectory { get; set; }
      public string ResourceDirectory { get; set; }
      public string ResourceMapFile { get; private set; }
      public TmrXmlReader(string beamDataDirectory, string appDirectory)
      {
         AppDirectory = appDirectory;
         BeamDataDirectory = beamDataDirectory;
         MapFile = Path.Combine(BeamDataDirectory ,"mapfile.txt");
         // copy it to the resources folder ?
         ResourceDirectory = Path.Combine(AppDirectory, "Resources");
         ResourceMapFile = Path.Combine(ResourceDirectory, "mapfile.txt");
      }
      public bool CopyMapFile()
      {
         try
         {
            //move calculation model map file to local so we dont destroy server copy
            File.Copy(MapFile, ResourceMapFile, true);
            return true;
         }
         catch
         {
            MessageBox.Show("Unable to copy Map File to Resource Folder. Find MapFile Manually!");
         }
         return false;
      }
      public MapfileModel ReadCopiedMapFile()
      {
         MapfileReader mapfileReader = new MapfileReader(ResourceMapFile);
         MapfileModel mapfileModel = mapfileReader.ReadMapFile();
         return mapfileModel;
      }

   }
}
