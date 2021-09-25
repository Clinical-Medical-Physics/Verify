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
         set { SetProperty(ref _planPrescriptionModel, value); }
      }
      private ObservableCollection<MachineModel> _machineModels;

      public ObservableCollection<MachineModel> MachineModels
      {
         get { return _machineModels; }
         set { SetProperty(ref _machineModels, value); }
      }

      private ObservableCollection<CheckedBeamModel> _checkedBeams;

      public ObservableCollection<CheckedBeamModel> CheckedBeams
      {
         get { return _checkedBeams; }
         set { SetProperty(ref _checkedBeams, value); }
      }

      public MUCheckViewModel(ConfigurationViewModel configurationViewModel, Patient patient,
         PlanInformationViewModel planInformationViewModel, PlanNavigationViewModel planNavigationViewModel,
         IEventAggregator eventAggregator, PlanPrescriptionModel planPrescriptionModel)
      {
         ConfigurationViewModel = configurationViewModel;
         PlanInformationViewModel = planInformationViewModel;
         MachineModels = ConfigurationViewModel.AppConfigModel.MachineModels;
         PlanPrescriptionModel = planPrescriptionModel;
         _eventAggregator = eventAggregator;
         _eventAggregator.GetEvent<PlanSelectedEvent>().Subscribe(OnPlanSelected);
         _patient = patient;
         _checkedBeams = new ObservableCollection<CheckedBeamModel>();

         setCheckedBeams(planNavigationViewModel.SelectedPlan);

      }

      public void setCheckedBeams(PlanModel obj)
      {
         //MessageBox.Show("In OnPlanSeleced");
         CheckedBeams.Clear();

         if (obj != null && ConfigurationViewModel.IsConfigured)
         {
            // is this a cone plan
            MachineModels = ConfigurationViewModel.AppConfigModel.MachineModels;
            _plan = _patient.Courses.FirstOrDefault(x => x.Id == obj.CourseId).PlanSetups.FirstOrDefault(x => x.Id == obj.PlanId);
            List<Beam> beams = _plan.Beams.Where(x => x.IsSetupField == false).ToList();

            double totalWeight = beams.Sum(x => x.WeightFactor);
            _planPrescriptionModel.getPlanPrescriptionModel(_plan.DosePerFraction.Dose
               , Convert.ToDouble(_plan.NumberOfFractions), _plan.TotalDose.Dose,
               _plan.TreatmentPercentage, totalWeight, _plan.Dose.DoseMax3D.Dose,
               _plan.Dose.GetDoseToPoint(beams.First().IsocenterPosition).Dose);
            foreach (Beam beam in _plan.Beams.Where(x => x.IsSetupField == false))
            {
               CheckedBeamModel checkedBeam = new CheckedBeamModel();
               checkedBeam.Id = beam.Id.ToString();
               checkedBeam.Machine = beam.TreatmentUnit.Id;
               checkedBeam.Energy = beam.EnergyModeDisplayName.ToString();

               checkedBeam.ConeSize = beam.Applicator?.Id.ToString() ?? "no cone";

               checkedBeam.AverageDepth = (1000 - beam.AverageSSD);
               checkedBeam.WeightFactor = beam.WeightFactor;

               checkedBeam.TPSMU = beam.Meterset.Value;

               _checkedBeams.Add(checkedBeam);
               Calculations.CalculateCheckBeam(checkedBeam, MachineModels, _planPrescriptionModel);
            }

         }
         else
         {
            //MessageBox.Show("Object was null in setCheckedBeams");
            _plan = null;
            _planPrescriptionModel.resetPlanPrescriptionModel();
         }
      }

      private void OnPlanSelected(PlanModel obj)
      {

         setCheckedBeams(obj);

      }


      public ConfigurationViewModel ConfigurationViewModel { get; }
      public PlanInformationViewModel PlanInformationViewModel { get; }
   }
}
