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

namespace SRSConeMUVerify.ViewModels
{
   public class PlanNavigationViewModel : BindableBase
   {
      private Patient _patient;
      private Course _course;
      private PlanSetup _plan;
      private IEventAggregator _eventAggregator;

      public ObservableCollection<CourseModel> Courses { get; set; }
      public ObservableCollection<PlanModel> Plans { get; set; }
      private CourseModel _selectedCourse;

      public CourseModel SelectedCourse
      {
         get { return _selectedCourse; }
         set
         {
            SetProperty(ref _selectedCourse, value);
            if (SelectedCourse != null)
            {
               GetPlans();
            }
         }
      }


      private PlanModel _selectedPlan;

      public PlanModel SelectedPlan
      {
         get { return _selectedPlan; }
         set
         {
            SetProperty(ref _selectedPlan, value);

            _eventAggregator.GetEvent<PlanSelectedEvent>().Publish(SelectedPlan);

         }
      }

      public PlanNavigationViewModel(Patient patient, Course course, PlanSetup plan, IEventAggregator eventAggregator)
      {
         _patient = patient;
         _course = course;
         _plan = plan;
         _eventAggregator = eventAggregator;
         Courses = new ObservableCollection<CourseModel>();
         Plans = new ObservableCollection<PlanModel>();
         if (patient != null)
         {
            GetCourses();
         }
      }

      private void GetCourses()
      {
         Courses.Clear();
         foreach (var course in _patient.Courses)
         {
            var cm = new CourseModel
            {
               CourseId = course.Id
            };
            foreach (var plan in course.PlanSetups)
            {
               cm.Plans.Add(new PlanModel
               {
                  PlanId = plan.Id
               });
            }
            Courses.Add(cm);
         }
         if (_course != null)
         {
            SelectedCourse = Courses.FirstOrDefault(x => x.CourseId == _course.Id);
         }
      }

      private void GetPlans()
      {
         SelectedPlan = null;
         Plans.Clear();
         foreach (var planM in SelectedCourse.Plans)
         {
            Plans.Add(new PlanModel
            {
               PlanId = planM.PlanId,
               CourseId = SelectedCourse.CourseId
            });
         }
         if (_plan != null && SelectedCourse.CourseId == _plan.Course.Id)
         {
            SelectedPlan = Plans.FirstOrDefault(x => x.PlanId == _plan.Id);
         }
      }
   }
}
