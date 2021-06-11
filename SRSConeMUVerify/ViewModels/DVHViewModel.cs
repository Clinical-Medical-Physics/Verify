using DVHPlot.Events;
using DVHPlot.Models;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using Prism.Events;
using Prism.Mvvm;
using SRSConeMUVerify.Events;
using SRSConeMUVerify.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;

namespace DVHPlot.ViewModels
{
   public class DVHViewModel : BindableBase
   {
      private PlanSetup _plan;

      public PlanSetup Plan
      {
         get { return _plan; }
         set { SetProperty(ref _plan, value); }
      }
      private Patient _patient;
      private Course _course;

      public Course Course
      {
         get { return _course; }
         set { SetProperty(ref _course, value); }
      }
      private IEventAggregator _eventAggregator;
      private PlotModel _dvhPlotModel;

      public PlotModel DVHPlotModel
      {
         get { return _dvhPlotModel; }
         set { SetProperty(ref _dvhPlotModel, value); }
      }


      public DVHViewModel(PlanSetup plan,
          IEventAggregator eventAggregator, Patient patient)
      {
         _plan = plan;
         _patient = patient;
         _eventAggregator = eventAggregator;
         DVHPlotModel = new PlotModel();
         SetPlotModelProperties(Plan.Id.ToString(), Plan.TotalPrescribedDose.UnitAsString);
         //GetDefaultDVH();
         _eventAggregator.GetEvent<StructureSelectionEvent>().Subscribe(OnStructureSelectionChanged);
         _eventAggregator.GetEvent<PlanSelectedEvent>().Subscribe(OnPlanChanged);
      }

      private void OnPlanChanged(PlanModel obj)
      {
         
         if (obj != null)
         {
            Course = _patient.Courses.Where(x => x.Id == obj.CourseId).FirstOrDefault();
            Plan = Course.PlanSetups.Where(x => x.Id == obj.PlanId).FirstOrDefault();
            DVHPlotModel = new PlotModel();
            SetPlotModelProperties(Plan.Id.ToString(), Plan.TotalPrescribedDose.UnitAsString);
         }
         else
         {
            Plan = null;
            DVHPlotModel = new PlotModel();
            SetPlotModelProperties("No plan selected", "Gy");
         }
      }

      private void OnStructureSelectionChanged(StructureSelectionModel selStructure)
      {
         if (selStructure.bIsChecked)
         {
            //add to the plot model
            Structure s = _plan.StructureSet.Structures.FirstOrDefault(x => x.Id == selStructure.Id);
            DVHData dvh = _plan.GetDVHCumulativeData(s,
                DoseValuePresentation.Absolute,
                VolumePresentation.Relative,
                1);
            if (dvh != null)
            {
               GeneratePlotSeries(s, dvh);
            }
         }
         else
         {
            //remove from the plot model
            if (DVHPlotModel.Series.FirstOrDefault(x => x.Title == selStructure.Id) != null)
            {
               DVHPlotModel.Series.Remove(DVHPlotModel.Series.FirstOrDefault(x => x.Title == selStructure.Id));
            }
         }
         DVHPlotModel.InvalidatePlot(true);
      }

      private void SetPlotModelProperties(string plotTitle, string doseUnits)
      {
         DVHPlotModel.Title = $"DVH for {plotTitle}";
         DVHPlotModel.PlotAreaBackground = OxyColor.FromRgb(37, 37, 37);
         DVHPlotModel.PlotAreaBorderColor = OxyColors.White;
         DVHPlotModel.Background = OxyColor.FromRgb(37,37,37);
         DVHPlotModel.TextColor = OxyColors.White;
         DVHPlotModel.LegendPlacement = LegendPlacement.Outside;
         DVHPlotModel.LegendPosition = LegendPosition.RightTop;
         DVHPlotModel.Axes.Add(new LinearAxis
         {
            Title = $"Dose [{doseUnits}]",
            Position = AxisPosition.Bottom,
            FontSize = 12,
            AxislineThickness = 3,
            TicklineColor = OxyColors.White,
            TickStyle = TickStyle.Inside,
            MinorGridlineThickness = 3,
            MajorGridlineThickness = 3,
            
            AxislineColor = OxyColors.White
         }); ;
         DVHPlotModel.Axes.Add(new LinearAxis
         {
            Title = $"Volume [%]",
            Position = AxisPosition.Left,
            FontSize =12,
            AxislineThickness = 3,
            TicklineColor = OxyColors.White,
            TickStyle = TickStyle.Inside,
            MinorGridlineThickness = 3,
            MajorGridlineThickness = 3,
            
            AxislineColor = OxyColors.White
         });
      }
      //dvh generation is controlled only by events from the StructureSelectionModel.
      //private void GetDefaultDVH()
      //{
      //    foreach(var structure in _plan.StructuresSelectedForDvh)
      //    {
      //        var dvh = _plan.GetDVHCumulativeData(structure,
      //            DoseValuePresentation.Absolute,
      //            VolumePresentation.Relative,
      //            1);
      //        if (dvh != null)
      //        {
      //            GeneratePlotSeries(structure, dvh);
      //        }
      //    }
      //}
      private void GeneratePlotSeries(Structure structure, DVHData dvh)
      {
         var dvhSeries = new LineSeries
         {
            Title = structure.Id,
            Color = OxyColor.FromArgb(structure.Color.A, structure.Color.R, structure.Color.G, structure.Color.B)
         };
         foreach (var dvhPoint in dvh.CurveData)
         {
            dvhSeries.Points.Add(new DataPoint(dvhPoint.DoseValue.Dose, dvhPoint.Volume));
         }
         DVHPlotModel.Series.Add(dvhSeries);
      }
   }
}
