using SRSConeMUVerify.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SRSConeMUVerify.Utilities
{
   public class DataBaseConnector : IDataConnection
   {
      // TODO - make the connector work
      /// <summary>
      /// return TMR Model based on database files
      /// </summary>
      /// <param name="model">The TMR information</param>
      /// <returns>The TMR information, model id is not unique!</returns>
      public TMRModel CreateTMR(TMRModel model)
      {
         model.ConeSize = "CC";
         return model;
      }
   }
}
