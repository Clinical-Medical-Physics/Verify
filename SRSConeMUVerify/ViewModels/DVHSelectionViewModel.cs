using DVHPlot.Events;
using DVHPlot.Models;
using Prism.Events;
using Prism.Mvvm;
using SRSConeMUVerify.Events;
using SRSConeMUVerify.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.API;

namespace DVHPlot.ViewModels
{
   public class DVHSelectionViewModel : BindableBase
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
         set { SetProperty(ref _course,value); }
      }

      private IEventAggregator _eventAggregator;
      public ObservableCollection<StructureSelectionModel> SelectionStructures { get; private set; }
      public DVHSelectionViewModel(PlanSetup plan,
          IEventAggregator eventAggregator, Patient patient)
      {
         _patient = patient;
         Plan = plan;
         _eventAggregator = eventAggregator;
         SelectionStructures = new ObservableCollection<StructureSelectionModel>();
         SetInitialStructures();
         _eventAggregator.GetEvent<PlanSelectedEvent>().Subscribe(OnPlanChanged);
      }

      private void OnPlanChanged(PlanModel obj)
      {
         if(obj != null)
         {
            Course = _patient.Courses.Where(x => x.Id == obj.CourseId).FirstOrDefault();
            Plan = Course.PlanSetups.Where(x => x.Id == obj.PlanId).FirstOrDefault();
            SelectionStructures.Clear();
            SetInitialStructures();
         }
         else
         {
            SelectionStructures.Clear();
         }
      }

      private void SetInitialStructures()
      {
         foreach (Structure s in Plan.StructureSet.Structures.Where(x => !x.IsEmpty && x.DicomType != "MARKER" && x.DicomType != "SUPPORT"))
         {
            SelectionStructures.Add(new StructureSelectionModel(_eventAggregator)
            {
               Id = s.Id,
               //bIsChecked = _plan.StructuresSelectedForDvh.Contains(s)
            });
         }
      }
   }
}
