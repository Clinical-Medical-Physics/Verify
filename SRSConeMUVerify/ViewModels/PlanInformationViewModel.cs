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

namespace SRSConeMUVerify.ViewModels
{
    public class PlanInformationViewModel:BindableBase
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
			set { SetProperty(ref _treatmentPercentage,value); }
		}

		public PlanInformationViewModel(Patient patient, IEventAggregator eventAggregator)
		{
			_patient = patient;
			PatientName = patient.Name;
			_eventAggregator = eventAggregator;
			_eventAggregator.GetEvent<PlanSelectedEvent>().Subscribe(OnPlanSelected);
		}

		private void OnPlanSelected(PlanModel obj)
		{
			if (obj != null)
			{
				var plan = _patient.Courses.FirstOrDefault(x => x.Id == obj.CourseId).PlanSetups.FirstOrDefault(x => x.Id == obj.PlanId);
				SelectedPlanId = plan.Id;
				TreatmentPercentage = plan.PrescribedPercentage * 100.0;
			}
		}
	}
}
