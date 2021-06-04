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
using DVHPlot.Views;

namespace SRSConeMUVerify.ViewModels
{
   public class NavigationViewModel
   {
      public ConfigurationViewModel ConfigurationViewModel { get; set; }
      public PlanInformationViewModel PlanInformationViewModel { get; }
      public MUCheckViewModel MUCheckViewModel { get; }
      public DVHViewModel DVHViewModel { get; }
      public DelegateCommand LaunchConfigurationCommand { get; private set; }
      public DelegateCommand PrintView { get; private set; }
      public IEventAggregator _eventAggregator;
      public ConfigurationView configurationView { get; set; }
      public MessageView mv { get; set; }
      public MessageViewModel mvm { get; set; }
      public NavigationViewModel(ConfigurationViewModel configurationViewModel, IEventAggregator eventAggregator, 
         PlanInformationViewModel planInformationViewModel, MUCheckViewModel muCheckViewModel, DVHViewModel dVHViewModel)
      {
         ConfigurationViewModel = configurationViewModel;
         PlanInformationViewModel = planInformationViewModel;
         MUCheckViewModel = muCheckViewModel;
         DVHViewModel = dVHViewModel;
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
         FlowDocument fd = new FlowDocument { FontSize = 12, FontFamily = new System.Windows.Media.FontFamily("Franklin Gothic") };
         fd.Blocks.Add(new Paragraph(new Run("Verify Report")));
         fd.Blocks.Add(new BlockUIContainer(new PlanInformationView { DataContext = PlanInformationViewModel }));

         fd.Blocks.Add(new BlockUIContainer(new MUCheckView { DataContext = MUCheckViewModel }));
         SetPlotModelProperties();
         //fd.Blocks.Add(new BlockUIContainer(new DVHView { DataContext = DVHViewModel }));
         var pngExporter = new PngExporter { Width = 600, Height = 400, Background = OxyColors.Transparent };
         BitmapSource bmp = pngExporter.ExportToBitmap(DVHViewModel.DVHPlotModel);
         //play with these numbers and try landscape to make the output look better
         fd.Blocks.Add(new BlockUIContainer(new System.Windows.Controls.Image
         {
            Source = bmp,
            Height = 768,
            Width = 1024
         }));
         System.Windows.Controls.PrintDialog printer = new System.Windows.Controls.PrintDialog();
         printer.PrintTicket.PageOrientation = System.Printing.PageOrientation.Landscape;
         fd.PageHeight = 816;
         fd.PageWidth = 1056;
         fd.PagePadding = new System.Windows.Thickness(50);
         fd.ColumnGap = 0;
         fd.ColumnWidth = 816;
         IDocumentPaginatorSource source = fd;
         if (printer.ShowDialog() == true)
         {
            printer.PrintDocument(source.DocumentPaginator, "TreatmentPlanReport");
         }
      }
      private void SetPlotModelProperties()
      {
         DVHViewModel.DVHPlotModel.InvalidatePlot(true);
         DVHViewModel.DVHPlotModel.Title = $"DVH for {PlanInformationViewModel.SelectedPlanId}";
         DVHViewModel.DVHPlotModel.PlotAreaBackground = OxyColors.Transparent;
         DVHViewModel.DVHPlotModel.PlotAreaBorderColor = OxyColors.Black;
         DVHViewModel.DVHPlotModel.Background = OxyColors.Transparent;
         DVHViewModel.DVHPlotModel.TextColor = OxyColors.Black;
         DVHViewModel.DVHPlotModel.LegendPlacement = LegendPlacement.Outside;
         DVHViewModel.DVHPlotModel.LegendPosition = LegendPosition.RightTop;
         DVHViewModel.DVHPlotModel.Axes.Add(new OxyPlot.Axes.LinearAxis
         {
            Title = $"Dose [{PlanInformationViewModel.DoseUnit}]",
            Position = AxisPosition.Bottom,
            FontSize = 12,
            AxislineThickness = 3,
            TicklineColor = OxyColors.Black,
            TickStyle = TickStyle.Inside,
            MinorGridlineThickness = 3,
            MajorGridlineThickness = 3,

            AxislineColor = OxyColors.Black
         }); ;
         DVHViewModel.DVHPlotModel.Axes.Add(new OxyPlot.Axes.LinearAxis
         {
            Title = $"Volume [%]",
            Position = AxisPosition.Left,
            FontSize = 12,
            AxislineThickness = 3,
            TicklineColor = OxyColors.Black,
            TickStyle = TickStyle.Inside,
            MinorGridlineThickness = 3,
            MajorGridlineThickness = 3,

            AxislineColor = OxyColors.Black
         });
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
