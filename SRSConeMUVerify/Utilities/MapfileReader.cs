using SRSConeMUVerify.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRSConeMUVerify.Utilities
{
   public class MapfileReader
   {
      // This is the resource Mapfile Name not the beamdata folder mapfile name
      public string MapfileName { get; set; }
      public MapfileReader(string mapfilename)
      {
         MapfileName = mapfilename;

      }

      public MapfileModel ReadMapFile()
      {
         MapfileModel mapfileModel = new MapfileModel();
         // open the file and read all lines
         string[] lines = File.ReadAllLines(MapfileName);
         bool continueToRead = true;
         int i = 0;


         while (continueToRead)
         {
            var line = lines[i];
            //Console.WriteLine(line);
            switch (line)
            {
               case @"$StartOfCodeSet":
                  CodeSet codeSet = new CodeSet();
                  i = GetCodeSetFromLines(lines, i, codeSet);
                  mapfileModel.CodeSets.Add(codeSet);
                  break;
               case @"$StartOfDataSet":
                  DataSet dataSet = new DataSet();
                  i = GetDataSetFromLines(lines, i, dataSet);
                  mapfileModel.DataSets.Add(dataSet);
                  break;
               default:
                  i++;
                  break;

            }
            if (i >= lines.Length) continueToRead = false;

         }

         return mapfileModel;
      }
      private int GetCodeSetFromLines(string[] lines, int startIndex, CodeSet codeSet)
      {
         // read until $EndOfcodeSet found
         int endIndex = startIndex;
         for (int i = startIndex; i < startIndex + 6; i++)
         {
            if (lines[i] == @"$EndOfCodeSet")
            {
               endIndex = i;
               break;
            }
         }
         char[] splitter = { ':' };
         for (int i = startIndex + 1; i < endIndex; i++)
         {
            string[] line = lines[i].Split(splitter,2);
            switch (line[0].Trim())
            {
               case "Machine Code":
                  codeSet.MachineCode = line[1];
                  break;
               case "Treatment Machine":
                  codeSet.TreatmentMachine = line[1];
                  break;
               case "AddOn":
                  codeSet.AddOn = line[1];
                  break;
            }
         }
         return endIndex + 1;
      }
      private int GetDataSetFromLines(string[] lines, int startIndex, DataSet dataSet)
      {
         // read until $EndOfDataSet found
         int endIndex = startIndex;
         for (int i = startIndex; i < startIndex + 20; i++)
         {
            if (lines[i] == @"$EndOfDataSet")
            {
               endIndex = i;
               break;
            }
         }
         char[] splitter = { ':' };
         for (int i = startIndex + 1; i < endIndex; i++)
         {
            string[] line = lines[i].Split(splitter,2);
            switch (line[0].Trim())
            {
               case "Machine Code":
                  dataSet.MachineCode = line[1];
                  break;
               case "General Parameters":
                  dataSet.GeneralParameters = line[1];
                  break;
               case "Model Parameters":
                  dataSet.ModelParameters = line[1];
                  break;
               case "AddOn":
                  dataSet.AddOn = line[1];
                  break;
               case "FastPlanImportInfo":
                  dataSet.FastPlanImportInfo = line[1];
                  break;
               case "AbsoluteDoseCalibration":
                  dataSet.AbsoluteDoseCalibration = line[1];
                  break;
               case "OutputFactorTable":
                  dataSet.OutputFactorTable = line[1];
                  break;
               case "TMR":
                  dataSet.TMR = line[1];
                  break;
               case "OPP":
                  dataSet.OPP = line[1];
                  break;
               case "TMR_processed":
                  dataSet.TMR_processed = line[1];
                  break;
               case "OPP_processed":
                  dataSet.OPP_processed = line[1];
                  break;
               case "TMR_calculated":
                  dataSet.TMR_calculated = line[1];
                  break;
               case "OPP_calculated":
                  dataSet.OPP_calculated = line[1];
                  break;
               case "ALL_histogram":
                  dataSet.ALL_histogram = line[1];
                  break;
               case "TMR_error":
                  dataSet.TMR_error = line[1];
                  break;
               case "OPP_error":
                  dataSet.OPP_error = line[1];
                  break;

            }

         }
         return endIndex + 1;

      }

   }
}
