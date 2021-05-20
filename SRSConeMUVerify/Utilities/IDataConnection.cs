using SRSConeMUVerify.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SRSConeMUVerify.Utilities
{
   public interface IDataConnection
   {
      TMRModel CreateTMR(TMRModel model);
   }
}
