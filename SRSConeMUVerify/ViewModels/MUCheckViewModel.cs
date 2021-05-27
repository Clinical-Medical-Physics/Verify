using Prism.Events;
using Prism.Mvvm;
using SRSConeMUVerify.Events;
using SRSConeMUVerify.Models;
using SRSConeMUVerify.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VMS.TPS.Common.Model.API;

namespace SRSConeMUVerify.ViewModels
{
   public class MUCheckViewModel : BindableBase
   {
      private Patient _patient;
      private IEventAggregator _eventAggregator;


      private PlanSetup _plan;

      public PlanSetup Plan
      {
         get { return _plan; }
         set { SetProperty(ref _plan, value); }
      }
      private PlanPrescriptionModel _planPrescriptionModel;

      public PlanPrescriptionModel PlanPrescriptionModel
      {
         get { return _planPrescriptionModel; }
         set { SetProperty(ref _planPrescriptionModel,value); }
      }

      private ObservableCollection<CheckedBeamModel> _checkedBeams;

      public ObservableCollection<CheckedBeamModel> CheckedBeams
      {
         get { return _checkedBeams; }
         set { SetProperty(ref _checkedBeams, value); }
      }

      public MUCheckViewModel(ConfigurationViewModel configurationViewModel, Patient patient,
         PlanInformationViewModel planInformationViewModel, PlanNavigationViewModel planNavigationViewModel,
         IEventAggregator eventAggregator)
      {
         ConfigurationViewModel = configurationViewModel;
         PlanInformationViewModel = planInformationViewModel;
         MachineModels = ConfigurationViewModel.AppConfigModel.MachineModels;
         _eventAggregator = eventAggregator;
         _eventAggregator.GetEvent<PlanSelectedEvent>().Subscribe(OnPlanSelected);
         _patient = patient;
         _checkedBeams = new ObservableCollection<CheckedBeamModel>();
         setCheckedBeams(planNavigationViewModel.SelectedPlan);
      }

      private void setCheckedBeams(PlanModel obj)
      {
         //MessageBox.Show("In OnPlanSeleced");
         _checkedBeams = null;
         _checkedBeams = new ObservableCollection<CheckedBeamModel>();
         List<Beam> beams = _plan.Beams.Where(x => x.IsSetupField == false).ToList();
         //double totalWeight = beams.Sum(x => x.WeightFactor);
         //_planPrescriptionModel = new PlanPrescriptionModel(_plan.UniqueFractionation.PrescribedDosePerFraction.Dose
         //   ,Convert.ToDouble(_plan.UniqueFractionation.NumberOfFractions),_plan.TotalPrescribedDose.Dose,
         //   _plan.PrescribedPercentage,totalWeight,_plan.Dose.DoseMax3D.Dose,
         //   _plan.Dose.GetDoseToPoint(beams.First().IsocenterPosition).Dose);

         if (obj != null)
         {
            _plan = _patient.Courses.FirstOrDefault(x => x.Id == obj.CourseId).PlanSetups.FirstOrDefault(x => x.Id == obj.PlanId);
            foreach (Beam beam in _plan.Beams.Where(x => x.IsSetupField == false))
            {
               CheckedBeamModel checkedBeam = new CheckedBeamModel();
               checkedBeam.Id = beam.Id.ToString();
               checkedBeam.Machine = beam.ExternalBeam.Id;
               checkedBeam.Energy = beam.EnergyModeDisplayName.ToString();
               checkedBeam.ConeSize = beam.Applicator.Id.ToString();
               checkedBeam.AverageDepth = (1000 - beam.AverageSSD);
               checkedBeam.WeightFactor = beam.WeightFactor;

               checkedBeam.TPSMU = beam.Meterset.Value;
               
               _checkedBeams.Add(checkedBeam);
               //Calculations.CalculateCheckBeam(checkedBeam,MachineModels,_planPrescriptionModel);
            }
            
         }
         else
         {
            MessageBox.Show("Object was null in setCheckedBeams");
         }
      }

      private void OnPlanSelected(PlanModel obj)
      {

         setCheckedBeams(obj);

      }

      public ObservableCollection<MachineModel> MachineModels { get; }

      public ConfigurationViewModel ConfigurationViewModel { get; }
      public PlanInformationViewModel PlanInformationViewModel { get; }
   }
}
