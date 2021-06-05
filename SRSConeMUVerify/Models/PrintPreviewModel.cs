using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using VMS.TPS.Common.Model.API;

namespace SRSConeMUVerify.Models
{
   public class PrintPreviewModel : BindableBase
   {
      public FlowDocument printViewFD { get; set; }
   }
}
