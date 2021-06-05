using Prism.Mvvm;
using SRSConeMUVerify.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.API;

namespace SRSConeMUVerify.ViewModels
{
   public class PrintPreviewViewModel:BindableBase
   {
      private string _patientName;

      public string PatientName
      {
         get { return _patientName; }
         set { SetProperty(ref _patientName, value); }
      }

      private PrintPreviewModel _printPreviewModel;

      public PrintPreviewModel PrintPreviewModel
      {
         get { return _printPreviewModel; }
         set { SetProperty(ref _printPreviewModel , value); }
      }
      public PrintPreviewViewModel(PrintPreviewModel printPreviewModel,Patient patient)
      {
         PrintPreviewModel = printPreviewModel;
         PatientName = patient.Name;
      }

   }
}
