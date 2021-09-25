using Prism.Events;
using Prism.Mvvm;
using SRSConeMUVerify.Events;
using SRSConeMUVerify.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VMS.TPS.Common.Model.API;

namespace SRSConeMUVerify.ViewModels
{
   public class PlanInformationViewModel : BindableBase
   {
      private string _patientName;

      public string PatientName
      {
         get { return _patientName; }
         set { SetProperty(ref _patientName, value); }
      }
      private string _selectedPlanId;

      public string SelectedPlanId
      {
         get { return _selectedPlanId; }
         set { SetProperty(ref _selectedPlanId, value); }
      }
      private double _treatmentPercentage;
      private Patient _patient;
      private IEventAggregator _eventAggregator;

      public double TreatmentPercentage
      {
         get { return _treatmentPercentage; }
         set { SetProperty(ref _treatmentPercentage, value); }
      }
      private PlanPrescriptionModel _planPrescriptionModel;

      public PlanPrescriptionModel PlanPrescriptionModel
      {
         get { return _planPrescriptionModel; }
         set { SetProperty(ref _planPrescriptionModel, value); }
      }
      private string _doseUnit;

      public string DoseUnit
      {
         get { return _doseUnit; }
         set { SetProperty(ref _doseUnit, value); }
      }

      public PlanInformationViewModel(Patient patient, IEventAggregator eventAggregator, PlanPrescriptionModel planPrescriptionModel)
      {
         PlanPrescriptionModel = planPrescriptionModel;
         _patient = patient;
         PatientName = patient.Name;
         _eventAggregator = eventAggregator;
         DoseUnit = String.Empty;
         _eventAggregator.GetEvent<PlanSelectedEvent>().Subscribe(OnPlanSelected);

      }

      private void OnPlanSelected(PlanModel obj)
      {
         if (obj != null)
         {
            var plan = _patient.Courses.FirstOrDefault(x => x.Id == obj.CourseId).PlanSetups.FirstOrDefault(x => x.Id == obj.PlanId);
            //MessageBox.Show("In OnPlanSelected PlanInfoViewModel");
            List<Beam> beams = plan.Beams.Where(x => x.IsSetupField == false).ToList();
            double totalWeight = beams.Sum(x => x.WeightFactor);
            PlanPrescriptionModel.getPlanPrescriptionModel(plan.DosePerFraction.Dose
               , Convert.ToDouble(plan.NumberOfFractions), plan.TotalDose.Dose,
               plan.TreatmentPercentage, totalWeight, plan.Dose.DoseMax3D.Dose,
               plan.Dose.GetDoseToPoint(beams.First().IsocenterPosition).Dose);

            DoseUnit = plan.TotalDose.UnitAsString;

            SelectedPlanId = plan.Id;
            TreatmentPercentage = plan.TreatmentPercentage * 100.0;
         }
         else
         {
            PlanPrescriptionModel.resetPlanPrescriptionModel();
            SelectedPlanId = String.Empty;
            TreatmentPercentage = 0;
            DoseUnit = String.Empty;
         }
      }
   }
}
