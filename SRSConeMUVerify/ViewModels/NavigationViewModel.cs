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
         //var doc = new PortableDocument();
         //doc.Title = "Verify Plan Report";

         //doc.AddPage(PageSize.Letter);
         //doc.SetFont("Franklin Gothic", 12);
         //doc.DrawText(25, 767, "Verify SRS Cone Plan MUV");
         //doc.MoveTo(25, 667);
         //MemoryStream stream = new MemoryStream();
         //MemoryStream stream1 = new MemoryStream();
         //using (stream)
         //{
         //var pdfExporter = new PdfExporter { Width = 1024, Height = 768 };
         //pdfExporter.Export(DVHViewModel.DVHPlotModel, stream);
         //var pngExporter = new PngExporter { Width = 600, Height = 400 };
         //pngExporter.Export(DVHViewModel.DVHPlotModel, stream);

         //stream1 = new MemoryStream(stream.ToArray());
         //var pdi = PortableDocumentImageUtilities.Convert(new OxyImage(stream.GetBuffer()), true);

         //doc.DrawImage(image: new PortableDocumentImage(pdi.Width, pdi.Height, pdi.BitsPerComponent, pdi.Bits));

         //}
         //stream1.Position = 0;
         //var pdi = PortableDocumentImageUtilities.Convert(new OxyImage(stream1), true);
         //doc.DrawImage(image: new PortableDocumentImage(pdi.Width, pdi.Height, pdi.BitsPerComponent, pdi.Bits));


         //var fd = new SaveFileDialog();
         //fd.Filter = "PDF|*.pdf";
         //fd.Title = "Save MUV to PDF";
         //fd.ShowDialog();
         //if (fd.FileName != "")
         //{
         //   FileStream fs = (FileStream)fd.OpenFile();
         //   doc.Save(fs);
         //}
         double res = 120;
         double dHeight = 11 / (1.0 / res);
         int page_height = (int)(dHeight);
         double dWidth = 8.5 / (1.0 / res);
         int page_width = (int)(dWidth);
         double ires = 300;
         double iheight = 0.9*5.5 / (1.0 / ires);
         int image_height = (int)iheight-50;
         double iwidth = 0.9 * 8.5 / (1.0 / ires);
         int image_width = (int)iwidth-50;
         FlowDocument fd = new FlowDocument { FontSize = 18, FontFamily = new FontFamily("Franklin Gothic") };
         fd.Blocks.Add(new Paragraph(new Run("Verify Report")));
         fd.Blocks.Add(new BlockUIContainer(new PlanInformationView { DataContext = PlanInformationViewModel }));

         fd.Blocks.Add(new BlockUIContainer(new MUCheckView { DataContext = MUCheckViewModel }));
         SetPlotModelProperties();
         //fd.Blocks.Add(new BlockUIContainer(new DVHView { DataContext = DVHViewModel }));
         var pngExporter = new PngExporter { Background = OxyColors.Transparent, Resolution = ires, Height = image_height, Width = image_width };
         
         BitmapSource bmp = pngExporter.ExportToBitmap(DVHViewModel.DVHPlotModel);
         
         //play with these numbers and try landscape to make the output look better
         fd.Blocks.Add(new BlockUIContainer(new System.Windows.Controls.Image
         {
            Source = bmp,
            //Height = image_height,
            //Width = image_width
         }));
         System.Windows.Controls.PrintDialog printer = new System.Windows.Controls.PrintDialog();
         //printer.PrintTicket.PageOrientation = System.Printing.PageOrientation.Landscape;
         fd.PageHeight = page_height;
         fd.PageWidth = page_width;
         fd.PagePadding = new System.Windows.Thickness(50);
         fd.ColumnGap = 0;
         fd.ColumnWidth = page_width;
         IDocumentPaginatorSource source = fd;
         if (printer.ShowDialog() == true)
         {
            printer.PrintDocument(source.DocumentPaginator, "TreatmentPlanReport");
         }
      }
      private void SetPlotModelProperties()
      {
         //DVHViewModel.DVHPlotModel.InvalidatePlot(true);
         DVHViewModel.DVHPlotModel.PlotAreaBackground = OxyColors.White;
         DVHViewModel.DVHPlotModel.PlotAreaBorderColor = OxyColors.Black;
         DVHViewModel.DVHPlotModel.PlotAreaBorderThickness.Bottom.Equals(30);
         DVHViewModel.DVHPlotModel.PlotAreaBorderThickness.Left.Equals(3);
         DVHViewModel.DVHPlotModel.PlotAreaBorderThickness.Top.Equals(3);
         DVHViewModel.DVHPlotModel.PlotAreaBorderThickness.Right.Equals(3);
         DVHViewModel.DVHPlotModel.Background = OxyColors.Transparent;
         DVHViewModel.DVHPlotModel.TextColor = OxyColors.Black;
         DVHViewModel.DVHPlotModel.LegendPlacement = LegendPlacement.Inside;
         DVHViewModel.DVHPlotModel.LegendPosition = LegendPosition.TopLeft;
         foreach(OxyPlot.Axes.LinearAxis axis in DVHViewModel.DVHPlotModel.Axes)
         {
            axis.FontSize = 14;
            axis.TitleFontSize = 14;
            axis.TitleFontWeight = OxyPlot.FontWeights.Bold;
            axis.AxislineThickness = 2;
            axis.TicklineColor = OxyColors.Black;
            axis.TickStyle = TickStyle.Inside;
            axis.MinorGridlineThickness = 2;
            axis.MajorGridlineThickness = 2;
            axis.AxislineColor = OxyColors.Black;
         }
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
