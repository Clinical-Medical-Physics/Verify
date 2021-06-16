using DVHPlot.ViewModels;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Wpf;
using Prism.Mvvm;
using SRSConeMUVerify.Models;
using SRSConeMUVerify.Views;
using System.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using VMS.TPS.Common.Model.API;
using Prism.Commands;

namespace SRSConeMUVerify.ViewModels
{
   public class PrintPreviewViewModel : BindableBase
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
         set { SetProperty(ref _printPreviewModel, value); }
      }
      private PlanInformationViewModel _planInfoViewModel;

      public PlanInformationViewModel PlanInformationViewModel
      {
         get { return _planInfoViewModel; }
         set { SetProperty(ref _planInfoViewModel, value); }
      }
      private MUCheckViewModel _muCheckViewModel;

      public MUCheckViewModel MUCheckViewModel
      {
         get { return _muCheckViewModel; }
         set { SetProperty(ref _muCheckViewModel, value); }
      }
      private DVHViewModel _dvhViewModel;

      public DVHViewModel DVHViewModel
      {
         get { return _dvhViewModel; }
         set { SetProperty(ref _dvhViewModel, value); }
      }

      public DelegateCommand SavePDF { get; set; }

      public PrintPreviewViewModel(PrintPreviewModel printPreviewModel, Patient patient, PlanInformationViewModel planInformationViewModel,
         MUCheckViewModel muCheckViewModel, DVHViewModel dvhViewModel)
      {
         PrintPreviewModel = printPreviewModel;
         PatientName = patient.Name;
         PlanInformationViewModel = planInformationViewModel;
         MUCheckViewModel = muCheckViewModel;
         DVHViewModel = dvhViewModel;
         SavePDF = new DelegateCommand(OnSavePDF);
      }

      private void OnSavePDF()
      {
         System.Windows.Controls.PrintDialog printer = new System.Windows.Controls.PrintDialog();
         ////printer.PrintTicket.PageOrientation = System.Printing.PageOrientation.Landscape;
         string reportName = $"{ PlanInformationViewModel.PatientName}-MUVerification";
         IDocumentPaginatorSource source = PrintPreviewModel.printViewFD;
         if (printer.ShowDialog() == true)
         {
            printer.PrintDocument(source.DocumentPaginator, reportName);
         }
      }

      public void getFlowDocument()
      {
         double res = 120;
         double dHeight = 11 / (1.0 / res);
         int page_height = (int)(dHeight);
         double dWidth = 8.5 / (1.0 / res);
         int page_width = (int)(dWidth);
         double ires = 300;
         double iheight = 0.9 * 5.5 / (1.0 / ires);
         int image_height = (int)iheight - 50;
         double iwidth = 0.9 * 8.5 / (1.0 / ires);
         int image_width = (int)iwidth - 50;
         PlotModel plotModel = DVHViewModel.DVHPlotModel;
         FlowDocument fd = new FlowDocument { FontSize = 12, FontFamily = new System.Windows.Media.FontFamily("Franklin Gothic") };
         fd.Background = System.Windows.Media.Brushes.White;
         fd.Foreground = System.Windows.Media.Brushes.Black;
         fd.ColumnWidth = page_width*0.95;
         fd.IsColumnWidthFlexible = true;
         fd.MinPageWidth = 816;
         fd.MinPageHeight = 1056;
         fd.PageHeight = page_height;
         fd.PageWidth = page_width;
         fd.PagePadding = new System.Windows.Thickness(50);
         fd.ColumnGap = 0;
         fd.ColumnWidth = page_width;
         Run titleRun = new Run("Verify SRS Cone MU Verification Report");
         titleRun.FontWeight = System.Windows.FontWeights.Bold;
         titleRun.FontSize = 18;
         Paragraph titleParagraph = new Paragraph(titleRun);
         titleParagraph.TextAlignment = System.Windows.TextAlignment.Center;
         fd.Blocks.Add(titleParagraph);
         Run nameRun = new Run($"Name: {PlanInformationViewModel.PatientName}");
         nameRun.FontWeight = System.Windows.FontWeights.Bold;
         nameRun.FontSize = 16;
         Paragraph nameParagraph = new Paragraph(nameRun);
         nameParagraph.TextAlignment = System.Windows.TextAlignment.Left;
         fd.Blocks.Add(nameParagraph);

         fd.Blocks.Add(GetHorizontalLine());
         fd.Blocks.Add(AddHeader("Prescription"));
         // Table to hold prescription information
         fd.Blocks.Add(AddPlanInformationTable());
         fd.Blocks.Add(GetHorizontalLine());
         fd.Blocks.Add(AddHeader("MU Verification"));
         fd.Blocks.Add(AddCheckedBeamsTable());
         //fd.Blocks.Add(new BlockUIContainer(new MUCheckView { DataContext = MUCheckViewModel }));
         SetPlotModelProperties(plotModel);
         //fd.Blocks.Add(new BlockUIContainer(new DVHView { DataContext = DVHViewModel }));
         var pngExporter = new PngExporter { Background = OxyColors.Transparent, Resolution = ires, Height = image_height, Width = image_width };

         BitmapSource bmp = pngExporter.ExportToBitmap(plotModel);

         //play with these numbers and try landscape to make the output look better
         fd.Blocks.Add(new BlockUIContainer(new System.Windows.Controls.Image
         {
            Source = bmp,
            //Height = image_height,
            //Width = image_width
         }));
         PrintPreviewModel.printViewFD = fd;
      }
      private Paragraph AddHeader(string header)
      {
         // Prescription
         Run headerRun = new Run(header);
         headerRun.FontWeight = System.Windows.FontWeights.Bold;
         headerRun.FontSize = 16;
         Paragraph headerParagraph = new Paragraph(headerRun);
         headerParagraph.TextAlignment = System.Windows.TextAlignment.Left;
         return headerParagraph;
      }
      private Table AddCheckedBeamsTable()
      {
         string[] rowNames = { "Id", "Energy", "ConeSize","AverageDepth","WeightFactor","OutputFactor","TMRValue",
         "RefDose","CalcDose","TPSMU","CalcMU","PercentDiffMU"};
         string[] units = { "", "(MV)", "(mm)", "(mm)","","","","(cGy)","(cGy)","","","" };
         Table t = new Table();
         int numberOfColumns = MUCheckViewModel.CheckedBeams.Count;
         for (int i = 0; i == numberOfColumns; i++)
         {
            t.Columns.Add(new TableColumn());
         }
         //
         t.RowGroups.Add(new TableRowGroup());
         for (int i=0;i< rowNames.Length; i++)
         {
            t.RowGroups[0].Rows.Add(new TableRow());
            TableRow currentRow = t.RowGroups[0].Rows[i];
            AddCheckedBeamRow(rowNames[i], units[i], currentRow);
         }
         return t;
      }
      private void AddCheckedBeamRow(string rowName,string unit, TableRow currentRow)
      {
         currentRow.Cells.Add(AddTableCell($"{ rowName} {unit}"));
         foreach (var checkedBeam in MUCheckViewModel.CheckedBeams)
         {
            
            //object value = typeof(CheckedBeamModel).GetProperty(rowName).GetValue(checkedBeam);
            currentRow.Cells.Add(AddTableCell($"{checkedBeam?.GetType()?.GetProperty(rowName)?.GetValue(checkedBeam):N3}"));
            
         }
      }
      private Table AddPlanInformationTable()
      {
         //fd.Blocks.Add(new BlockUIContainer(new PlanInformationView { DataContext = PlanInformationViewModel }));
         PlanPrescriptionModel planPrescriptionModel = PlanInformationViewModel.PlanPrescriptionModel;
         Table t = new Table();
         int numberOfcolumns = 8;
         for (int x = 0; x < numberOfcolumns; x++)
         {
            t.Columns.Add(new TableColumn());
         }
         //first row
         t.RowGroups.Add(new TableRowGroup());
         t.RowGroups[0].Rows.Add(new TableRow());
         TableRow currentRow = t.RowGroups[0].Rows[0];
         currentRow.Cells.Add(AddTableCell("ID of Plan:"));
         currentRow.Cells.Add(AddTableCell($"{PlanInformationViewModel.SelectedPlanId}"));
         currentRow.Cells.Add(AddTableCell("Isodose Line:"));
         currentRow.Cells.Add(AddTableCell($"{PlanInformationViewModel.TreatmentPercentage} %"));
         currentRow.Cells.Add(AddTableCell("Total Dose:"));
         currentRow.Cells.Add(AddTableCell($"{planPrescriptionModel.TotalDose:N2} {PlanInformationViewModel.DoseUnit}"));
         t.RowGroups[0].Rows.Add(new TableRow());
         currentRow = t.RowGroups[0].Rows[1];
         currentRow.Cells.Add(AddTableCell("Total Weight:"));
         currentRow.Cells.Add(AddTableCell($"{planPrescriptionModel.TotalWeight:N3}"));
         currentRow.Cells.Add(AddTableCell("Weight at DMax:"));
         currentRow.Cells.Add(AddTableCell($"{planPrescriptionModel.WeightAtDoseMaximum:N3}"));
         currentRow.Cells.Add(AddTableCell("Repeat Factor:"));
         currentRow.Cells.Add(AddTableCell($"{planPrescriptionModel.RepeatFactor:N3}"));
         return t;

      }
      private TableCell AddTableCell(string contents)
      {
         return new TableCell(new Paragraph(new Run(contents)));
      }
      private static Table GetHorizontalLine()
      {
         //table to act as a  horizontal line
         Table t = new Table();
         t.Background = System.Windows.Media.Brushes.Black;
         t.Columns.Add(new TableColumn());
         t.RowGroups.Add(new TableRowGroup());
         t.RowGroups[0].Rows.Add(new TableRow());
         t.RowGroups[0].Rows[0].Cells.Add(new TableCell(new Paragraph(new Run(String.Empty))));
         t.RowGroups[0].Rows[0].Cells[0].LineHeight = 1;
         t.RowGroups[0].Rows[0].Cells[0].FontSize = 1;
         return t;
      }
      private static void SetPlotModelProperties(PlotModel plotModel)
      {
         //plotModel.InvalidatePlot(true);
         plotModel.PlotAreaBackground = OxyColors.White;
         plotModel.PlotAreaBorderColor = OxyColors.Black;
         plotModel.PlotAreaBorderThickness.Bottom.Equals(30);
         plotModel.PlotAreaBorderThickness.Left.Equals(3);
         plotModel.PlotAreaBorderThickness.Top.Equals(3);
         plotModel.PlotAreaBorderThickness.Right.Equals(3);
         plotModel.Background = OxyColors.Transparent;
         plotModel.TextColor = OxyColors.Black;
         plotModel.LegendPlacement = LegendPlacement.Inside;
         plotModel.LegendPosition = LegendPosition.TopLeft;
         foreach (OxyPlot.Axes.LinearAxis axis in plotModel.Axes)
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
   }
}
