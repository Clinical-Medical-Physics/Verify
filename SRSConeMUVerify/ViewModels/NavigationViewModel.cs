using Prism.Commands;
using Prism.Events;
using SRSConeMUVerify.Events;
using SRSConeMUVerify.Models;
using SRSConeMUVerify.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MahApps.Metro.Controls.Dialogs;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using OxyPlot.Wpf;
using DVHPlot.ViewModels;
using OxyPlot;
using OxyPlot.Axes;
//using OxyPlot.Pdf;
using DVHPlot.Views;
using System.IO;
using Microsoft.Win32;
using System.Windows.Media;
using Prism.Mvvm;

namespace SRSConeMUVerify.ViewModels
{
   public class NavigationViewModel : BindableBase
   {
      public ConfigurationViewModel ConfigurationViewModel { get; set; }
      public PlanInformationViewModel PlanInformationViewModel { get; }
      public MUCheckViewModel MUCheckViewModel { get; }
      public DVHViewModel DVHViewModel { get; }
      public DelegateCommand LaunchConfigurationCommand { get; private set; }
      public DelegateCommand PrintView { get; private set; }
      public IEventAggregator _eventAggregator;
      public ConfigurationView configurationView { get; set; }
      public PrintPreviewView printPreviewView { get; set; }
      public PrintPreviewModel PrintPreviewModel { get; set; }
      public MessageView mv { get; set; }
      public MessageViewModel mvm { get; set; }
      public PrintPreviewViewModel PrintPreviewViewModel { get; set; }
      private FlowDocument _printPreviewFD;

      public FlowDocument PrintPreviewFD
      {
         get { return _printPreviewFD; }
         set
         {
            SetProperty(ref _printPreviewFD, value);
            
         }
      }

      public NavigationViewModel(ConfigurationViewModel configurationViewModel, IEventAggregator eventAggregator,
         PlanInformationViewModel planInformationViewModel, MUCheckViewModel muCheckViewModel, DVHViewModel dVHViewModel,
         PrintPreviewViewModel printPreviewViewModel)
      {
         ConfigurationViewModel = configurationViewModel;
         PlanInformationViewModel = planInformationViewModel;
         MUCheckViewModel = muCheckViewModel;
         DVHViewModel = dVHViewModel;
         PrintPreviewViewModel = printPreviewViewModel;
         PrintPreviewFD = new FlowDocument();
         //PrintPreviewModel = new PrintPreviewModel(PlanInformationViewModel, MUCheckViewModel, DVHViewModel);
         LaunchConfigurationCommand = new DelegateCommand(OnLaunchConfiguration);
         PrintView = new DelegateCommand(OnPrintView);
         _eventAggregator = eventAggregator;
         mvm = new MessageViewModel(_eventAggregator);
         _eventAggregator.GetEvent<ConfigViewCloseEvent>().Subscribe(Closer);
         _eventAggregator.GetEvent<MessageViewCloseEvent>().Subscribe(MessageCloser);
         if (ConfigurationViewModel.IsConfigured == false)
         {
            ShowMessage("First Run...Opening Configuration", "Continue", "Cancel");
            if (mvm.OnRequestOkay == "Continue")
            {
               OnLaunchConfiguration();
            }
            else
            {
               App.Current.Shutdown();
            }

         }
      }

      private void OnPrintView()
      {
         //PrintPreviewModel = new PrintPreviewModel(PlanInformationViewModel, MUCheckViewModel, DVHViewModel);
         PrintPreviewViewModel.getFlowDocument();
         printPreviewView = new PrintPreviewView();
         printPreviewView.DataContext = PrintPreviewViewModel;
         printPreviewView.ShowDialog();
         //System.Windows.Controls.PrintDialog printer = new System.Windows.Controls.PrintDialog();
         ////printer.PrintTicket.PageOrientation = System.Printing.PageOrientation.Landscape;
         //fd.PageHeight = page_height;
         //fd.PageWidth = page_width;
         //fd.PagePadding = new System.Windows.Thickness(50);
         //fd.ColumnGap = 0;
         //fd.ColumnWidth = page_width;
         //IDocumentPaginatorSource source = fd;
         //if (printer.ShowDialog() == true)
         //{
         //   printer.PrintDocument(source.DocumentPaginator, "TreatmentPlanReport");
         //}
      }

      private void MessageCloser(bool obj)
      {
         mv.Close();
         mv = null;
      }

      private void ShowMessage(string message, string btn1, string btn2)
      {
         mv = new MessageView();
         mvm = new MessageViewModel(_eventAggregator);
         mvm.Message = message;
         mvm.MessageButton1 = btn1;
         mvm.MessageButton2 = btn2;
         mv.DataContext = mvm;
         mv.ShowDialog();
      }
      private void OnLaunchConfiguration()
      {
         bool continueAnyway = false;

         //MessageBoxResult userAnswer = new MessageBoxResult();
         if (ConfigurationViewModel.IsConfigured)
         {
            //userAnswer = MessageBox.Show("SRS Cone MU Verify is already configured! Do you want to Continue?",
            //   "WARNING Already Configured",
            //   MessageBoxButton.YesNo,
            //   MessageBoxImage.Warning);
            // use custom message window
            ShowMessage("SRS Cone MU Verify is already configured!\nDo you want to Continue?", "Continue", "Cancel");
            if (mvm.OnRequestOkay == "Continue")
            {
               continueAnyway = true;
            }
         }
         else
         {
            continueAnyway = true;
         }
         if (continueAnyway)
         {
            configurationView = new ConfigurationView();
            configurationView.DataContext = ConfigurationViewModel;
            configurationView.ShowDialog();
         }
      }
      public void Closer(bool close)
      {
         configurationView.Close();
         configurationView = null;
      }
   }
}
