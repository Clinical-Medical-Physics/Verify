using Autofac;
using SRSConeMUVerify.Startup;
using SRSConeMUVerify.ViewModels;
using SRSConeMUVerify.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using VMS.TPS.Common.Model.API;

namespace SRSConeMUVerify
{
   /// <summary>
   /// Interaction logic for App.xaml
   /// </summary>
   public partial class App : System.Windows.Application
   {
      private VMS.TPS.Common.Model.API.Application _app;
      private string _patientId;
      private string _courseId;
      private string _planId;
      private void Application_Startup(object sender, StartupEventArgs e)
      {
         try
         {
            // TODO need to remove the null, null for versions > 11
            
            using (_app = VMS.TPS.Common.Model.API.Application.CreateApplication())
            {
               
               if (e.Args.Count() > 0 && !String.IsNullOrEmpty(e.Args.FirstOrDefault()))
               {
                  if (e.Args.First().Contains(';'))
                  {
                     _patientId = e.Args.First().Split(';').First().TrimStart('"');
                     if (e.Args.First().Split(';').Count() > 1)
                     {
                        _courseId = e.Args.First().Split(';').ElementAt(1);
                     }
                     if (e.Args.First().Split(';').Count() > 2)
                     {
                        _planId = e.Args.First().Split(';').Last().TrimEnd('"');
                     }
                  }
               }
               Patient patient = null;
               Course course = null;
               PlanSetup plan = null;
               if (!String.IsNullOrWhiteSpace(_patientId))
               {
                  patient = _app.OpenPatientById(_patientId);
               }
               if (!String.IsNullOrWhiteSpace(_courseId) && patient != null)
               {
                  course = patient.Courses.FirstOrDefault(x => x.Id == _courseId);
               }
               if (!String.IsNullOrWhiteSpace(_planId) && course != null)
               {
                  plan = course.PlanSetups.FirstOrDefault(x => x.Id == _planId);
               }
               //if patient, course, or plan are null bail out
               if (patient != null && course != null && plan != null)
               {
                  var bootstrapper = new Bootstrapper();
                  var container = bootstrapper.Bootstrap(patient, course, plan);
                  var mv = container.Resolve<MainView>();
                  //  TODO what does resolve do?
                  mv.DataContext = container.Resolve<MainViewModel>();
                  mv.ShowDialog();
               }
               else
               {
                  throw new ArgumentNullException("Plan, Course, or Patient not defined.");
               }
            }
         }
         catch (Exception ex)
         {
            string message = ex.Message;
            if (ex?.InnerException?.Message != null)
            {
               message += " " + ex.InnerException.Message;
            }
            MessageBox.Show($"Could not introduce application:\n{message}","Error",MessageBoxButton.OKCancel,
               MessageBoxImage.Error,MessageBoxResult.OK,MessageBoxOptions.DefaultDesktopOnly);
            // TODO is this necessary or is there a better way
            App.Current.Shutdown();
         }
      }
   }
}
