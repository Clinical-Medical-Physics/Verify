using SRSConeMUVerify.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SRSConeMUVerify.Utilities
{
   public class CsvConnector : IDataConnection
   {
      // TODO - Wire up
      /// <summary>
      /// saves a new TMR model based on a csv input
      /// </summary>
      /// <param name="model">The TMR information</param>
      /// <returns>The TMR information</returns>
      public TMRModel CreateTMR(TMRModel model)
      {
         model.Id = 1;
         return model;
      }
   }
}
